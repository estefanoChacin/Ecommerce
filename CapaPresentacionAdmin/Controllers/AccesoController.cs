using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Web.Security;
using System.Net;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {

        //==============================================================================
        //      REDIRECCIONA AL INDEX
        //==============================================================================
        public ActionResult Index()
        {
            return View();
        }



        //==========================================================================================
        //      CONTRALADOR QUE RECIBE LAS CREDENCIALES Y SI ESTAN CORRECTAS REDIRECIONA (LOGUEO)
        //==========================================================================================
        [HttpPost]
        public ActionResult Index(string email, string clave)
        {
            Usuario usuario = new Usuario();

            usuario = new CN_Usuario().listar().Where(usu => usu.CORREO == email && usu.CLAVE == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta.";
                return View();
            }
            else
            {


                if (usuario.RESTABLECER == 1)
                {
                    TempData["IDusuario"] = usuario.IDUSUARIO;
                    return RedirectToAction("CambiarClave");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(usuario.CORREO, false);

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Home");
                }
            }
        }






        //==============================================================================
        //     REDIRECCIONA A LA VISTA 
        //==============================================================================
        public ActionResult CambiarClave()
        {
            return View();
        }


        //==============================================================================
        //     RECIBE LAS CREDENCIALES Y ACTUALIZA LA CONTRASEÑA DEL USUARIO 
        //==============================================================================
        [HttpPost]
        public ActionResult CambiarClave(string IdUsuario, string ClaveActual, string NuevaClave, string confirmarClave)
        {
            Usuario usuario = new Usuario();
            string mensaje = string.Empty;
            usuario = new CN_Usuario().listar().Where(usu => usu.IDUSUARIO == Convert.ToInt32(IdUsuario)).FirstOrDefault();

            if (usuario.CLAVE != CN_Recursos.ConvertirSha256(ClaveActual))
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
                bool status = new CN_Usuario().CambiarClave(Convert.ToInt32(IdUsuario), CN_Recursos.ConvertirSha256(NuevaClave), out mensaje);
                TempData["mensaje"] = mensaje;
                return RedirectToAction("Index");
            }

        }






        //==============================================================================================================
        //   RECIBE UN UNA DIRECCION DE CORREO, VALIDA, Y REDIRIGE AL FORMULARIO DE CAMBIAR CONTRASEÑA PARA RESTABLECER
        //==============================================================================================================
        public ActionResult RestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RestablecerClave(string correoElectronico)
        {
            string mensaje = string.Empty;
            Usuario usuario = new CN_Usuario().listar().Where(usu => usu.CORREO == correoElectronico).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "El correo ingresado no se encuntra registrado.";
                return View();
            }
            else
            {
                bool status = new CN_Usuario().RestablecerClave(usuario.IDUSUARIO, correoElectronico, out mensaje);
                if (status)
                {
                    TempData["restablecer"] = "correo enviado, por favor revise su bandeja de entrada.";
                    TempData["IDusuario"] = usuario.IDUSUARIO;
                    return RedirectToAction("CambiarClave");
                }
                else
                {
                    ViewBag.Error = "hubo un problema enviando el correo con la nueva contraseña.";
                    return View();
                }
            }
        }




        //==============================================================================================================
        //   CIERRA LA SESION Y DESTRUYE LA COOCKIE DE AUTORIAZACION
        //==============================================================================================================
        public ActionResult CerrarSession() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}