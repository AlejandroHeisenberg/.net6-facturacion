using Microsoft.AspNetCore.Mvc;
using ProyectoVenta.Datos;
using ProyectoVenta.Models;
using System.Xml.Linq;

using Microsoft.AspNetCore.Authorization;

namespace ProyectoVenta.Controllers
{
    //Un controlador se puede bloquear, pero permitir el acceso anónimo y no autenticado a acciones individuales
    [Authorize]
    public class HomeController : Controller
    {
        //creamos una instancia para poder acceder y utilizar los metodos basicos de un crud, estos se encuentran en la carpeta Datos/DA_Producto.cs
        DA_Producto _daProducto = new DA_Producto( );
        //creamos una instancia para poder acceder y utilizar los metodos basicos de un crud, estos se encuentran en la carpeta Datos/DA_Venta.cs
        DA_Venta _daVenta = new DA_Venta( );

        public IActionResult Index()
        {
            //retornamos a la vista del usuario despues de iniciar sesion, nuestro index
            return View( );
        }

        //metodo de detalle que realiza la accion de redireccionar a la vista de detalle
        public IActionResult DetalleVenta()
        {
            return View( );
        }
        //metodo publico que me redirecciona a la vista index de privacy 
        public IActionResult Privacy()
        {
            return View( );
        }

        //metodo de tipo get
        [HttpGet]
        //Este metodo es llamado desde el front por medio de una peticion ajax
        //Para el campo de texto buscar producto lo cual sirve para autocompletar la busqueda
        public JsonResult AutoCompleteProducto( string search )
        {
            //realizamos una instancia por medio de la variable autocomplete que sera de tipo List autocompletable
            List<Autocomplete> autocomplete = new List<Autocomplete>( );
            //en autocomplete guardaremos los datos obtenidos al usar el metodo de listar() de la clase DA_Producto cuando el ususario usa el buscador
            //y almacena palabras que inicien con las palabras que ingreso el producto en el campo de texto de buscar

            //mediante un string concatenaremos los valores del codigo, descripcion de la categoria, descripcion del producto, en mayusculas, todo esto aparecera
            //conforme al texto que este contenido en el campo de buscar y por ultimo seleccionamos los label o nombre de las cabeceras y los valores de los mismos

            //Un método.Where( ) filtrará las filas devueltas.

            //Un método.Select( ) filtrará las columnas devueltas.

            autocomplete = _daProducto.Listar( )
                .Where(x => string.Concat(x.Codigo.ToUpper( ), x.oCategoria.Descripcion.ToUpper( ), x.Descripcion.ToUpper( )).Contains(search.ToUpper( )))
                .Select(m => new Autocomplete
                {
                    label = $"{m.Codigo} - {m.oCategoria.Descripcion} - {m.Descripcion}",
                    value = m.IdProducto
                }
                ).ToList( );
            //retornamos al final un json con una lista de palabras que coincidan con lo que busco el ususario en el campo de buscary lo que busco el ususario se envunetra almacenado en autocomplete
            return Json(autocomplete);
        }

        //metodo de tipo get
        [HttpGet]
        //metodo publico que devuelve un json como resultado al acceder al metodo listar de la
        //clase DA_Producto
        public JsonResult ObtenerProducto( int idproducto )
        {
            //realizamos una instancia opcional de la clase del modelo de Producto
            Producto? oProducto = new Producto( );
            //usamos la clase DA_Producto y entramos al metodo de Listar() y listaremos los productos que pase la validacion
            //de que el idProducto alojado en la BD sea igual al id que pasamos en este metodo como parametro, por
            //lo que oProducto es de tipo object de tipo producto
            oProducto = _daProducto.Listar( ).Where(x => x.IdProducto == idproducto).FirstOrDefault( );
            //retornamos un json con el objeto de tipo producto
            return Json(oProducto);
        }

