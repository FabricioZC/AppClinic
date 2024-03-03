using APICitasMedicas.Model;
using APICitasMedicas.Model.Custom;
using APICitasMedicas.Context;
 
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
 
using Microsoft.EntityFrameworkCore;


namespace APICitasMedicas.Services
{
    public class AutorizacionServices : IAutorizacionServices
    {
         
        private readonly IConfiguration _configuration;
        private readonly DbContextCitas _context;

        public AutorizacionServices(IConfiguration configuration, DbContextCitas context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<AutorizacionResponse> DevolverToken(Usuario autorizacion)
        {
            var temp = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Equals(autorizacion.Email)
            && u.Password.Equals(autorizacion.Password));
         
            if (temp == null)
            {
                 
                return await Task.FromResult<AutorizacionResponse>(null);
            }
           
            string tokenCreado = GenerarToken(autorizacion.Email.ToString());

            return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Msj = "Ok" };
        }

        private string GenerarToken(string IdUsuario)
        {
            
            var key = _configuration.GetValue<string>("JwtSettings:Key");

        
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IdUsuario));

           
            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),   
                SecurityAlgorithms.HmacSha256Signature);

           
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),  
                SigningCredentials = credencialesToken  
            };

            var tokenHandler = new JwtSecurityTokenHandler();  
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);  
 
            var tokenCreado = tokenHandler.WriteToken(tokenConfig);
             
            return tokenCreado;

        }

    }
}
