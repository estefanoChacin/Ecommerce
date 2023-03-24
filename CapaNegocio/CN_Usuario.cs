using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuarios objCapaDatos = new CD_Usuarios();

        public List<Usuario> listar()
        {

            return objCapaDatos.Listar();
        }




        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.NOMBRES) || string.IsNullOrWhiteSpace(obj.NOMBRES))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";

            }
            else if (string.IsNullOrEmpty(obj.APELLIDOS) || string.IsNullOrWhiteSpace(obj.APELLIDOS))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.CORREO) || string.IsNullOrWhiteSpace(obj.CORREO))
            {
                Mensaje = "el correo del usuario no puede ser vacia";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();
                string mensaje = "<h3>Su cuenta fue creada correctamente</h3> </br> <p> su contraseña para acceder es:" + clave + "</p>";
                string correo = obj.CORREO;

                bool status = CN_Recursos.EnviarCorreo(correo, "Creacion de cuenta", mensaje);

                if (status)
                {
                    obj.CLAVE = CN_Recursos.ConvertirSha256(clave);

                }
                else
                {
                    mensaje = "No se puede enviar el correo";
                    return 0;
                }
                return objCapaDatos.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }




        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.NOMBRES) || string.IsNullOrWhiteSpace(obj.NOMBRES))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";

            }
            else if (string.IsNullOrEmpty(obj.APELLIDOS) || string.IsNullOrWhiteSpace(obj.APELLIDOS))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.CORREO) || string.IsNullOrWhiteSpace(obj.CORREO))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDatos.EditarUusuario(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }



        public bool Eliminar(int id, out string mensaje)
        {

            return objCapaDatos.Eliminar(id, out mensaje);
        }




        public bool CambiarClave(int id, string nuevaClave, out string mensaje)
        {

            return objCapaDatos.CambiarClave(id, nuevaClave, out mensaje);
        }




        public bool RestablecerClave(int id, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            string claveEncriptada = CN_Recursos.ConvertirSha256(nuevaClave);

            bool resultado = objCapaDatos.RestablecerClave(id, nuevaClave, out mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3> </br> <p> su contraseña para acceder es:" + nuevaClave + "</p>";

                bool status = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                if (status)
                {
                    return true;
                }
                else
                {
                    return false;
                    mensaje = "no se pudo enviar el correo";
                }
            }
            else 
            {
                return false;
                mensaje = "no se pudo reestablecer la clave";
            }
        }




    }
}
