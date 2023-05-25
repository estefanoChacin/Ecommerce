using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;

namespace CapaDatos
{
    public class CD_Cliente
    {

        //======================================================================================
        // METODO PARA LISTAR  LOS CLIENTES
        //======================================================================================
        public List<Cliente> Listar()
        {

            List<Cliente> listaClientes = new List<Cliente>();

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    string query = "SELECT IDCliente, NOMBRES, APELLIDOS, CORREO, CLAVE, RESTABLECER, ACTIVO FROM CLIENTE";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = System.Data.CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            listaClientes.Add(
                                    new Cliente()
                                    {
                                        IDCLIENTE = Convert.ToInt32(result["IDCliente"]),
                                        NOMBRES = result["NOMBRES"].ToString(),
                                        APELLIDOS = result["APELLIDOS"].ToString(),
                                        CORREO = result["CORREO"].ToString(),
                                        CLAVE = result["CLAVE"].ToString(),
                                        RESTABLECER = Convert.ToBoolean(result["RESTABLECER"]),
                                        ACTIVO = Convert.ToBoolean(Convert.ToInt32(result["ACTIVO"]))
                                    }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                listaClientes = new List<Cliente>();
            }

            return listaClientes;
        }






        //======================================================================================
        // METODO PARA REGISTRAR LOS DATOS EN DE LOS CLIENTES
        //======================================================================================

        public int Registrar(Cliente obj, out string Mensaje)
        {

            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {

                    SqlCommand cmd = new SqlCommand("pa_RegistrarCliente", conexion);
                    cmd.Parameters.AddWithValue("NOMBRES", obj.NOMBRES);
                    cmd.Parameters.AddWithValue("APELLIDOS", obj.APELLIDOS);
                    cmd.Parameters.AddWithValue("CORREO", obj.CORREO);
                    cmd.Parameters.AddWithValue("CLAVE", obj.CLAVE);
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
        // METODO PARA EDITAR LOS DATOS EN DE LOS CLIENTES
        //======================================================================================
        public bool EditarUCliente(Cliente obj, out string Mensaje)
        {

            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {

                    SqlCommand cmd = new SqlCommand("sp_EditarCliente", conexion);
                    cmd.Parameters.AddWithValue("IDCliente", obj.IDCLIENTE);
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
        // METODO PARA ELIMINAR LOS CLIENTES
        //======================================================================================
        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Cliente WHERE IDCliente = @ID", conexion);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    mensaje = "Cliente Eliminado correctamente";
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
                    SqlCommand cmd = new SqlCommand("UPDATE Cliente SET CLAVE = @NUEVACLAVE, RESTABLECER = 0 WHERE IDCliente = @ID", conexion);

                    cmd.Parameters.AddWithValue("@NUEVACLAVE", nuevaClave);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                mensaje = "Contraseña reestablecida, por favor vuelva a iniciar sesión";

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
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CLAVE=@CLAVE, RESTABLECER=1 WHERE IDCliente=@ID", conexion);

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

