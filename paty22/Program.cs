using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using paty22.Models;
using System.IO;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de la base de datos MySQL
builder.Services.AddDbContext<ProyectoFinalContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Connection"),
        ServerVersion.Parse("10.4.25-mariadb") // Ajusta la versión si es necesario
    ));

// Configuración de sesiones
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session"; 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
});


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "es-ES", "en-US", "es-CL" };
    options.SetDefaultCulture("es-CL")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

var app = builder.Build();



    app.UseDeveloperExceptionPage(); 


    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

Console.WriteLine($"Entorno actual: {builder.Environment.EnvironmentName}");

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();  
app.UseSession();   
app.UseAuthorization();  
builder.Host.UseEnvironment("Development");

// Mapeo de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
