using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cliente
    {
        public int IDCLIENTE { get; set; }
        public string NOMBRES { get; set; }
        public string APELLIDOS { get; set; }
        public string CORREO { get; set; }
        public string CLAVE { get; set; }
        public bool RESTABLECER { get; set; }
        public bool ACTIVO { get; set; }

    }
}
