using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Inventario
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public int CantidadDisponible { get; set; }

    public DateTime FechaEntrada { get; set; }

    public DateTime? FechaSalida { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
