using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int IDPRODUCTO { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public Marca MARCA { get; set; }
        public Categoria CATEGORIA { get; set; }
        public decimal  PRECIO { get; set; }
        public string PRECIOTEXTO { get; set; }
        public int STOCK { get; set; }
        public string RUTAIMAGEN { get; set; }
        public string NOMBRE_IMAGEN { get; set; }
        public int ACTIVO { get; set; }
        public int BASE64 { get; set; }
        public int EXTENSION { get; set; }



    }
}
