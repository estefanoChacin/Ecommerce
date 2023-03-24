using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int IDCARRITO { get; set; }
        public  Cliente IDCLIENTE { get; set; }
        public Producto IDPRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
    }
}
