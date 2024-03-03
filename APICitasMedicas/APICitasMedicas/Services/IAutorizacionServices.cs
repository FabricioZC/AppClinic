using APICitasMedicas.Model.Custom;
using APICitasMedicas.Model;


namespace APICitasMedicas.Services
{
    public interface IAutorizacionServices
    {
        Task<AutorizacionResponse> DevolverToken(Usuario autorizacion);

    }
}
