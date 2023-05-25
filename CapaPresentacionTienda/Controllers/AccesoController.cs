using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }


        [HttpPost]

        //==============================================================================
        //      REGISTRA LOS CLIENTES EN LA BASE DE DATOS
        //==============================================================================
        public ActionResult Registrar(Cliente objecto)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["NOMBRES"] = string.IsNullOrEmpty(objecto.NOMBRES) ? "" : objecto.NOMBRES;
            ViewData["APELLIDOS"] = string.IsNullOrEmpty(objecto.APELLIDOS) ? "" : objecto.APELLIDOS;
            ViewData["CORREO"] = string.IsNullOrEmpty(objecto.CORREO) ? "" : objecto.CORREO;

            if (objecto.CLAVE != objecto.CONFIRMAR_CLAVE)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            else
            {
                resultado = new CN_Cliente().Registrar(objecto, out mensaje);
                ViewBag.Error = null;
                TempData["iniciar"] = mensaje;
                return RedirectToAction("Index", "Acceso");
            }
        }




        //======================================================================================================================================================
        //    VALIDA LAS CREDENCIALES DEL CLIENTE Y LE PERMITE EL ACCESO, SI ES 1 VEZ ENVIA AL FORMUALRIO DE CAMBAIR CONTRASEÑA DE LO CONTRARIO AL INDEX
        //======================================================================================================================================================
        [HttpPost]
        public ActionResult Index(string correo, string clave) { 
            string calveee = CN_Recursos.ConvertirSha256(clave);
            Cliente cliente = new CN_Cliente().listarClientes().Where(cli => cli.CORREO == correo && cli.CLAVE == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecto.";
                return View();
            }
            else
            {
                if (cliente.RESTABLECER)
                {
                    TempData["IdCliente"] = cliente.IDCLIENTE;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(cliente.CORREO, false);
                    Session["Cliente"] = cliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }





        //========================================================================================================================
        //      RECIBE UNA DIRECCION DE CORREO ELECTRONICO, VALIDA, ENVIA CORREO Y REDIRECCIONA AL FORM DE CAMBIAR CONTRASEÑA
        //========================================================================================================================
        [HttpPost]
        public ActionResult Reestablecer(string correoElectronico)
        {
            string mensaje = string.Empty;
            Cliente cliente = new  CN_Cliente().listarClientes().Where(usu => usu.CORREO == correoElectronico).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "El correo ingresado no se encuntra registrado.";
                return View();
            }
            else
            {
                bool status = new CN_Cliente().RestablecerClave(cliente.IDCLIENTE, correoElectronico, out mensaje);
                if (status)
                {
                    TempData["restablecer"] = "correo enviado, por favor revise su bandeja de entrada.";
                    TempData["IDCliente"] = cliente.IDCLIENTE;
                    return RedirectToAction("CambiarClave");
                }
                else
                {
                    ViewBag.Error = "hubo un problema enviando el correo con la nueva contraseña.";
                    return View();
                }
            }
        }


        //==============================================================================================
        //      RECIBE LA CONTRASELA ACTUAL Y VALIDA LAS NUEVAS, ACTUALIZA LA CONTRASEÑA DEL CLIENTE
        //==============================================================================================
        [HttpPost]
        public ActionResult CambiarClave(string IdCliente, string ClaveActual, string NuevaClave, string confirmarClave)
        {
            Cliente cliente = new Cliente();
            string mensaje = string.Empty;
            cliente = new CN_Cliente().listarClientes().Where(usu => usu.IDCLIENTE == Convert.ToInt32(IdCliente)).FirstOrDefault();

            if (cliente.CLAVE != CN_Recursos.ConvertirSha256(ClaveActual))
            {
                ViewBag.ErrorClave = "La contraseña actual no es correcta.";
                return View();
            }
            else if (NuevaClave != confirmarClave)
            {
                ViewBag.ErrorClave = "Las contraseñas no coinciden.";
                return View();
            }
            else
            {
                bool status = new CN_Cliente().CambiarClave(Convert.ToInt32(IdCliente), CN_Recursos.ConvertirSha256(NuevaClave), out mensaje);
                TempData["mensaje"] = mensaje;
                return RedirectToAction("Index");
            }

        }

        //==============================================================================================================
        //   CIERRA LA SESION Y DESTRUYE LA COOCKIE DE AUTORIAZACION
        //==============================================================================================================
        public ActionResult CerrarSession()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}