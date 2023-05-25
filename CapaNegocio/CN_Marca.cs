using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marcas objMarca = new CD_Marcas();

        //==============================================================================================================
        //   OBTIENE UNA LISTA DE TODAS LA MARCAS ACTUALMENTE REGISTRADAS EN LA BD
        //==============================================================================================================
        public List<Marca> Obtenermarcas()
        {

            return objMarca.ObtenerMarcas();
        }


        //==============================================================================================================
        //   REGISTRAR LAS MARCAS EN LA BASE DE DATOS
        //==============================================================================================================
        public int RegistrarMarca(Marca marca, out string mensaje)
        {
            mensaje = string.Empty;
            int resultado = 0;

            if (string.IsNullOrEmpty(marca.DESCRIPCION) || string.IsNullOrWhiteSpace(marca.DESCRIPCION))
            {
                mensaje = "La categoria debe tener una Descripcion";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                resultado = new CD_Marcas().registrarMarca(marca, out mensaje);
            }

            return resultado;
        }


        //==============================================================================================================
        //   EDITAR LAS MARCAS EN LA BASE DE DATOS
        //==============================================================================================================
        public int EditarMarca(Marca marca, out string mensaje)
        {
            mensaje = string.Empty;
            int resultado = 0;

            if (string.IsNullOrEmpty(marca.DESCRIPCION) || string.IsNullOrWhiteSpace(marca.DESCRIPCION))
            {
                mensaje = "La marca debe tener una Descripcion";
            }
            if (string.IsNullOrEmpty(mensaje))
            {
                resultado = new CD_Marcas().EditarMarca(marca, out mensaje);
            }

            return resultado;
        }


        //==============================================================================================================
        //   PERMITE ELIMINAR UNA MARCA DE LA BD SEGUN EL ID SELECCIONADO
        //==============================================================================================================
        public bool Eliminar(int id, out string mensaje) { 
            mensaje = string.Empty;
            bool status = false;

            status = new CD_Marcas().Eliminar(id, out mensaje);

            return status;
        }

        //==============================================================================================================
        //   PERMITE LISTAR LAS MARCAS SEGUN LA CATEGORIA SELECCIONADA
        //==============================================================================================================
        public List<Marca> ListarMarcasPorCategorias(int idCategoria)
        {
            return objMarca.ObtenerMarcasPorCategorias(idCategoria);
        }
    }
}
