using APICitasMedicas.Context;
using APICitasMedicas.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICitasMedicas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitasController : Controller
    {
        public readonly DbContextCitas _contexto;

        private TipoCambioAPI tipoCambioAPI;

        private HttpClient clientTipoCambio;

        public static TipoCambio tipoCambio;


        public CitasController(DbContextCitas pContext)
        {
            _contexto = pContext;

            tipoCambioAPI = new TipoCambioAPI();
 
            clientTipoCambio = tipoCambioAPI.Inicial();

            extraerTipoCambio();
        }
        
        private async void extraerTipoCambio()
        {
            try
            {
                HttpResponseMessage response = await clientTipoCambio.GetAsync("tdc/tdc.json");

                if (response.IsSuccessStatusCode)
                {

                    var result = response.Content.ReadAsStringAsync().Result;


                    tipoCambio = JsonConvert.DeserializeObject<TipoCambio>(result);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

 
        [HttpGet]
        [Route("ListaCitas")]
        public async Task<IActionResult> ListaCitas()
        {
            try
            {
                var citas = await _contexto.Citas.ToListAsync();

                return Ok(citas);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener la lista de citas: {ex.Message}");
            }
        }
 
        [HttpPost("CrearCita")]
        public async Task<IActionResult> AgregarCita(Citas pCita)
        {
            try
            {
                // Validar si ya existe una cita
                var citaExistente = await _contexto.Citas
                    .FirstOrDefaultAsync(x => x.FechaHora == pCita.FechaHora);

                if (citaExistente != null)
                {
                    return BadRequest("Ya existe una cita registrada para la fecha y hora indicada.");
                }

                // IdProcedimiento asociado a cita
                var procedimiento = await _contexto.Procedimientos
                    .FindAsync(pCita.IDProcedimiento);

                if (procedimiento == null)
                {
                    return NotFound("No se encontró el procedimiento con el ID especificado.");
                }

                // Calcular montos
                decimal montoColones = 0;
                decimal montoTotal = 0;
                decimal montoImpuesto = 0;

                switch (procedimiento.ID)
                {
                    case 44:
                        montoColones = procedimiento.PrecioActual * tipoCambio.venta;
                        montoImpuesto = montoColones * 0.13m;
                        montoTotal = montoColones + montoImpuesto;
                        break;
                    case 48:
                        montoColones = procedimiento.PrecioActual;
                        montoImpuesto = procedimiento.PrecioActual * 0.13m;
                        montoTotal = montoColones + montoImpuesto;
                        break;
                    case 53:
                        montoColones = procedimiento.PrecioActual * tipoCambio.venta;
                        montoImpuesto = montoColones * 0.13m;
                        montoTotal = montoColones + montoImpuesto;
                        break;
                    case 71:
                        montoColones = procedimiento.PrecioActual * tipoCambio.venta;
                        montoImpuesto = montoColones * 0.13m;
                        montoTotal = montoColones + montoImpuesto;
                        break;
                    default:
                        return BadRequest("El ID del procedimiento no tiene un precio asociado.");
                }

              
                pCita.MontoTotal = montoTotal;
                 
                _contexto.Citas.Add(pCita);
                await _contexto.SaveChangesAsync();

               
                return Ok("La cita se ha creado correctamente.");
            }
            catch (Exception)
            {
    
                return StatusCode(500, "Error al crear la cita.");
            }
        }

      
        [HttpDelete("EliminarCita")]
        public async Task<IActionResult> EliminarCita(int idCita)
        {
            try
            {
             
                var cita = await _contexto.Citas.FindAsync(idCita);
   
                if (cita == null)
                {
                    return NotFound("No se encontró la cita con ese ID.");
                }
 
                _contexto.Citas.Remove(cita);

                await _contexto.SaveChangesAsync();

                return Ok("La cita ha sido eliminada correctamente...");
            }
            catch (Exception)
            {
               
                return StatusCode(500, "Error interno al eliminar la cita.");
            }
        }

   
        [HttpGet("BuscarCita")]
        public async Task<IActionResult> BuscarCita(int idCita)
        {
            try
            {
                var cita = await _contexto.Citas.FindAsync(idCita);
        
                if (cita == null)
                {
                    return NotFound("No se encontró ninguna cita con ese ID.");
                }

                return Ok(cita);
            }
            catch (Exception)
            {
    
                return StatusCode(500, "Error interno al buscar la cita.");
            }
        }

        [HttpPut("EditarCita")]
        public string ModificarCita(Citas pCita)
        {
            string mensaje = "No se logró aplicar los cambios a la cita..";

            try
            {
                _contexto.Citas.Update(pCita);

                _contexto.SaveChanges();

                mensaje = "Cambios aplicados correctamente a la cita...";
            }
            catch (Exception ex)
            {
                mensaje = "Error => " + ex.Message;
            }
            return mensaje;
        }

    }
}
