using System.ComponentModel.DataAnnotations;

namespace APICitasMedicas.Model
{
    public class Usuario
    {
        [Key]
        public string Email { get; set; }

        public string NombreCompleto { get; set; }

        public string Telefono { get; set; }

        public string Password { get; set; }

        public DateTime FechaRegistro { get; set; }

        public char Estado { get; set; }
    }
}
