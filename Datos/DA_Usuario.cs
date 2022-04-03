using ProyectoVenta.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProyectoVenta.Datos
{
    //clase datos y metodos de ususario de usuario
    public class DA_Usuario
    {
        //metodo listar Ususarios y devuelve una lista de tipo Ususario
        public List<Usuario> Listar()
        {
            //creamos una variable de tipo List de tipo Ususario

            var oLista = new List<Usuario>( );
            //hacemos conexion con la base de datos
            var cn = new Conexion( );
            //vamos a usar la conexion a la base de datos 
            using (var conexion = new SqlConnection(cn.getCadenaSQL( )))
            {
                //abrimos la conexion
                conexion.Open( );
                //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado llamado sp_listar_usuario
                //donde contiene un SELECT de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                SqlCommand cmd = new SqlCommand("sp_listar_usuario", conexion);
                //aqui estamos diciendo que el tipo de comando es de procedimiento almacenado
                cmd.CommandType = CommandType.StoredProcedure;
                //usamos la ejecusion del procedimiento almacenado y guardamos los datos en dr
                using (var dr = cmd.ExecuteReader( ))
                {
                    //mientras haya conexion y se pueda leer los datos...
                    while (dr.Read( ))
                    {
                        //añadimos a nuestra variable oLista de tipo lista 'List<Usuario>' los campos que estan
                        //en la consulta en el procedimineto almacenado select IdUsuario, NombreCompleto, Correo, Clave from USUARIO
                        oLista.Add(new Usuario( )
                        {
                            //convertimos el id en entero, y los demas a string ya que viene de tipo object
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString( ),
                            Correo = dr["Correo"].ToString( ),
                            Clave = dr["Clave"].ToString( )
                        });
                    }
                }
            }
            //devolvemos la lista que acabamos de añadirle datos
            return oLista;
        }

        //metodo boleeano de guardar, retorna un true o false  
        public bool Guardar( Usuario obj )
        {
            //creamos una variable llamada respuesta de tipo bool
            bool respuesta;
            //conectamos a la base de datos
            var cn = new Conexion( );
            //tratamos de guardar ususarios
            try
            {
                //usamos la cadena de conexion a la base de datos creando una nueva instancia
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL( )))
                {
                    //abrimos la conexion
                    oconexion.Open( );
                    //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado
                    //llamado sp_guardar_usuario donde contiene un insert de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                    SqlCommand cmd = new SqlCommand("sp_guardar_usuario", oconexion);
                    //introducimos el valor de los datos en el modelo construido, Usuario, y posteriormente usamos este modelo para insertarlo mediante el 
                    //insert into USUARIO(NombreCompleto, Correo, Clave) values (@NombreCompleto, @Correo, @Clave)
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
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
        //metodo boleeano para editar usuario, devuelve un true o un false 
        public bool Editar( Usuario obj )
        {
            //creamos una variable llamada respuesta de tipo bool
            bool respuesta;
            //conectamos a la base de datos
            var cn = new Conexion( );
            //tratamos de editar un ususario 
            try
            {
                //usamos la cadena de conexion a la base de datos creando una nueva instancia
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL( )))
                {
                    //abrimos la conexion
                    oconexion.Open( );
                    //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado
                    //llamado sp_guardar_usuario donde contiene un update de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                    SqlCommand cmd = new SqlCommand("sp_editar_usuario", oconexion);
                    //introducimos el valor de los datos en el modelo construido, Usuario, y posteriormente usamos este modelo para insertarlo mediante el 
                    //update USUARIO set NombreCompleto = @NombreCompleto, Correo = @Correo,Clave = @Clave where IdUsuario = @IdUsuario
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("NombreCompleto", obj.NombreCompleto);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ejecutamos los cambios asignados y me devuelve un numero de filas afectadas
                    cmd.ExecuteNonQuery( );
                    //asignamos true a La variable respuesta como confirmacion que se (actualizo-edito) correctamente
                    respuesta = true;
                }
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya editar  o no los datos 
            catch (Exception ex)
            {
                respuesta = false;
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya guardado  o no los datos 
            return respuesta;
        }

        //metodo boleeano para eliminar usuario, devuelve un true o un false 
        public bool Eliminar( int idUsuario )
        {
            //creamos una variable llamada respuesta de tipo bool
            bool respuesta;
            //conectamos a la base de datos
            var cn = new Conexion( );
            //tratamos de eliminar un ususario
            try
            {
                //usamos la cadena de conexion a la base de datos creando una nueva instancia
                using (SqlConnection oconexion = new SqlConnection(cn.getCadenaSQL( )))
                {
                    //abrimos la conexion
                    oconexion.Open( );
                    //crea un SqlCommand y luego lo ejecuta pasando una cadena que es una instrucción proceso almacenado
                    //llamado sp_guardar_usuario donde contiene un delete de Transact-SQL y una cadena que se usará para conectarse al origen de datos.
                    SqlCommand cmd = new SqlCommand("sp_eliminar_usuario", oconexion);
                    //introducimos el valor del dato idUsuarios en el modelo construido, Usuario, y posteriormente usamos este modelo para insertarlo en la variable declarada en BD del prcedimiento almacenado
                    //ya despues de asignar este valor a esa variable podremos proceder al eliminado de dicho ususario
                    //delete from USUARIO where IdUsuario = @IdUsuario
                    cmd.Parameters.AddWithValue("IdUsuario", idUsuario);
                    //decimos que el tipo de comando a ejecutar va a ser de procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ejecutamos los cambios asignados y me devuelve un numero de filas afectadas
                    cmd.ExecuteNonQuery( );
                    //asignamos true a La variable respuesta como confirmacion que se elimino correctamente
                    respuesta = true;
                }
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya editar  o no los datos 
            catch (Exception ex)
            {
                respuesta = false;
            }
            //por ultimo retornamos el valor de la respuesta ,true o false, bien sea que haya guardado  o no los datos
            return respuesta;
        }
    }
}
