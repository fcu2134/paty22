using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using paty22.Models;
using BCrypt.Net;
using System;
using System.Threading.Tasks;

namespace patyy.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ProyectoFinalContext _context;

        public ClientesController(ProyectoFinalContext context)
        {
            _context = context;
        }

        // GET: Clientes/Login

        public IActionResult Login()
        {
            return View();
        }

        // POST: Clientes/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string contrasena)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
            {
                ModelState.AddModelError(string.Empty, "Correo y contraseña son obligatorios.");
                return View();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email);

            if (cliente == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View();
            }

            // Verificar si la contraseña ingresada coincide con el hash de la contraseña almacenado
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(contrasena, cliente.Contrasena);

            if (!isPasswordCorrect)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View();
            }

            HttpContext.Session.SetInt32("ClienteId", cliente.Id);  // Guardamos el ClienteId en la sesión
           
            TempData["SuccessMessage"] = "¡Has iniciado sesión con éxito!";

            HttpContext.Session.SetString("NombreCliente", cliente.Nombre ?? "Usuario");

            return RedirectToAction("Index", "Home");
        }



        // GET: Clientes/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpiar la sesión
            return RedirectToAction("Login");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Cliente model)
        {
            if (ModelState.IsValid)
            {
                // Hashear la contraseña con bcrypt
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Contrasena);

                var cliente = new Cliente
                {
                    Nombre = model.Nombre,
                    Telefono = model.Telefono,
                    Email = model.Email,
                    Contrasena = hashedPassword, // Almacena la contraseña hasheada
                };

                // Agregar el cliente a la base de datos
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                // Redirigir al login sin guardar la sesión
                return RedirectToAction("Login");
            }

            return View(model);
        }

    }
}

