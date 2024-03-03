using System.ComponentModel.DataAnnotations;

namespace APICitasMedicas.Model
{
    public class Citas
    {
        [Key]
        public int ID { get; set; }
        public string Email { get; set; }
        public DateTime FechaHora { get; set; }
        public int IDProcedimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal MontoTotal { get; set; }
        public char Estado { get; set; }
    }
}
