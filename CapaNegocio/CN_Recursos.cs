using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;


namespace CapaNegocio
{
    public class CN_Recursos
    {

        public static string GenerarClave() { 
            string clave = Guid.NewGuid().ToString("N").Substring(0,8);
            return clave;
        }

        public static string ConvertirSha256(string texto) { 
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create()) { 
                Encoding encoding= Encoding.UTF8;
                byte[] result = hash.ComputeHash(encoding.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }



        public  static bool EnviarCorreo(string correo, string asunto, string mensaje) { 
            bool resultado = false;

            try { 
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("parssivals22@gmail.com");
                mail.To.Add(correo);
                mail.Subject = asunto;
                mail.Body= mensaje;
                mail.IsBodyHtml= true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("parssivals22@gmail.com", "yugvyfpwgrupaboi"),
                    Host= "smtp.gmail.com",
                    Port =587,
                    EnableSsl= true,
                };
                smtp.Send(mail);
                resultado = true;

            } catch(Exception e) { 
                resultado= false;
            }
             return resultado;
        }


        public static string ConvertirBase64(string ruta, out bool conversion) { 
        
            string texto64 = string.Empty;
            conversion = true;
            try {
                byte[] bytes = File.ReadAllBytes(ruta);
                texto64= Convert.ToBase64String(bytes);
            
            }catch{
                conversion= false;
            }
            return texto64;
        }

    }
}
