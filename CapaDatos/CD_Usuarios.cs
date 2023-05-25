using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuarios
    {

        //======================================================================================
        // METODO PARA LISTAR LOS DATOS EN DE LOS USUARIOS
        //======================================================================================
        public List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    string query = "SELECT IDUSUARIO, NOMBRES, APELLIDOS, CORREO, CLAVE, RESTABLECER, ACTIVO FROM USUARIO";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            lista.Add(
                                    new Usuario()
                                    {
                                        IDUSUARIO = Convert.ToInt32(result["IDUSUARIO"]),
                                        NOMBRES = result["NOMBRES"].ToString(),
                                        APELLIDOS = result["APELLIDOS"].ToString(),
                                        CORREO = result["CORREO"].ToString(),
                                        CLAVE = result["CLAVE"].ToString(),
                                        RESTABLECER = Convert.ToInt32(result["RESTABLECER"]),
                                        ACTIVO = Convert.ToBoolean(Convert.ToInt32(result["ACTIVO"]))
                                    }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Usuario>();
            }

            return lista;
        }






        //======================================================================================
        // METODO PARA REGISTRAR LOS DATOS EN DE LOS USUARIOS
        //======================================================================================

        public int Registrar(Usuario obj, out string Mensaje)
        {

            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", conexion);
                    cmd.Parameters.AddWithValue("NOMBRES", obj.NOMBRES);
                    cmd.Parameters.AddWithValue("APELLIDOS", obj.APELLIDOS);
                    cmd.Parameters.AddWithValue("CORREO", obj.CORREO);
                    cmd.Parameters.AddWithValue("CLAVE", obj.CLAVE);
                    cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["RESULTADO"].Value);
                    Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();

                }
            }
            catch (Exception e)
            {
                idAutogenerado = 0;
                Mensaje = e.Message;
            }

            return idAutogenerado;
        }






        //======================================================================================
        // METODO PARA EDITAR LOS DATOS EN DE LOS USUARIOS
        //======================================================================================
        public bool EditarUusuario(Usuario obj, out string Mensaje)
        {

            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {

                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", conexion);
                    cmd.Parameters.AddWithValue("IDUSUARIO", obj.IDUSUARIO);
                    cmd.Parameters.AddWithValue("NOMBRES", obj.NOMBRES);
                    cmd.Parameters.AddWithValue("APELLIDOS", obj.APELLIDOS);
                    cmd.Parameters.AddWithValue("CORREO", obj.CORREO);
                    cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();

                }
            }
            catch (Exception e)
            {
                resultado = false;
                Mensaje = e.Message;
            }

            return resultado;
        }




        //======================================================================================
        // METODO PARA ELIMINAR LOS USUARIOS
        //======================================================================================
        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM USUARIO WHERE IDUSUARIO = @ID", conexion);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    mensaje = "Usuario Eliminado correctamente";
                }


            }
            catch (Exception e)
            {
                resultado = false;
                mensaje = e.Message;

            }
            return resultado;
        }




        //======================================================================================
        // METODO PARA CAMBIAR CLAVE
        //======================================================================================
        public bool CambiarClave(int id, string nuevaClave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CLAVE = @NUEVACLAVE, RESTABLECER = 0 WHERE IDUSUARIO = @ID", conexion);

                    cmd.Parameters.AddWithValue("@NUEVACLAVE", nuevaClave);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }


            }
            catch (Exception e)
            {
                resultado = false;
                mensaje = e.Message;

            }
            return resultado;
        }





        //======================================================================================
        // METODO PARA RESTABLECER CLAVE
        //======================================================================================
        public bool RestablecerClave(int id, string Clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET CLAVE=@CLAVE, RESTABLECER=1 WHERE IDUSUARIO=@ID", conexion);

                    cmd.Parameters.AddWithValue("CLAVE", Clave);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception e)
            {
                resultado = false;
                mensaje = e.Message;

            }
            return resultado;
        }

    }
}
