using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int? CategoriaId { get; set; }

    public int EtiquetaId { get; set; }

    public int ProveedorId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Carro> Carros { get; set; } = new List<Carro>();

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<ClienteProducto> ClienteProductos { get; set; } = new List<ClienteProducto>();

    public virtual Etiqueta Etiqueta { get; set; } = null!;

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<PedidoProducto> PedidoProductos { get; set; } = new List<PedidoProducto>();

    public virtual Proveedore Proveedor { get; set; } = null!;

    public virtual ICollection<Proveedore> Proveedors { get; set; } = new List<Proveedore>();
}
