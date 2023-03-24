using CapaDatos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CN_Producto
    {
        //==============================================================================================
        // METODO PARA LISTAR LOS PRODUCTOS, LLAMANDO AL METODO "LISTARPRODUCTOS" DE LA CD_PRODUCTOS
        //==============================================================================================
        public List<Producto> listarProductos() { 

            List<Producto> productos=  new CD_Producto().ListarProductos();
            return productos;
        }


        
        public int registarProductos(Producto obj, out string mensaje) { 
            mensaje = string.Empty;
            int idGenerado = 0;

            if (string.IsNullOrEmpty(obj.NOMBRE) || string.IsNullOrWhiteSpace(obj.NOMBRE)) {
                mensaje = "el producto debe tener un nombre.";
            }
            if (string.IsNullOrEmpty(obj.DESCRIPCION) || string.IsNullOrWhiteSpace(obj.DESCRIPCION))
            {
                mensaje = "La descripcion del producto no debe ir vacia.";
            }
            if (obj.MARCA.IDMARCA == 0)
            {
                mensaje = "Debe seleccionar una marca.";
            }
            if (obj.CATEGORIA.IDCATEGORIA == 0)
            {
                mensaje = "Debe seleccionar una categoria.";
            }
            if (obj.PRECIO == 0)
            {
                mensaje = "El producto debe tener un precio.";
            }
            if (string.IsNullOrEmpty(mensaje)) {
                idGenerado = new CD_Producto().RegistrarProdcuto(obj, out mensaje);
            }
            return idGenerado;
        }



        public bool eliminarProducto(int id, out string mensaje) {
            mensaje = string.Empty;
            bool status = new CD_Producto().eliminarProducto(id, out mensaje);
            return status;
        }






        //==============================================================================================
        // METODO PARA EDITAR LOS PRODUCTOS, LLAMANDO AL METODO "EDITARPRODUCTOS" DE LA CD_PRODUCTOS
        //==============================================================================================

        public bool EditarProductos(Producto obj, out string mensaje)
        {
            mensaje = string.Empty;
            bool status = new CD_Producto().EditarProductos(obj, out mensaje);
            return status;
        }

        public bool insertarDatosImagen(Producto obj, out string mensajeImagen) {
            mensajeImagen = string.Empty;
            bool status = new CD_Producto().GuardarDatosImagen(obj, out mensajeImagen);
            return status;
        }



    }
}
