namespace paty22.Models
{
    public class PedidoPagoViewModel
    {
        public Pedido Pedido { get; set; } = new Pedido();
        public List<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>(); // Inicializa aquí
        public decimal Total { get; set; }
        public Pago Pago { get; set; } = new Pago();
        

        // Datos del cliente
        public string? Nombre { get; set; }   // Valor por defecto: cadena vacía
        public string? Telefono { get; set; } 
        public string? Direccion { get; set; } 
    }


}
