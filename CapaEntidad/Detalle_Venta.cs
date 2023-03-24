using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Detalle_Venta
    {
        public int IDDETALLE { get; set; }
        public Venta IDVENTA { get; set; }
        public Producto IDPRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
        public double TOTAL { get; set; }




    }
}
