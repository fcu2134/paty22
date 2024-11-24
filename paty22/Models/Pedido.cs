using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public DateTime Fecha { get; set; }

    public string Estado { get; set; } = null!;

    public string EstadoPedido { get; set; } = null!;

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();
}
