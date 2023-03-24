using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int IDVENTA { get; set; }
        public Cliente IDCLIENTE { get; set; }
        public int TOTALPRODUCTO { get; set; }
        public decimal MONTOTOTAL { get; set; }
        public string CONTACTO { get; set; }
        public Distrito IDDISTRITO { get; set; }
        public int TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public int IDTRANSACCION { get; set; }
    }
}
