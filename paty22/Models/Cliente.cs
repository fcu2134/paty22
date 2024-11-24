using System;
using System.Collections.Generic;

namespace paty22.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Carro> Carros { get; set; } = new List<Carro>();

    public virtual ICollection<ClienteProducto> ClienteProductos { get; set; } = new List<ClienteProducto>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
