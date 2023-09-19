using autenticacionApp.Data.Context;
using autenticacionApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace autenticacionApp.Controllers
{
    public class ErroresController : BaseApiController
    {
        private readonly DataContext _context;
        public ErroresController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("no-autorizado")]
        public ActionResult<string> ObtenerNoAutorizado()
        {
            return Unauthorized("No se encuentra autorizado para ver la información");
        }

        [HttpGet("no-encontrado")]
        public ActionResult<Usuario> ObtenerNoEncontrado()
        {
            var consultaUsuario = _context.Users.Find(-1);
            if(consultaUsuario == null) return NotFound();
            return consultaUsuario;
        }

        [HttpGet("error-servidor")]
        public ActionResult<string> ObtenerErrorServidor()
        {
            var consultaUsuario = _context.Users.Find(-1);
            var obtenerRetorno = consultaUsuario.ToString();
            return obtenerRetorno;
        }

        [HttpGet("mal-requerimiento")]
        public ActionResult<Usuario> ObtenerMalRequerimiento()
        {
            return BadRequest("No cumple con los requisitos");
        }

    }
}