namespace AppCitasMedicas.Models
{
    public class CitasAPI
    {
        public HttpClient Iniciar() 
        { 
            var client = new HttpClient();

             //client.BaseAddress = new Uri("https://localhost:7163/");

            client.BaseAddress = new Uri("https://www.APICitasDrZamoraC08694.somee.com");

            return client;


        }
    }
}
