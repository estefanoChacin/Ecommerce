using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Provincia
    {
        public int IDPROVINCIA { get; set; }
        public string DESCRIPCION { get; set; }
        public Departamento IDDEPARTAMENTO { get; set; }


    }
}
