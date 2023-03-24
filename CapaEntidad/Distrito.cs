using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Distrito
    {
        public int IDDISTRITO { get; set; }
        public string DESCRIPCION { get; set; }
        public Provincia IDPROVINCIA { get; set; }
        public Departamento IDDEPARTAMENTO{ get; set; }
    }
}
