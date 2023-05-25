using CapaEntidad;
using CapaNegocio;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }



        //==============================================================================================================
        //   Obtener una lista de las categorias
        //==============================================================================================================
        [HttpGet]
        public JsonResult ListarCategorias() { 
            List<Categoria> lista = new CN_Categorias().ObtenerCategorias();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }



        //==============================================================================================================
        //   Obtener una lista de las marcas segun la categoria seleccionada
        //==============================================================================================================
        [HttpPost]
        public JsonResult ListarMarcasPorCategorias(string idCategoria) {
            int id = Convert.ToInt32(idCategoria);
            List<Marca> listaMarca = new CN_Marca().ListarMarcasPorCategorias(id);
            return Json(new { data = listaMarca }, JsonRequestBehavior.AllowGet);
        }



        //==============================================================================================================
        //   Obtener una lista de los productos seleccionando por marca y categoria.
        //==============================================================================================================
        [HttpPost]
        public JsonResult ListarProductos(string idCategoria, string idMarca) {
            bool conversion;
            int categoria = Convert.ToInt32(idCategoria);
            int marca = Convert.ToInt32(idMarca);
            List<Producto> lista = new CN_Producto().listarProductos().Select( p=> new Producto() { 
                IDPRODUCTO = p.IDPRODUCTO,
                NOMBRE= p.NOMBRE,
                DESCRIPCION= p.DESCRIPCION,
                MARCA= p.MARCA,
                CATEGORIA= p.CATEGORIA,
                PRECIO= p.PRECIO,
                STOCK= p.STOCK,
                RUTAIMAGEN= p.RUTAIMAGEN,
                BASE64= CN_Recursos.ConvertirBase64(System.IO.Path.Combine(p.RUTAIMAGEN, p.NOMBRE_IMAGEN), out conversion),
                EXTENSION = System.IO.Path.GetExtension(p.NOMBRE_IMAGEN),
                ACTIVO = p.ACTIVO
            }).Where(P=>
                P.CATEGORIA.IDCATEGORIA == (categoria ==0? P.CATEGORIA.IDCATEGORIA: categoria) &&
                P.MARCA.IDMARCA == (marca == 0? P.MARCA.IDMARCA: marca) &&
                P.STOCK > 0 && P.ACTIVO == 1
            ).ToList();

             var Jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
             Jsonresult.MaxJsonLength= int.MaxValue;
            return Jsonresult;
        }
    }
}