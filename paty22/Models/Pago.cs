using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string EstadoPago { get; set; } = null!;

    public DateTime FechaPago { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
