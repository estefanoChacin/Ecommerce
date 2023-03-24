using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categorias()
        {
            return View();
        }

        public ActionResult Marcas()
        {
            return View();
        }

        public ActionResult Productos()
        {
            return View();
        }


        //==============================================================================
        //      CATEGORIAS (LISTAR-GAUARDAR-EDITAR- ELIMINAR)
        //==============================================================================
        public JsonResult ListarCategoria()
        {

            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categorias().ObtenerCategorias();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(Categoria obj)
        {
            int resultado = 0;
            string mensaje = string.Empty;

            if (obj.IDCATEGORIA == 0)
            {
                resultado = new CN_Categorias().RegistrarCategoria(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Categorias().EditarCategoria(obj, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarCategoria(Categoria obj)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Categorias().EliminarCategoria(obj.IDCATEGORIA, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }





        //==============================================================================
        //      MARCAS (LISTAR-GAUARDAR-EDITAR- ELIMINAR)
        //==============================================================================

        public JsonResult listarMarcas()
        {
            List<Marca> lista = new CN_Marca().Obtenermarcas();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarMarcas(Marca obj)
        {
            int resultado = 0;
            string mensaje = string.Empty;

            if (obj.IDMARCA == 0)
            {
                resultado = new CN_Marca().RegistrarMarca(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().EditarMarca(obj, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult eliminarMarca(Marca obj)
        {

            string mensaje = string.Empty;
            bool resultado = new CN_Marca().Eliminar(obj.IDMARCA, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }




        //==============================================================================
        //      PRODUCTOS (LISTAR-GAUARDAR-EDITAR- ELIMINAR)
        //==============================================================================

        public JsonResult listarProductos()
        {
            List<Producto> lista = new CN_Producto().listarProductos();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult GuardarProductos(string objecto, HttpPostedFileBase imagen)
        {
            string mensaje = string.Empty;
            string mensajeImagen = string.Empty;
            int idGenerado = 0;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto obj = new Producto();
            obj = JsonConvert.DeserializeObject<Producto>(objecto);

            decimal precio;

            if (decimal.TryParse(obj.PRECIOTEXTO, NumberStyles.AllowDecimalPoint, new CultureInfo("es-CO"), out precio))
            {
                obj.PRECIO = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "formato de precio no valido." }, JsonRequestBehavior.AllowGet);
            }


            if (obj.IDPRODUCTO == 0)
            {
                idGenerado = new CN_Producto().registarProductos(obj, out mensaje);
                if (idGenerado != 0)
                {
                    obj.IDPRODUCTO = idGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }

            }
            else
            {
                operacion_exitosa = new CN_Producto().EditarProductos(obj, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (imagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["rutaImagen"];
                    string extesion = Path.GetExtension(imagen.FileName);
                    string nombre_imagen = string.Concat(obj.IDPRODUCTO.ToString(), extesion);

                    try
                    {
                        imagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));

                    }
                    catch (Exception e)
                    {
                        string mensj = e.Message;
                        guardar_imagen_exito = false;
                    }
                    if (guardar_imagen_exito)
                    {
                        obj.RUTAIMAGEN = ruta_guardar;
                        obj.NOMBRE_IMAGEN = nombre_imagen;
                        bool respuesta = new CN_Producto().insertarDatosImagen(obj, out mensajeImagen);
                    }
                    else
                    {
                        mensaje = "se guardo el producto pero hubo problemas con la imagen.";
                    }
                }
            }

            return Json(new { resultado = operacion_exitosa, idGenerado = idGenerado, mensaje = mensaje });
        }





        [HttpPost]
        public JsonResult eliminarProductos(Producto obj)
        {
            string mensaje = string.Empty;
            bool resultado = new CN_Producto().eliminarProducto(obj.IDPRODUCTO, out mensaje);
            string imagen = obj.RUTAIMAGEN + "/" + obj.NOMBRE_IMAGEN;

            FileInfo file = new FileInfo(imagen);

            if (file.Exists)
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                    mensaje = "Problemas al intentar eliminar la imagen.";
                }
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }





        //==============================================================================
        //      METODO PARA CONVERTIR IMAGEN A BASE64 PARA MOSTRAR EN EL FRONT-END
        //==============================================================================
        [HttpPost]
        public JsonResult ImagenProducto(int ID)
        {
            bool conversion;
            Producto produc = new CN_Producto().listarProductos().Where(P => P.IDPRODUCTO == ID).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(produc.RUTAIMAGEN, produc.NOMBRE_IMAGEN), out conversion);
            string nombreImagen = Path.GetExtension(produc.NOMBRE_IMAGEN);
            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = nombreImagen
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
