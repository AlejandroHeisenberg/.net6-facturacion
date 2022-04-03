using ProyectoVenta.Models;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace ProyectoVenta.Datos
{
    public class DA_Venta
    {

        //metodo que retorna un string el cual recibe como paramtro un string con un formato xml
        public string Registrar(string venta_xml)
        {
            //declaramos una variable repuesta en vacio
            string respuesta = "";
            //creamos una instancia a la concexion de la ba se de datos
            var cn = new Conexion();
            //intentaremos registrar una venta
            try
            {
                //usamos la cadena de conexion a la base de datos creando una nueva instancia
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    oconexion.Open();
                    //decimos que el comando ajecutar  mediante la conexion es un procedimiento almacenado, sp_registrar_venta
                    SqlCommand cmd = new SqlCommand("sp_registrar_venta", oconexion);
                    //vamos a agregar a la variable Venta_xml declara en el procedimiento almacenado sp_registrar_venta
                    //en nuestra base de datos el valor del parametro que recibimos en nuestro metodo llamado Venta_xml
                    cmd.Parameters.Add("Venta_xml", SqlDbType.Xml).Value = venta_xml;
                    //Obtiene o establece un valor de tipo varchar que indica si el parámetro NroDocumento de la factura
                    //es solo de entrada, solo de salida,  bidireccional o un parámetro de valor de retorno de procedimiento almacenado. en este
                    //caso es de salida(Output)
                    cmd.Parameters.Add("NroDocumento", SqlDbType.VarChar,6).Direction = ParameterDirection.Output;
                    //ademas decimos que el comando almacenado en la variable cmd es de tipo procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Ejecuta una instrucción Transact-SQL en la conexión y devuelve el número de filas afectadas.
                    cmd.ExecuteNonQuery();
                    //por medio del comando de conexion cmd asignamos el valor convertido a string del parametro NroDocumento de la factura de venta
                    //declarado en la BD a la variable respuesta
                    respuesta = cmd.Parameters["NroDocumento"].Value.ToString();
                }
            }
            //capture el error
            catch (Exception ex)
            {
                //sino se puede registrar la venta entonces pone la variable respuesta en vacio
                respuesta = "";
            }
            //retornamos el valor de respuesta
            return respuesta;
        }

        //metodo para ver detalle que recibe un string que es el numero de documento de la factura
        //y este arroja un objeto de tipo Venta 
        public Venta Detalle(string nrodocumento) {
            //realiamos una instancia opcional del modelo Venta
            Venta? oVenta = new Venta();
            //creamos una instancia a la conexion de la BD
            var cn = new Conexion();
            //tratamos de ver los detalles de una compra
            try
            {
                //usamos la cadena de conexion a la base de datos creando una nueva instancia
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    //abrimos la conexion
                    oconexion.Open();
                    //decimos que el comando ajecutar  mediante la conexion es un procedimiento almacenado, sp_detalle_venta y pasamos la conexion abierta como parametros
                    SqlCommand cmd = new SqlCommand("sp_detalle_venta", oconexion);
                    //agrega el valor del nrodocumento de la factura  que pasamos como parametro a este metodo y lo guardamos a la variable declarada en bd llamada @nrodocumento
                    cmd.Parameters.AddWithValue("nrodocumento", nrodocumento);
                    //else tipo de comando a ejecutar es un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    //leemos el archivo xml y guardamos la info leida en la variable local dr
                    XmlReader dr = cmd.ExecuteXmlReader();
                    //mientras dr se pueda leer
                    if (dr.Read()) {
                        //leemos el documento xml y guardamos en doc
                        XDocument doc = XDocument.Load(dr);

                        oVenta = (doc.Element("Venta") != null) ? (from v in doc.Elements("Venta")
                                                                   select new Venta()
                                                                   {
                                                                       TipoPago = v.Element("TipoPago").Value,
                                                                       NumeroDocumento = v.Element("NumeroDocumento").Value,
                                                                       DocumentoCliente = v.Element("DocumentoCliente").Value,
                                                                       NombreCliente = v.Element("NombreCliente").Value,
                                                                       MontoPagoCon = Convert.ToDecimal(v.Element("MontoPagoCon").Value,new CultureInfo("es-PE")),
                                                                       MontoCambio = Convert.ToDecimal(v.Element("MontoCambio").Value,new CultureInfo("es-PE")),
                                                                       MontoSubTotal = Convert.ToDecimal(v.Element("MontoSubTotal").Value,new CultureInfo("es-PE")),
                                                                       MontoIGV = Convert.ToDecimal(v.Element("MontoIGV").Value,new CultureInfo("es-PE")),
                                                                       MontoTotal = Convert.ToDecimal(v.Element("MontoTotal").Value,new CultureInfo("es-PE")),
                                                                       FechaRegistro = v.Element("FechaRegistro").Value,
                                                                       oDetalleVenta = (v.Element("DetalleVenta") != null) ? (from i in v.Element("DetalleVenta").Elements("Item")
                                                                                                                              select new Detalle_Venta() {
                                                                                                                                  oProducto = new Producto() {
                                                                                                                                      Descripcion = i.Element("Descripcion").Value 
                                                                                                                                  },
                                                                                                                                  Cantidad = Convert.ToInt32(i.Element("Cantidad").Value),
                                                                                                                                  PrecioVenta = Convert.ToDecimal(i.Element("PrecioVenta").Value, new CultureInfo("es-COP")),
                                                                                                                                  Total = Convert.ToDecimal(i.Element("Total").Value, new CultureInfo("es-COP")),
                                                                                                                              }).ToList(): new List<Detalle_Venta> ()

                                                                   }).FirstOrDefault() : new Venta();

                    }


                }
            }
            catch (Exception ex)
            {
            //sino se puede obtener el detalle creamos una nueva instancia de Venta()
                oVenta = new Venta();
            }
            //retornamos un objeto de tipo venta
            return oVenta;
        }


    }
}
