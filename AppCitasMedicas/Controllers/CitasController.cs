using AppCitasMedicas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;

namespace AppCitasMedicas.Controllers
{
    [Authorize]
    public class CitasController : Controller
    {
        private CitasAPI citasAPI;

        private HttpClient client;

 
        public CitasController()
        {
           
            citasAPI = new CitasAPI();

            client = citasAPI.Iniciar();

        }
 
        public async Task<IActionResult> Index()
        {
       
            List<Citas> listado = new List<Citas>();

  
            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            HttpResponseMessage response = await client.GetAsync("/Citas/ListaCitas");


            if (ValidarTransaccion(response.StatusCode) == false)
            {

                return RedirectToAction("Logout", "Usuarios");
            }

     
            if (response.IsSuccessStatusCode)
            {
             
                var resultados = response.Content.ReadAsStringAsync().Result;

 
                listado = JsonConvert.DeserializeObject<List<Citas>>(resultados);
            }
 
            return View(listado);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind] Citas pCita)
        {
            pCita.ID = 0;
            pCita.FechaRegistro = DateTime.Now;
            pCita.Estado = 'A';

            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var agregar = client.PostAsJsonAsync<Citas>("/Citas/CrearCita", pCita);

            await agregar;  

            var resultado = agregar.Result;

            //Se vence el token
            if (resultado.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Logout", "Usuarios");
            }

            if (resultado.IsSuccessStatusCode)  
            {
                return RedirectToAction("Index");  
            }
            else  
            {
                TempData["Mensaje"] = "No se logró registrar la cita.."; 
                return View(pCita);  
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int ID)
        {
            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var cita = new Citas();

            HttpResponseMessage mensaje = await client.GetAsync($"/Citas/BuscarCita?idCita={ID}");


            if (mensaje.IsSuccessStatusCode)
            {

                var resultado = mensaje.Content.ReadAsStringAsync().Result;


                cita = JsonConvert.DeserializeObject<Citas>(resultado);
            }


            return View(cita);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind] Citas pCita)
        {

            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            var modificar = client.PutAsJsonAsync<Citas>("/Citas/EditarCita", pCita);
            
            await modificar;  

            var resultado = modificar.Result;
          
            if (resultado.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Citas");   
            }
            else   
            {
                TempData["Mensaje"] = "Error al ingresar los datos";
                return View(pCita);  
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int ID)
        {
 
            var cita = new Citas();

            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            HttpResponseMessage mensaje = await client.GetAsync($"/Citas/BuscarCita?idCita={ID}");


            if (mensaje.IsSuccessStatusCode) 
            {
               
                var resultado = mensaje.Content.ReadAsStringAsync().Result;

            
                cita = JsonConvert.DeserializeObject<Citas>(resultado);
            } 
            return View(cita);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteCita(int ID)
        {

            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            HttpResponseMessage response = await client.DeleteAsync($"/Citas/EliminarCita?idCita={ID}");

            return RedirectToAction("Index", "Citas");
        }


        [HttpGet]
        public async Task<IActionResult> Details(int ID)
        {
            var cita = new Citas();

            client.DefaultRequestHeaders.Authorization = AutorizacionToken();

            HttpResponseMessage respuesta = await client.GetAsync($"/Citas/BuscarCita?idCita={ID}");

            if (respuesta.IsSuccessStatusCode)
            {
                var resultado = respuesta.Content.ReadAsStringAsync().Result;
                cita = JsonConvert.DeserializeObject<Citas>(resultado);
            }

            return View(cita);
        }


        private AuthenticationHeaderValue AutorizacionToken()
        {
            var token = HttpContext.Session.GetString("token");

            AuthenticationHeaderValue autorizacion = null;

      
            if (token != null && token.Length != 0)
            {
               
                autorizacion = new AuthenticationHeaderValue("Bearer", token);
            }

            return autorizacion;
        }

        private bool ValidarTransaccion(HttpStatusCode resultado)
        {
       
            if (resultado == HttpStatusCode.Unauthorized)
            {
                TempData["MensajeSesion"] = "Su sesión fue válida o expiro..";
                return false;
            }
            else
            {
                TempData["MensajeSesion"] = null;
                return true;
            }
        }


    }
}
