using System.Data.SqlClient;
using System.Data;
using ProyectoVenta.Models;
using System.Globalization;

namespace ProyectoVenta.Datos
{
    public class DA_Producto
    {
        //metodo listar productos y devuelve una lista de tipo producto
        public List<Producto> Listar()
        {
            //creamos una variable de tipo List de tipo producto
            var oLista = new List<Producto>( );
            //creamos un puente o instancia a la conexion a la base de datos
            var cn = new Conexion( );
            //vamos a usar la conexion a la base de datos 
            using (var conexion = new SqlConnection(cn.getCadenaSQL( )))
            {
                //abrimos la conexion
                conexion.Open( );
                //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado llamado sp_listar_producto
                //donde contiene un SELECT de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                SqlCommand cmd = new SqlCommand("sp_listar_producto", conexion);
                //aqui estamos diciendo que el tipo de comando es de procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;
                //usamos la ejecusion del procedimiento almacenado y guardamos los datos en dr
                using (var dr = cmd.ExecuteReader( ))
                {
                    //mientras haya conexion y se pueda leer los datos...
                    while (dr.Read( ))
                    {
                        //añadimos a nuestra variable oLista de tipo lista 'List<Producto>' los campos que estan
                        //en la consulta en el procedimineto almacenado select 
                        //p.IdProducto,
                        //p.Codigo,
                        //c.IdCategoria,
                        //c.Descripcion[DesCategoria],
                        //p.Descripcion,
                        //p.PrecioCompra,
                        //p.PrecioVenta,
                        //p.Stock
                        //from PRODUCTO p

                        //inner join CATEGORIA c on c.IdCategoria = p.IdCategoria

                        //order by p.IdProducto desc
                        //almacenamos en oLista los valores encontrados
                        oLista.Add(new Producto( )
                        {
                            //convertimos el id en entero, y los demas a string ya que viene de tipo object
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Codigo = dr["Codigo"].ToString( ),
                            oCategoria = new Categoria( ) { IdCategoria = Convert.ToInt32(dr["IdCategoria"]), Descripcion = dr["DesCategoria"].ToString( ) },
                            Descripcion = dr["Descripcion"].ToString( ),
                            PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"], new CultureInfo("es-PE")),
                            PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"], new CultureInfo("es-PE")),
                            Stock = Convert.ToInt32(dr["Stock"]),
                        });

                    }
                }
            }
            //retornamos el objeto de tipo List de tipo producto
            return oLista;
        }
        //metodo boleeano de guardar, retorna un true o false  
        public bool Guardar( Producto obj )
        {
            //creamos una variable llamada respuesta de tipo bool
            bool respuesta;
            //conectamos a la base de datos
            var cn = new Conexion( );
            //tratamos de guardar productos
            try
            {
                //usamos la cadena de conexion a la base de datos creando una nueva instancia
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL( )))
                {
                    //abrimos la conexion
                    oconexion.Open( );
                    //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado
                    //llamado sp_guardar_usuario donde contiene un insert de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                    SqlCommand cmd = new SqlCommand("sp_guardar_producto", oconexion);
                    //introducimos el valor de los datos en el modelo construido, Producto, y posteriormente usamos este modelo para insertarlo mediante el 
                    //insert into PRODUCTO(Codigo, IdCategoria, Descripcion, PrecioCompra, PrecioVenta, Stock) values
                    //(@Codigo, @IdCategoria, @Descripcion, @PrecioCompra, @PrecioVenta, @Stock)
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("PrecioCompra", obj.PrecioCompra);
                    cmd.Parameters.AddWithValue("PrecioVenta", obj.PrecioVenta);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //ejecutamos los cambios asignados y me devuelve un numero de filas afectadas
                    cmd.ExecuteNonQuery( );
                    //asignamos true a La variable respuesta como confirmacion que se guardo correctamente
                    respuesta = true;
                }
            }
            //si sale error, capturelo y asigne en respuesta false, como respuesta a que no se guardo correctamente los datos 
            catch (Exception ex)
            {
                respuesta = false;
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya guardado  o no los datos 
            return respuesta;
        }

        //metodo boleeano para editar producto, devuelve un true o un false
        public bool Editar( Producto obj )
        {
            //creamos una variable llamada respuesta de tipo bool
            bool respuesta;
            //conectamos a la base de datos
            var cn = new Conexion( );
            //tratamos de Editar productos
            try
            {

                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL( )))
                {
                    //abrimos la conexion
                    oconexion.Open( );
                    //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado
                    //llamado sp_guardar_usuario donde contiene un insert de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                    SqlCommand cmd = new SqlCommand("sp_editar_producto", oconexion);
                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("PrecioCompra", obj.PrecioCompra);
                    cmd.Parameters.AddWithValue("PrecioVenta", obj.PrecioVenta);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery( );
                    respuesta = true;
                }
            }
            //si sale error, capturelo y asigne en respuesta false, como respuesta a que no se guardo correctamente los datos 
            catch (Exception ex)
            {
                respuesta = false;
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya editado  o no los datos 
            return respuesta;
        }

        public bool Eliminar( int idProducto )
        {
            //creamos una variable llamada respuesta de tipo bool
            bool respuesta;
            var cn = new Conexion( );
            //tratamos de eliminar productos
            try
            {
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL( )))
                {
                    //abrimos la conexion
                    oconexion.Open( );
                    //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado
                    //llamado sp_guardar_usuario donde contiene un insert de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                    SqlCommand cmd = new SqlCommand("sp_eliminar_producto", oconexion);
                    cmd.Parameters.AddWithValue("IdProducto", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery( );
                    respuesta = true;
                }
            }
            //si sale error, capturelo y asigne en respuesta false, como respuesta a que no se guardo correctamente los datos 
            catch (Exception ex)
            {
                respuesta = false;
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya eliminado o no los datos 
            return respuesta;
        }

    }
}
