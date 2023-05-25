using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Marcas
    {

        //======================================================================================
        // METODO PARA LISTAR LAS MARCAS
        //======================================================================================
        public List<Marca> ObtenerMarcas()
        {

            List<Marca> lista = new List<Marca>();

            using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
            {
                try
                {
                    string query = "SELECT IDMARCA, DESCRIPCION, ACTIVO FROM MARCA";
                    SqlCommand comand = new SqlCommand(query, conexion);

                    conexion.Open();

                    using (SqlDataReader result = comand.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    IDMARCA = Convert.ToInt32(result["IDMARCA"]),
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
                    return lista = new List<Marca>();
                }
            }
        }





        //======================================================================================
        // METODO PARA REGISTRAR LAS MARCAS
        //======================================================================================
        public int registrarMarca(Marca objMarca, out string mensaje)
        {

            mensaje = string.Empty;
            int idAutogenerado = 0;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", conexion);

                    cmd.Parameters.AddWithValue("DESCRIPCION", objMarca.DESCRIPCION);
                    cmd.Parameters.AddWithValue("ACTIVO", objMarca.ACTIVO);
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
        // METODO PARA EDITAR LAS MARCAS
        //======================================================================================
        public int EditarMarca(Marca objMarca, out string mensaje)
        {
            int status = 0;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", conexion);
                    cmd.Parameters.AddWithValue("IDMARCA", objMarca.IDMARCA);
                    cmd.Parameters.AddWithValue("DESCRIPCION", objMarca.DESCRIPCION);
                    cmd.Parameters.AddWithValue("ACTIVO", objMarca.ACTIVO);
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
        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM MARCA WHERE IDMARCA= @ID", conexion);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                    mensaje = "Marca Eliminada correctamente";
                }


            }
            catch (Exception e)
            {
                resultado = false;
                mensaje = e.Message;

            }
            return resultado;
        }



        public List<Marca> ObtenerMarcasPorCategorias(int idCategoria)
        {

            List<Marca> lista = new List<Marca>();

            using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select distinct m.IDMARCA, m.DESCRIPCION from PRODUCTO p");
                    sb.AppendLine("inner join CATEGORIA c on c.IDCATEGORIA = p.IDCATEGORIA");
                    sb.AppendLine("inner join MARCA m on m.IDMARCA = p.IDMARCA");
                    sb.AppendLine("where c.IDCATEGORIA = iif(@idCategoria = 0, c.IDCATEGORIA, @idCategoria)");
                    SqlCommand comand = new SqlCommand(sb.ToString(), conexion);

                    comand.Parameters.AddWithValue("@idCategoria", idCategoria);
                    conexion.Open();

                    using (SqlDataReader result = comand.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            lista.Add(
                                new Marca()
                                {
                                    IDMARCA = Convert.ToInt32(result["IDMARCA"]),
                                    DESCRIPCION = result["DESCRIPCION"].ToString()                                }
                                );
                        }
                    }
                    return lista;

                }
                catch (Exception)
                {
                    return lista = new List<Marca>();
                }
            }
        }
    }
}
