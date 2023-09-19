using autenticacionApp.Models;
using autenticacionApp.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace autenticacionApp.Controllers
{
    [Authorize(Policy ="RolAdministradorRequerido")]
    public class AdministradorController : BaseApiController
    {
        private readonly IAdministradorRepository _administradorRepository;
        private readonly UserManager<Usuario> _administradorUsuario;

        public AdministradorController(
            IAdministradorRepository administradorRepository,
            UserManager<Usuario> administradorUsuario
            )
        {
            _administradorRepository = administradorRepository;
            _administradorUsuario = administradorUsuario;
        }

        [HttpGet("usuario-con-roles")]
        public async Task<ActionResult> traerRolesDeUsuarios()
        {
            var usuario = await _administradorRepository.ObtenerUsuarioConRoles();
            return Ok(usuario);
        }

        [HttpPost("editar-roles/{username}")]
        public async Task<ActionResult> editarRolesDeUsuario(
            string username,
            [FromQuery] string roles
            )
        {
            if(string.IsNullOrEmpty(roles))
                return BadRequest("Debes seleccionar al menos un rol de usuario");
            
            var seleccionarRoles = roles.Split(",").ToArray();

            var usuario = await _administradorUsuario.FindByNameAsync(username);

            if(usuario == null) return NotFound();

            var rolesUsuario = await _administradorUsuario.GetRolesAsync(usuario);

            var resultadoDeRolesUsuario = await _administradorUsuario
                .AddToRolesAsync(usuario, seleccionarRoles.Except(rolesUsuario));
            
            if(!resultadoDeRolesUsuario.Succeeded) 
                return BadRequest("No se pudo agregar roles");
            
            resultadoDeRolesUsuario = await _administradorUsuario
                .RemoveFromRolesAsync(usuario, seleccionarRoles.Except(seleccionarRoles));
            
            if(!resultadoDeRolesUsuario.Succeeded) 
                return BadRequest("No se pudo eliminar roles");

            return Ok(await _administradorUsuario.GetRolesAsync(usuario));
        }
    }
}