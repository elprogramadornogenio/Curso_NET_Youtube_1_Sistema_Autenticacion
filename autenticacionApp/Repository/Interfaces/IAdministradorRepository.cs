using autenticacionApp.DTOs;

namespace autenticacionApp.Repository.Interfaces
{
    public interface IAdministradorRepository
    {
        Task<List<UsuarioRolesDto>> ObtenerUsuarioConRoles();
    }
}