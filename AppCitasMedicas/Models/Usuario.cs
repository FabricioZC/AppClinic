using System.ComponentModel.DataAnnotations;

namespace AppCitasMedicas.Models
{
    public class Usuario
    {
        [Key]  
        public string Email { get; set; }

        [Display(Name = "Nombre Completo")]

        public string NombreCompleto { get; set; }

        public string Telefono { get; set; }

     
        public string Password { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; }

        public char Estado { get; set; }
    }
}
