using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Categorias
    {

        //======================================================================================
        // METODO PARA LISTAR LAS CATEGORIAS
        //======================================================================================
        public List<Categoria> ObtenerCategorias()
        {

            List<Categoria> lista = new List<Categoria>();

            using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
            {
                try
                {
                    string query = "SELECT IDCATEGORIA, DESCRIPCION, ACTIVO FROM CATEGORIA";
                    SqlCommand comand = new SqlCommand(query, conexion);

                    conexion.Open();

                    using (SqlDataReader result = comand.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            lista.Add(
                                new Categoria()
                                {
                                    IDCATEGORIA = Convert.ToInt32(result["IDCATEGORIA"]),
                                    DESCRIPCION = result["DESCRIPCION"].ToString(),
                                    ACTIVO = Convert.ToBoolean(result["ACTIVO"])
                                }
                                );
                        }
                    }
                    return lista;

                }
                catch (Exception)
                {
                    return lista = new List<Categoria>();
                }
            }
        }





        //======================================================================================
        // METODO PARA REGISTRAR  LAS CATEGORIAS
        //======================================================================================
        public int registrarCategoria(Categoria objCategoria, out string mensaje)
        {

            mensaje = string.Empty;
            int idAutogenerado = 0;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", conexion);

                    cmd.Parameters.AddWithValue("DESCRIPCION", objCategoria.DESCRIPCION);
                    cmd.Parameters.AddWithValue("ACTIVO", objCategoria.ACTIVO);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["RESULTADO"].Value);
                    mensaje = cmd.Parameters["MENSAJE"].Value.ToString();

                }

            }
            catch (Exception e)
            {
                mensaje = e.Message;
                idAutogenerado = 0;
            }

            return idAutogenerado;
        }





        //======================================================================================
        // METODO PARA EDITAR LAS CATEGORIAS
        //======================================================================================
        public int EditarCategoria(Categoria objCategoria, out string mensaje)
        {
            int status = 0;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", conexion);
                    cmd.Parameters.AddWithValue("IDCATEGORIA", objCategoria.IDCATEGORIA);
                    cmd.Parameters.AddWithValue("DESCRIPCION", objCategoria.DESCRIPCION);
                    cmd.Parameters.AddWithValue("ACTIVO", objCategoria.ACTIVO);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();

                    cmd.ExecuteNonQuery();
                    status = Convert.ToInt32(cmd.Parameters["RESULTADO"].Value);
                    mensaje = cmd.Parameters["MENSAJE"].Value.ToString();

                }

            }
            catch (Exception e)
            {
                mensaje = e.Message;
                status = 0;
            }

            return status;
        }





        //======================================================================================
        // METODO PARA ELIMINAR LAS MARCAS
        //======================================================================================
        public bool EliminarCategoria(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM CATEGORIA WHERE IDCATEGORIA= @ID", conexion);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    mensaje = "Categoria Eliminada correctamente";
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
