using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {

        public ActionResult Index() 
        { 
            return View();
        }
        
        
        
        
        // post: acceso
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
            else {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult RestablecerClave()
        {
            return View();
        }
    }
}