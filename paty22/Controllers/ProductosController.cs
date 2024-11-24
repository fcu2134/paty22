using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paty22.Models;
using System.Globalization;
using CsvHelper;
using System.Text;
using System.Linq;

public class ProductosController : Controller
{
    private readonly ProyectoFinalContext _context;

    public ProductosController(ProyectoFinalContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var productos = _context.Productos
          .Include(p => p.Categoria)
          .Include(p => p.Etiqueta)
          .GroupBy(p => p.Id)
          .Select(g => g.FirstOrDefault())
          .Take(20) // Limitar a 20 productos
          .ToList();

        var categorias = _context.Categorias.ToList();
        ViewData["Categorias"] = categorias;
        return View(productos);
    }

    public IActionResult ImportCsv()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            Console.WriteLine($"Archivo recibido: {file.FileName}, tamaño: {file.Length} bytes");

            if (file.ContentType != "text/csv" && !file.FileName.EndsWith(".csv"))
            {
                TempData["ErrorMessage"] = "Por favor, carga un archivo CSV válido.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, MissingFieldFound = null }))
                {
                    var records = csv.GetRecords<ProductoImportCsv>().ToList();
                    var productos = new List<Producto>();
                    var errores = new List<string>();

                    // Cargar las categorías, proveedores y etiquetas en memoria sin rastreo
                    var categorias = _context.Categorias.AsNoTracking().ToList();
                    var proveedores = _context.Proveedores.AsNoTracking().ToList();
                    var etiquetas = _context.Etiquetas.AsNoTracking().ToList();

                    // Buscar el ID del proveedor "Proveedor1"
                    var proveedor1 = proveedores.FirstOrDefault(p => p.Nombre?.ToLower().Trim() == "proveedor1");

                    if (proveedor1 == null)
                    {
                        TempData["ErrorMessage"] = "No se encontró el proveedor 'Proveedor1'.";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var record in records)
                    {
                        // Verificar si el producto ya existe en la base de datos
                        var productoExistente = _context.Productos
                          .FirstOrDefault(p => (p.Nombre != null && record.Nombre != null) &&
                     p.Nombre.ToLower() == record.Nombre.ToLower());


                        if (productoExistente != null)
                        {
                            // Buscar la categoría y la etiqueta
                            var categoria = categorias.FirstOrDefault(c => (c.Nombre?.ToLower().Trim() ?? "") == (record.Categoria?.ToLower().Trim() ?? ""));
                            var etiqueta = etiquetas.FirstOrDefault(e => (e.Nombre?.ToLower().Trim() ?? "") == (record.Etiqueta?.ToLower().Trim() ?? ""));

                            if (categoria != null)
                                productoExistente.CategoriaId = categoria.Id;
                            else
                                errores.Add($"Categoría '{record.Categoria}' no válida para el producto {record.Nombre}.");

                            if (etiqueta != null)
                                productoExistente.EtiquetaId = etiqueta.Id;
                            else
                                errores.Add($"Etiqueta '{record.Etiqueta}' no válida para el producto {record.Nombre}.");

                            // Actualizar el producto
                            productoExistente.Descripcion = record.Descripcion ?? productoExistente.Descripcion;
                            productoExistente.Precio = record.Precio;
                            productoExistente.Stock = record.Stock;
                            productoExistente.ImageUrl = record.ImageUrl ?? productoExistente.ImageUrl;

                            _context.Productos.Update(productoExistente);
                        }
                        else
                        {
                            // Crear un nuevo producto solo si la categoría y la etiqueta son válidas
                            var categoria = categorias.FirstOrDefault(c => (c.Nombre?.ToLower().Trim() ?? "") == (record.Categoria?.ToLower().Trim() ?? ""));
                            var etiqueta = etiquetas.FirstOrDefault(e => (e.Nombre?.ToLower().Trim() ?? "") == (record.Etiqueta?.ToLower().Trim() ?? ""));

                            if (categoria == null || etiqueta == null)
                            {
                                errores.Add($"Producto {record.Nombre} no tiene categoría o etiqueta válidas.");
                                continue;
                            }

                            var producto = new Producto
                            {
                                Nombre = record.Nombre ?? "Nombre desconocido",
                                Descripcion = record.Descripcion,
                                Precio = record.Precio,
                                Stock = record.Stock,
                                CategoriaId = categoria.Id,
                                EtiquetaId = etiqueta.Id,
                                ProveedorId = proveedor1.Id,
                                ImageUrl = record.ImageUrl
                            };

                            productos.Add(producto);
                        }
                    }

                    // Si hay productos válidos, se insertan en la base de datos
                    if (productos.Any())
                    {
                        _context.AddRange(productos);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = $"Se han importado {productos.Count} productos exitosamente.";
                        Console.WriteLine($"Se han importado {productos.Count} productos.");
                    }

                    // Si hubo errores, mostramos un mensaje detallado
                    if (errores.Any())
                    {
                        TempData["ErrorMessage"] = string.Join("<br/>", errores);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hubo un error al procesar el archivo. {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        TempData["ErrorMessage"] = "No se ha seleccionado un archivo válido.";
        return RedirectToAction(nameof(Index));
    }

    public class ProductoImportCsv
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Categoria { get; set; }
        public string? Proveedor { get; set; }
        public string? Etiqueta { get; set; }
        public string? ImageUrl { get; set; }
    }
}
