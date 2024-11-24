using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace paty22.Models;

public partial class ProyectoFinalContext : DbContext
{
    public ProyectoFinalContext()
    {
    }

    public ProyectoFinalContext(DbContextOptions<ProyectoFinalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carro> Carros { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ClienteProducto> ClienteProductos { get; set; }

    public virtual DbSet<Etiqueta> Etiquetas { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoProducto> PedidoProductos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { if (optionsBuilder.IsConfigured) { } }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Carro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carro");

            entity.HasIndex(e => e.ClienteId, "FK_Carro_Cliente");

            entity.HasIndex(e => e.ProductoId, "FK_Carro_Producto");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Cantidad)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");
            entity.Property(e => e.ClienteId)
                .HasColumnType("int(11)")
                .HasColumnName("cliente_id");
            entity.Property(e => e.FechaAgregado)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_agregado");
            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("producto_id");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Carros)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carro_Cliente");

            entity.HasOne(d => d.Producto).WithMany(p => p.Carros)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carro_Producto");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.Direccion)
                .HasColumnType("text")
                .HasColumnName("direccion");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<ClienteProducto>(entity =>
        {
            entity.HasKey(e => new { e.ClienteId, e.ProductoId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("cliente_producto");

            entity.HasIndex(e => e.ProductoId, "FK_ClienteProducto_Producto");

            entity.Property(e => e.ClienteId)
                .HasColumnType("int(11)")
                .HasColumnName("cliente_id");
            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("producto_id");
            entity.Property(e => e.Cantidad)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");

            entity.HasOne(d => d.Cliente).WithMany(p => p.ClienteProductos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClienteProducto_Cliente");

            entity.HasOne(d => d.Producto).WithMany(p => p.ClienteProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClienteProducto_Producto");
        });

        modelBuilder.Entity<Etiqueta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("etiquetas");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("inventario");

            entity.HasIndex(e => e.ProductoId, "producto_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CantidadDisponible)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad_disponible");
            entity.Property(e => e.FechaEntrada)
                .HasColumnType("datetime")
                .HasColumnName("fecha_entrada");
            entity.Property(e => e.FechaSalida)
                .HasColumnType("datetime")
                .HasColumnName("fecha_salida");
            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("producto_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("inventario_ibfk_1");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagos");

            entity.HasIndex(e => e.PedidoId, "FK_Pagos_Pedido");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(50)
                .HasColumnName("estado_pago");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha_pago");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.PedidoId)
                .HasColumnType("int(11)")
                .HasColumnName("pedido_id");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pagos_Pedido");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedidos");

            entity.HasIndex(e => e.ClienteId, "FK_Pedidos_Clientes");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ClienteId)
                .HasColumnType("int(11)")
                .HasColumnName("cliente_id");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.EstadoPedido)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pendiente'");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedidos_Clientes");
        });

        modelBuilder.Entity<PedidoProducto>(entity =>
        {
            entity.HasKey(e => new { e.PedidoId, e.ProductoId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("pedido_producto");

            entity.HasIndex(e => e.ProductoId, "fk_producto");

            entity.Property(e => e.PedidoId)
                .HasColumnType("int(11)")
                .HasColumnName("pedido_id");
            entity.Property(e => e.ProductoId)
                .HasColumnType("int(11)")
                .HasColumnName("producto_id");
            entity.Property(e => e.Cantidad)
                .HasColumnType("int(11)")
                .HasColumnName("cantidad");

            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoProductos)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("fk_pedido");

            entity.HasOne(d => d.Producto).WithMany(p => p.PedidoProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_producto");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.EtiquetaId, "FK_Producto_Etiqueta");

            entity.HasIndex(e => e.CategoriaId, "FK_Productos_Categorias");

            entity.HasIndex(e => e.ProveedorId, "FK_Productos_Proveedores");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CategoriaId)
                .HasColumnType("int(11)")
                .HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EtiquetaId)
                .HasColumnType("int(11)")
                .HasColumnName("etiqueta_id");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.ProveedorId)
                .HasColumnType("int(11)")
                .HasColumnName("Proveedor_Id");
            entity.Property(e => e.Stock)
                .HasColumnType("int(11)")
                .HasColumnName("stock");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK_Productos_Categorias");

            entity.HasOne(d => d.Etiqueta).WithMany(p => p.Productos)
                .HasForeignKey(d => d.EtiquetaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Etiqueta");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productos)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Productos_Proveedores");

            entity.HasMany(d => d.Proveedors).WithMany(p => p.ProductosNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductoProveedor",
                    r => r.HasOne<Proveedore>().WithMany()
                        .HasForeignKey("ProveedorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductoProveedor_Proveedor"),
                    l => l.HasOne<Producto>().WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProductoProveedor_Producto"),
                    j =>
                    {
                        j.HasKey("ProductoId", "ProveedorId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("producto_proveedor");
                        j.HasIndex(new[] { "ProveedorId" }, "FK_ProductoProveedor_Proveedor");
                        j.IndexerProperty<int>("ProductoId")
                            .HasColumnType("int(11)")
                            .HasColumnName("producto_id");
                        j.IndexerProperty<int>("ProveedorId")
                            .HasColumnType("int(11)")
                            .HasColumnName("proveedor_id");
                    });
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("proveedores");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
