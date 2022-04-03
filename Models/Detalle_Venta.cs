namespace ProyectoVenta.Models
{
    public class Detalle_Venta
    {
        public int IdDetalleVenta { get; set; }

        //propiedad de tipo Producto 
        public Producto oProducto { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}
