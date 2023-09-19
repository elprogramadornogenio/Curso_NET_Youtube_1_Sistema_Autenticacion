using autenticacionApp.DTOs;
using autenticacionApp.Extensions;
using autenticacionApp.Interfaces;
using autenticacionApp.Models;
using autenticacionApp.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace autenticacionApp.Controllers
{
    public class CuentaUsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> _administradorUsuario;
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public CuentaUsuarioController(
            UserManager<Usuario> administradorUsuario,
            IUsuariosRepository usuariosRepository,
            ITokenServices tokenServices,
            IMapper mapper
            )
        {
            _administradorUsuario = administradorUsuario;
            _usuariosRepository = usuariosRepository;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        [HttpPost("registrar-cuenta")]
        public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto datosRegistroDto)
        {
            if(await _usuariosRepository.existeUsuario(datosRegistroDto.Username))
                return BadRequest("El Usuario existe");
            var usuario = _mapper.Map<Usuario>(datosRegistroDto);
            var resultadoCreacionCuentaUsuario = await _administradorUsuario
                .CreateAsync(usuario, datosRegistroDto.Password);
            if(!resultadoCreacionCuentaUsuario.Succeeded) 
                return BadRequest(resultadoCreacionCuentaUsuario.Errors);
            // Agregar Roles esta parte va en la segunda edicion del código
            var resultadoCreacionRolesCuentaUsuario = await _administradorUsuario
                .AddToRoleAsync(usuario, "Miembro");
            if(!resultadoCreacionRolesCuentaUsuario.Succeeded)
                return BadRequest(resultadoCreacionRolesCuentaUsuario.Errors);
            return new UsuarioDto 
            {
                Username = usuario.UserName,
                Token = await _tokenServices.CrearToken(usuario),
                Email = usuario.Email,
                FotoUrl = usuario.traerFotoPrincipal(),
                UsuarioConocidoPor = usuario.UsuarioConocidoPor
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto datosLoginDto)
        {
            var usuario = await _usuariosRepository.traerUsuariosPorUsername(datosLoginDto.Username);
            if(usuario == null) return Unauthorized("No existe usuario");
            var resultadoLoginUsuario = await _administradorUsuario
                .CheckPasswordAsync(usuario, datosLoginDto.Password);
            if(!resultadoLoginUsuario) return Unauthorized("Password Invalido");
            return new UsuarioDto 
            {
                Username = usuario.UserName,
                Token = await _tokenServices.CrearToken(usuario),
                Email = usuario.Email,
                FotoUrl = usuario.traerFotoPrincipal(),
                UsuarioConocidoPor = usuario.UsuarioConocidoPor
            };
        }
    }
}