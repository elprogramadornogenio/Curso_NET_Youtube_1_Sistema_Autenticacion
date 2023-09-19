using autenticacionApp.DTOs;
using autenticacionApp.Extensions;
using autenticacionApp.Interfaces;
using autenticacionApp.Models;
using autenticacionApp.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace autenticacionApp.Controllers
{
    [Authorize]
    public class FotosController : BaseApiController
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IFotoServices _fotoServices;
        private readonly IMapper _mapper;
        public FotosController(
            IUsuariosRepository usuariosRepository,
            IFotoServices fotoServices,
            IMapper mapper
            )
        {
            _usuariosRepository = usuariosRepository;
            _fotoServices = fotoServices;
            _mapper = mapper;
        }
        
        [HttpPost("agregar-foto")]
        public async Task<ActionResult<FotoDto>> AgregarFoto(IFormFile foto)
        {
            var usuario = await _usuariosRepository
                .traerUsuariosPorUsername(User.traerUsuarioEnSesion());
            
            if(usuario == null) return NotFound();

            var resultadosFoto = await _fotoServices.SubirFotoCloudinary(foto);

            if(resultadosFoto.Error != null) return BadRequest(resultadosFoto.Error.Message);

            var descripcionFoto = new Foto
            {
                Url = resultadosFoto.SecureUrl.AbsoluteUri,
                PublicId  = resultadosFoto.PublicId
            };

            if(usuario.Fotos.Count == 0) descripcionFoto.EsPrincipal = true;

            usuario.Fotos.Add(descripcionFoto);

            if( await _usuariosRepository.Completado())
                return CreatedAtAction("TraerUsuario", new {controller = "usuario", username = usuario.UserName}, 
                    _mapper.Map<FotoDto>(descripcionFoto));
            return BadRequest($"No se pudo agregar la foto al usuario {usuario.UserName}");

        }

        [HttpPut("cambiar-foto-principal/{fotoId}")]
        public async Task<ActionResult> CambiarFotoPrincipal(int fotoId)
        {
            var usuario = await _usuariosRepository
                .traerUsuariosPorUsername(User.traerUsuarioEnSesion());
            if(usuario == null) return NotFound();
            var foto = usuario.Fotos.FirstOrDefault(foto => foto.Id == fotoId);
            if(foto == null) return NotFound();
            if(foto.EsPrincipal) return BadRequest("Esta es la foto principal");
            var fotoPrincipalActual = usuario.Fotos.FirstOrDefault(foto => foto.EsPrincipal);
            if(fotoPrincipalActual!= null) fotoPrincipalActual.EsPrincipal = false;
            foto.EsPrincipal = true;
            if(await _usuariosRepository.Completado()) return Ok();
            return BadRequest("No se ha podido actualizar la foto");
        }

        [HttpDelete("eliminar-foto/{fotoId}")]
        public async Task<ActionResult> EliminarFoto(int fotoId)
        {
            var usuario = await _usuariosRepository
                .traerUsuariosPorUsername(User.traerUsuarioEnSesion());
            
            if(usuario == null) return NotFound();
            
            var foto = usuario.Fotos.FirstOrDefault(foto => foto.Id == fotoId);

            if(foto == null) return NotFound();

            if(foto.EsPrincipal) return BadRequest("La foto principal no puede ser eliminada");

            if(foto.PublicId != null)
            {
                var resultadoImagenEliminada = await _fotoServices.EliminarFotoCloudinary(foto.PublicId);
                if(resultadoImagenEliminada.Error != null) 
                    return BadRequest(resultadoImagenEliminada.Error.Message);
            }

            usuario.Fotos.Remove(foto);

            if(await _usuariosRepository.Completado()) return Ok();
            return BadRequest("No se ha podido eliminar la foto");
        }
    }
}