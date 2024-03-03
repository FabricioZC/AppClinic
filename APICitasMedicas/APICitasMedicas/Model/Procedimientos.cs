using System.ComponentModel.DataAnnotations;

namespace APICitasMedicas.Model
{
    public class Procedimientos
    {
        [Key]
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioActual { get; set; }
        public char PagoDolares { get; set; }
        public decimal PorImp { get; set; }
    }
}
