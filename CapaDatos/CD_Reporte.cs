using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Reporte
    {
        private DashBoard objecto = new DashBoard();
        private Reporte reporte = new Reporte();


        //======================================================================================
        // METODO PARA LISTAR LOS DATOS EN DE LOS PRODUCTOS, CLIENTES Y VENTAS EN EL DASHBOARD
        //======================================================================================
        public DashBoard VerRegistro()
        {

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader resultado = cmd.ExecuteReader())
                    {
                        while (resultado.Read())
                        {
                            objecto = new DashBoard()
                            {
                                TotalCliente = Convert.ToInt32(resultado["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(resultado["TotalVenta"]),
                                TotalProductos = Convert.ToInt32(resultado["TotalProducto"])
                            };

                        }
                    }
                }

            }
            catch
            {
                objecto = new DashBoard();
            }

            return objecto;
        }




        //=======================================================================
        // METODO PARA LISTAR LOS DATOS EN LA TABLA DEL DASHBOARD
        //=======================================================================
        public List<Reporte> reporteTabla(string fechaInicio, string fechaFin, string IdTransaccion)
        {
            List<Reporte> listReporte = new List<Reporte>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.getConexion))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("IDTRANSACCION", IdTransaccion);
                    cmd.Parameters.AddWithValue("FECHAINICIO", fechaInicio);
                    cmd.Parameters.AddWithValue("FECHAFIN", fechaFin);


                    conexion.Open();

                    using (SqlDataReader resultado = cmd.ExecuteReader())
                    {
                        while (resultado.Read())
                        {
                            listReporte.Add(
                                    new Reporte()
                                    {
                                        FechaVenta = resultado["FECHAVENTA"].ToString(),
                                        Cliente = resultado["CLIENTE"].ToString(),
                                        Producto = resultado["PRODUCTO"].ToString(),
                                        Precio = Convert.ToDecimal(resultado["PRECIO"], new CultureInfo("es-CO")),
                                        Cantidad = Convert.ToInt32(resultado["CANTIDAD"]),
                                        Total = Convert.ToDecimal(resultado["TOTAL"], new CultureInfo("es-CO")),
                                        IdTransaccion = resultado["IDTRANSACCION"].ToString()
                                    });
                        }
                    }
                }

            }
            catch
            {
                reporte = new Reporte();
            }

            return listReporte;

        }



    }
}
