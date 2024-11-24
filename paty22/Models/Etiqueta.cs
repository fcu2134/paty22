using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Etiqueta
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
