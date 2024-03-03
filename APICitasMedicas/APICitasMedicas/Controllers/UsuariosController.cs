 
using APICitasMedicas.Context;
using APICitasMedicas.Services;
using Microsoft.AspNetCore.Mvc;
using APICitasMedicas.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace APICitasMedicas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : Controller
    {
        
        private readonly DbContextCitas _contexto = null;
       
        private readonly IAutorizacionServices _autorizacionServices;

 
        public UsuariosController(DbContextCitas pContext, IAutorizacionServices autorizacionServices)
        {
             _contexto = pContext;

             _autorizacionServices = autorizacionServices;
        }


        [HttpPost("Registrarse")]
        public string Agregar(Usuario nuevoUsuario)
        {
            try
            {
               
                var usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.Email == nuevoUsuario.Email);

                if (usuarioExistente != null)
                {
                    return "El correo electrónico ya está registrado.";
                }

                _contexto.Usuarios.Add(nuevoUsuario);
                _contexto.SaveChanges();

                return "Usuario registrado correctamente.";
            }
            catch (Exception ex)
            {
              
                return $"Error al registrar usuario: {ex.Message}";
            }
        }


        [HttpPost]
        [Route("AutenticarPW")]
        public async Task<IActionResult> AutenticarPW(string email, string password)
        {
            var temp = await _contexto.Usuarios.FirstOrDefaultAsync(u => (u.Email.Equals(email))
                && (u.Password.Equals(password)));

            if (temp == null)
            {
                return Unauthorized();
            }
            else
            {
                var autorizado = await _autorizacionServices.DevolverToken(temp);

                if (autorizado == null)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(autorizado);
                }
            }
        }

    }
}
