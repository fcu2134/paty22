using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Cupone
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public decimal Descuento { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public bool? Activo { get; set; }
}
