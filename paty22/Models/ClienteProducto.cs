using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class ClienteProducto
{
    public int ClienteId { get; set; }

    public int ProductoId { get; set; }

    public int? Cantidad { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
