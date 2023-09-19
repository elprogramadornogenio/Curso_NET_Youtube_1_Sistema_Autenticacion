using autenticacionApp.Data.Context;
using autenticacionApp.DTOs;
using autenticacionApp.Models;
using autenticacionApp.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace autenticacionApp.Repository.Implementations
{
    public class AdministradorRepository : IAdministradorRepository
    {
        private readonly UserManager<Usuario> _administradorUsuario;

        public AdministradorRepository(UserManager<Usuario> administradorUsuario)
        {
            _administradorUsuario = administradorUsuario;
        }
        public async Task<List<UsuarioRolesDto>> ObtenerUsuarioConRoles()
        {
            return await _administradorUsuario.Users
                .OrderBy(usuario => usuario.UserName)
                .Select(usuario => new UsuarioRolesDto
                {
                    Id = usuario.Id,
                    Username = usuario.UserName,
                    Roles = usuario.UsuarioRoles.Select(roles => roles.Rol.Name).ToList()
                }).ToListAsync();
        }
    }
}