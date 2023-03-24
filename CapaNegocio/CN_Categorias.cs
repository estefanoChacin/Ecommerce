using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categorias
    {
        private CD_Categorias objCategoria = new CD_Categorias();


        public  List<Categoria> ObtenerCategorias() {

            return objCategoria.ObtenerCategorias();
        }


        public int RegistrarCategoria(Categoria categoria, out string mensaje) {
            mensaje = string.Empty;
            int resultado = 0;

            if (string.IsNullOrEmpty(categoria.DESCRIPCION) || string.IsNullOrWhiteSpace(categoria.DESCRIPCION)) {
                mensaje = "La categoria debe tener una Descripcion";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                resultado = new CD_Categorias().registrarCategoria(categoria, out mensaje);
            }

            return resultado;
        }



        public int EditarCategoria(Categoria categoria, out string mensaje) {
            mensaje = string.Empty;
            int resultado = 0;

            if (string.IsNullOrEmpty(categoria.DESCRIPCION) || string.IsNullOrWhiteSpace(categoria.DESCRIPCION))
            {
                mensaje = "La categoria debe tener una Descripcion";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                resultado = new CD_Categorias().EditarCategoria(categoria, out mensaje);
            }

            return resultado;
        }



        public bool EliminarCategoria(int id, out string mensaje) { 
            mensaje =string.Empty;
            bool resultado = new CD_Categorias().EliminarCategoria(id, out mensaje);
            return resultado;
        }
    }
}