        //metodo de tipo post
        [HttpPost]
        //metodo  de tipo post que devuelve un json para registrar una venta ademas recibe parametros con formato
        //Json y lee el valor del campo value de los pares key:value, es decir usara los valores del cuerpo del contenido mas no sus nombre
        //por ejemplo si tengo { nombre: alejo }, tomaremos el valor de alejo
        public JsonResult RegistrarVenta( [FromBody] Venta body )
        {
            //creamos una variable de tipo string inicializada como vacia
            string rpta = "";

            //se hace una instancia de la clase XElement para almacenar en la variable "venta"
            //tanto el encabezado de la etiqueta como el valor de la operacion segun sea su caso
            //para el registro de la vente el lo convertira en formato xml de la siguiente manera ejemplo:

            //< Venta >

            //< TipoPago > Efectivo </ TipoPago >

            //< NumeroDocumento > 0 </ NumeroDocumento >

            //< DocumentoCliente > 1234 </ DocumentoCliente >

            //< NombreCliente > alejo prueba14 </ NombreCliente >

            //< MontoPagoCon > 100 </ MontoPagoCon >

            //< MontoCambio > 30 </ MontoCambio >

            //< MontoSubTotal > 59.32 </ MontoSubTotal >

            //< MontoIGV > 10.68 </ MontoIGV >

            //< MontoTotal > 70 </ MontoTotal >

            //</ Venta >

            XElement venta = new XElement("Venta",
                new XElement("TipoPago", body.TipoPago),
                new XElement("NumeroDocumento", "0"),
                new XElement("DocumentoCliente", body.DocumentoCliente),
                new XElement("NombreCliente", body.NombreCliente),
                new XElement("MontoPagoCon", body.MontoPagoCon),
                new XElement("MontoCambio", body.MontoCambio),
                new XElement("MontoSubTotal", body.MontoSubTotal),
                new XElement("MontoIGV", body.MontoIGV),
                new XElement("MontoTotal", body.MontoTotal)
            );

            //se crea una instancia nuevamente de Xelement para almacenar
            //el detalle de la compra pero en formato xml el cual tendra una etiqueta padre llamada Detalle_Venta
            XElement oDetalleVenta = new XElement("Detalle_Venta");
            //despues de esta etiqueta padre construimos otra etiqueta pero sera hija llamada Item
            //y agregaremos el siguiente codigo xml:

            //< Detalle_Venta >
            //< Item >

            //< IdProducto > Efectivo </ IdProducto >

            //< PrecioVenta > 70 </ PrecioVenta >

            //< Cantidad > 1 </ Cantidad >

            //< Total > 70 </ Total >       

            //</ Item >
            //</ Detalle_Venta >


            foreach (Detalle_Venta item in body.oDetalleVenta)
            {
                oDetalleVenta.Add(new XElement("Item",
                    new XElement("IdProducto", item.oProducto.IdProducto),
                    new XElement("PrecioVenta", item.PrecioVenta),
                    new XElement("Cantidad", item.Cantidad),
                    new XElement("Total", item.Total)
                    ));
            }
            //agregamos a la variable venta que ya tiene datos xml por la venta, entonces le agregaremos
            //el detalle de la misma compra y se vera de la siguiente manera:
            //<Venta>

            //< TipoPago > Efectivo </ TipoPago >

            //< NumeroDocumento > 0 </ NumeroDocumento >

            //< DocumentoCliente > 1234 </ DocumentoCliente >

            //< NombreCliente > prueba17 </ NombreCliente >

            //< MontoPagoCon > 70 </ MontoPagoCon >

            //< MontoCambio > 0 </ MontoCambio >

            //< MontoSubTotal > 59.32 </ MontoSubTotal >

            //< MontoIGV > 10.68 </ MontoIGV >

            //< MontoTotal > 70 </ MontoTotal >


            //< Detalle_Venta >


            //< Item >

            //< IdProducto > 6 </ IdProducto >

            //< PrecioVenta > 70 </ PrecioVenta >

            //< Cantidad > 1 </ Cantidad >

            //< Total > 70 </ Total >

            //</ Item >

            //</ Detalle_Venta >

            //</ Venta >
            venta.Add(oDetalleVenta);
            // usamos el modelo para la venta de productos usando sus propiedades
            //y en convertimos a string el archivo xml formado y este archivo convertido lo
            //pasamos como parametro para el metodo registrar venta, y todo esto lo almacenaremos
            //en la variable rpta  
            rpta = _daVenta.Registrar(venta.ToString( ));
            //retornamos un json con el xml convertido en string 
            return Json(new { respuesta = rpta });
        }

        //metodo de tipo get
        [HttpGet]
        //metodo que devuelve un json para obtener las ventas 
        //realizadas y recibe como parametro el numero de documento de la orden o venta 
        public JsonResult ObtenerVenta( string nrodocumento )
        {
            //creamos una nueva instancia  opcional para poder acceder y utilizar los metodos
            //basicos de un crud(ventas), estos se encuentran en la carpeta Datos/DA_Venta.cs
            Venta? oVenta = new Venta( );
            //en oVenta almacenamos el valor que se obtiene(objeto de tipo venta) al usar los metodos que conectan
            //con la DB en la carpeta Datos DA-Venta.cs recibiendo como parametro el numero de documento de la venta
            oVenta = _daVenta.Detalle(nrodocumento);
            //retornamos como un json el objeto almacenado en oVenta
            return Json(oVenta);
        }

    }
}