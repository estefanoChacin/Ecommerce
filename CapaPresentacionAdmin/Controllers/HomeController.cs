using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTable = System.Data.DataTable;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }





        //==============================================================================
        //      USUARIOS (LISTAR-GAUARDAR-EDITAR- ELIMINAR)
        //==============================================================================

        public JsonResult ListarUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            usuarios = new CN_Usuario().listar();

            return Json(new { data = usuarios }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario obj)
        {
            object resultado;
            string mensaje = string.Empty;
            if (obj.IDUSUARIO == 0)
            {
                resultado = new CN_Usuario().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Usuario().EditarUsuario(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [Route("imagen")]
        public JsonResult Eliminar(Usuario obj)
        {
            object uu = obj;
            string mensaje = string.Empty;
            object resultado = new CN_Usuario().Eliminar(obj.IDUSUARIO, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje });
        }





        //==============================================================================
        //     REPORTE DASHBOARD
        //==============================================================================
        public JsonResult VerReporte()
        {
            DashBoard dashboard = new CN_DashBoard().VerReporte();

            return Json(new { resultado = dashboard }, JsonRequestBehavior.AllowGet);
        }




        //======================================================================================
        //    RETORNA UN JSON DE LAS VENTAS CON LOS FILTROS APLICADOS PARA LLENAR LA TABLA 
        //======================================================================================
        public JsonResult VerReporteVentas(string fechaInicio, string fechaFinal, string IdTransaccion)
        {
            List<Reporte> lista = new CN_DashBoard().ReporteVentas(fechaInicio, fechaFinal, IdTransaccion);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }





        //==============================================================================
        //     EXPORTAR LOS REPORTES EN UN ARCHIVO EN FORMATO EXCEL
        //==============================================================================

        [HttpPost]
        public FileResult ExportarReporte(string fechaInicio, string fechaFin, string IDtRANSACCION)
        {
            List<Reporte> listaReporte = new CN_DashBoard().ReporteVentas(fechaInicio, fechaFin, IDtRANSACCION);

            DataTable dt = new DataTable();
            dt.Locale = new CultureInfo("en-CO");
            dt.Columns.Add("Fecha venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTtransaccion", typeof(string));

            foreach (Reporte rp in listaReporte)
            {
                dt.Rows.Add(new object[] {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }
            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}