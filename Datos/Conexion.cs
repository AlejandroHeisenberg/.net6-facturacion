namespace ProyectoVenta.Datos
{
    public class Conexion
    {
        //creamos una variable estring inicialicada en vacio llamada cadenaSQL
        private string cadenaSQL = string.Empty;
        //metodo para conectarnos a la base de datos 
        public Conexion()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            cadenaSQL = builder.GetSection("ConnectionStrings:CadenaSQL").Value;
        }

        public string getCadenaSQL()
        {
            return cadenaSQL;
        }
    }
}
