using autenticacionApp.DTOs;
using autenticacionApp.Helpers;
using autenticacionApp.Helpers.Paginacion;
using autenticacionApp.Models;
using CloudinaryDotNet.Actions;

namespace autenticacionApp.Repository.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<IEnumerable<Usuario>> traerUsuarios();
        Task<MiembroDto> traerUsuariosId(int id);
        Task<Usuario> traerUsuariosPorUsername(string username);
        Task<ListaPaginacion<MiembroDto>> traerMiembrosAsync(ParametrosUsuario parametrosUsuario);
        Task<bool> existeUsuario(string username);
        Task<bool> Completado();
    }
}