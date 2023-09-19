using autenticacionApp.Models;

namespace autenticacionApp.Interfaces
{
    public interface ITokenServices
    {
        Task<string> CrearToken(Usuario usuario);
    }
}