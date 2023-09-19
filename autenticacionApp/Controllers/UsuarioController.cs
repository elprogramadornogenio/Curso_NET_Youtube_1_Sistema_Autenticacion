using autenticacionApp.DTOs;
using autenticacionApp.Extensions;
using autenticacionApp.Helpers;
using autenticacionApp.Helpers.Paginacion;
using autenticacionApp.Models;
using autenticacionApp.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace autenticacionApp.Controllers
{
    [Authorize]
    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> _administradorUsuario;
        private readonly IUsuariosRepository _usuariosRepository;
        
        public UsuarioController(
            UserManager<Usuario> administradorUsuario,
            IUsuariosRepository usuariosRepository
            )
        {
            _administradorUsuario = administradorUsuario;
            _usuariosRepository = usuariosRepository;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<Usuario>> TraerUsuario(string username)
        {
            return await _usuariosRepository.traerUsuariosPorUsername(username);
        }

        [HttpGet]
        public async Task<ActionResult<MiembroDto>> TraerUsuarioId(int id)
        {
            return await _usuariosRepository.traerUsuariosId(id);
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<Usuario>> Registrar(Usuario usuario)
        {
            var resultadoCrearUsuario = await _administradorUsuario.CreateAsync(usuario);
            if(!resultadoCrearUsuario.Succeeded) return BadRequest(resultadoCrearUsuario.Errors);
            return usuario;
        }

        [HttpGet("traer-usuarios-miembro")]
        public async Task<ActionResult<ListaPaginacion<Usuario>>> TraerUsuarios(
            [FromQuery] ParametrosUsuario parametrosUsuario)
        {
            parametrosUsuario.UsuarioActual = User.traerUsuarioEnSesion();
            var usuarioRegistrados = await _usuariosRepository
                .traerMiembrosAsync(parametrosUsuario);
            
            Response.CrearPaginacionEnLaCabeceraPeticion(new CabeceraPaginacion(
                usuarioRegistrados.PaginaActual,
                usuarioRegistrados.CantidadElementosPorPagina,
                usuarioRegistrados.TotalElementos,
                usuarioRegistrados.TotalPaginas
            ));
            
            return Ok(usuarioRegistrados);
        }
    }
}