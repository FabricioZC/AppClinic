using AppCitasMedicas.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net;

namespace AppCitasMedicas.Controllers
{
    public class UsuariosController : Controller
    {
        private CitasAPI citasAPI;

        private HttpClient httpClient;

 
        public UsuariosController()
        {
            citasAPI = new CitasAPI();

            httpClient = citasAPI.Iniciar();

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind] Usuario usuario)
        {
            // Valores predeterminados
            usuario.FechaRegistro = DateTime.Now;
            usuario.Estado = 'A';

            var agregar = await httpClient.PostAsJsonAsync("/Usuarios/Registrarse", usuario);

            if (agregar.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Usuarios");
            }
            else
            {
                var errorMessage = await agregar.Content.ReadAsStringAsync();
                TempData["MensajeRegister"] = "Error al realizar el registro";

                return View(usuario);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind] Usuario usuario)
        {
            AutorizacionResponse autorizacion = null;

            HttpResponseMessage response = await httpClient.PostAsync(
                $"/Usuarios/AutenticarPW?email={usuario.Email}&password={usuario.Password}", null);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                autorizacion = JsonConvert.DeserializeObject<AutorizacionResponse>(resultado);
            }

            if (autorizacion != null)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Email));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("token", autorizacion.Token);

                return RedirectToAction("Index", "Citas");
            }
            else
            {
                
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["MensajeLogin"] = "Credenciales incorrectas. Por favor, verifique su correo electrónico y contraseña.";
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["MensajeLogin"] = "Usuario no encontrado. Por favor, registre una cuenta antes de intentar iniciar sesión.";
                }
                else
                {
                    TempData["MensajeLogin"] = "Error al intentar iniciar sesión. Por favor, inténtelo nuevamente más tarde.";
                }

                return View(usuario);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();  
            HttpContext.Session.SetString("token", "");  

            return RedirectToAction("Login", "Usuarios");  
        }

    }
}
