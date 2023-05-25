using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;


namespace CapaDatos
{
    public class CD_Producto
    {

        //=======================================================================
        // METODO PARA LISTAR LOS PRODUCTOS EN UNA LISTA TIPO PRODUCTO
        //=======================================================================
        public List<Producto> ListarProductos() 
        {
            List<Producto> lista = new List<Producto>();

            try 
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("SELECT P.IDPRODUCTO,P.NOMBRE,P.DESCRIPCION,M.IDMARCA,M.DESCRIPCION[DescrMarca],C.IDCATEGORIA,C.DESCRIPCION,P.PRECIO,P.STOCK,P.RUTAIMAGEN,P.NOMBREIMAGEN,P.ACTIVO").ToString();
                    query.AppendLine("FROM PRODUCTO AS P INNER JOIN MARCA AS M ON(P.IDMARCA = M.IDMARCA ) INNER JOIN CATEGORIA AS C ON(P.IDCATEGORIA = C.IDCATEGORIA)").ToString();

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    conexion.Open();

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {

                        while (result.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IDPRODUCTO = Convert.ToInt32(result["IDPRODUCTO"]),
                                NOMBRE = result["NOMBRE"].ToString(),
                                DESCRIPCION = result["DESCRIPCION"].ToString(),
                                MARCA = new Marca()
                                {
                                    IDMARCA = Convert.ToInt32(result["IDMARCA"]),
                                    DESCRIPCION = result["DescrMarca"].ToString(),
                                },
                                CATEGORIA = new Categoria()
                                {
                                    IDCATEGORIA = Convert.ToInt32(result["IDCATEGORIA"]),
                                    DESCRIPCION = result["DESCRIPCION"].ToString()
                                },
                                PRECIO = Convert.ToDecimal(result["PRECIO"], new CultureInfo("es-CO")),
                                STOCK = Convert.ToInt32(result["STOCK"]),
                                RUTAIMAGEN = result["RUTAIMAGEN"].ToString(),
                                NOMBRE_IMAGEN = result["NOMBREIMAGEN"].ToString(),
                                ACTIVO = Convert.ToInt32(result["ACTIVO"])

                            }) ;

                        }

                    }
                }
            } catch(Exception e) 
            { 
                lista  = new List<Producto>();
                string mensaje = e.Message;
            }

            return lista;
        }


        //=======================================================================
        // REGISTRA UN PRODUCTO EN LA BASE DE DATOS
        //=======================================================================
        public int RegistrarProdcuto(Producto obj, out string mensaje) {
            int idGenerado = 0;
            mensaje = string.Empty;

            try
            {


                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_Registrar_Producto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("NOMBRE", obj.NOMBRE);
                    cmd.Parameters.AddWithValue("DESCRIPCION", obj.DESCRIPCION);
                    cmd.Parameters.AddWithValue("IDMARCA", obj.MARCA.IDMARCA);
                    cmd.Parameters.AddWithValue("IDCATEGORIA", obj.CATEGORIA.IDCATEGORIA);
                    cmd.Parameters.AddWithValue("PRECIO", obj.PRECIO);
                    cmd.Parameters.AddWithValue("STOCK", obj.STOCK);
                    cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    idGenerado = Convert.ToInt32(cmd.Parameters["RESULTADO"].Value);
                    mensaje = cmd.Parameters["MENSAJE"].Value.ToString();



                }
            }
            catch (Exception e)
            {
                idGenerado = 0;
                mensaje = e.Message;
            }
            return idGenerado;

        }




        //=======================================================================
        // METODO PARA EDITAR LOS PRODUCTOS
        //=======================================================================

        public bool EditarProductos(Producto obj, out string mensaje) 
        {
            bool status = false;
            try
            {


                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("IDPRODUCTO", obj.IDPRODUCTO);
                    cmd.Parameters.AddWithValue("NOMBRE", obj.NOMBRE);
                    cmd.Parameters.AddWithValue("DESCRIPCION", obj.DESCRIPCION);
                    cmd.Parameters.AddWithValue("IDMARCA", obj.MARCA.IDMARCA);
                    cmd.Parameters.AddWithValue("IDCATEGORIA", obj.CATEGORIA.IDCATEGORIA);
                    cmd.Parameters.AddWithValue("PRECIO", obj.PRECIO);
                    cmd.Parameters.AddWithValue("STOCK", obj.STOCK);
                    cmd.Parameters.AddWithValue("ACTIVO", obj.ACTIVO);
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    status = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    mensaje = cmd.Parameters["MENSAJE"].Value.ToString();



                }
            }
            catch (Exception e)
            {
                status = false;
                mensaje = e.Message;
            }
            return status;

        }


        //=======================================================================
        // METODO ELIMINAR LOS PRODUCTOS
        //=======================================================================

        public bool eliminarProducto(int id, out string mensaje) 
        {
                bool status = false;
            mensaje = string.Empty;
            try 
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    string query = "delete from producto where IDPRODUCTO = @ID";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    status = true;
                    mensaje = "Producto eliminado correctamente.";
                }
            }
            catch(Exception e) 
            { 
                status = false;
                mensaje = e.Message;
            }
            return status;
        }





        //=======================================================================
        // METODO PARA INSERTAR LOS DATOS DE LA IMAGEN EN LA BASE DE DATOS
        //=======================================================================
        public bool GuardarDatosImagen(Producto obj, out string mensajeImagen) 
        { 
            bool status = false;
            mensajeImagen = string.Empty;

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    string query = "UPDATE PRODUCTO SET RUTAIMAGEN = @RUTAIMAGEN, NOMBREIMAGEN=@NOMBREIMAGEN WHERE IDPRODUCTO = @IDPRODUCTO ";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.AddWithValue("@RUTAIMAGEN", obj.RUTAIMAGEN);
                    cmd.Parameters.AddWithValue("@NOMBREIMAGEN", obj.NOMBRE_IMAGEN);
                    cmd.Parameters.AddWithValue("@IDPRODUCTO", obj.IDPRODUCTO);
                    conexion.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        status = true;
                    }
                    else {
                        mensajeImagen = "No se pudo actualizar imagen";
                    }

                }
            }
            catch (Exception e)
            {
                mensajeImagen = e.Message;
                status = false;
            }
            return status;
        }


    }
}
