using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_DashBoard
    {
        public DashBoard VerReporte() { 
            return new CD_Reporte().VerRegistro();
        }

        public List<Reporte> ReporteVentas(string fechaInicio, string fechaFin, string IDtransaccion) {
            return new CD_Reporte().reporteTabla(fechaInicio, fechaFin, IDtransaccion);
        }
    }
}
