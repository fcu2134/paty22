using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Proveedore
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Producto> ProductosNavigation { get; set; } = new List<Producto>();
}
