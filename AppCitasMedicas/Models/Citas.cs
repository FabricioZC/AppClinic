using System.ComponentModel.DataAnnotations;

namespace AppCitasMedicas.Models
{
    public class Citas
    {
        [Key]
        public int ID { get; set; }

        public string Email { get; set; }

        [Display(Name = "Fecha Hora")]
        public DateTime FechaHora { get; set; }

        [Display(Name = "ID Procedimiento")]
        public int IDProcedimiento { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime FechaRegistro { get; set; }

        [Display(Name = "Monto Total")]
        public decimal MontoTotal { get; set; }

        public char Estado { get; set; }
    }
}
