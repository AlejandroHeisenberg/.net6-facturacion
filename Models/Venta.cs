namespace ProyectoVenta.Models
{
    //este metodo esta definido como el modelo que representa la base de datos para las ventas
    public class Venta
    {
        //propiedades del modelo de ventas
        public int IdVenta { get; set; }
        public string TipoPago { get; set; }
        public string NumeroDocumento { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal MontoPagoCon { get; set; }
        public decimal MontoCambio { get; set; }
        public decimal MontoSubTotal { get; set; }

        public decimal MontoIGV { get; set; }
        public decimal MontoTotal { get; set; }

        public string FechaRegistro { get; set; }

        //propiedad de tipo lista de detalle de la venta
        public List<Detalle_Venta> oDetalleVenta { get; set; }
    }
}
