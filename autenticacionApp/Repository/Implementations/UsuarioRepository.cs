using autenticacionApp.Data.Context;
using autenticacionApp.DTOs;
using autenticacionApp.Helpers;
using autenticacionApp.Helpers.Paginacion;
using autenticacionApp.Models;
using autenticacionApp.Repository.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace autenticacionApp.Repository.Implementations
{
    public class UsuarioRepository: IUsuariosRepository
    {
        private readonly DataContext _contexto;
        private readonly IMapper _mapper;

        public UsuarioRepository(DataContext contexto, IMapper mapper)
        {
            _contexto = contexto;
            _mapper = mapper;
        }

        public async Task<bool> Completado()
        {
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> existeUsuario(string username)
        {
            return await _contexto.Users
            .AnyAsync(usuario => usuario.UserName == username);
        }

        public async Task<IEnumerable<Usuario>> traerUsuarios()
        {
            return await _contexto.Users
                .ToListAsync();
        }

        public async Task<MiembroDto> traerUsuariosId(int id)
        {
            return await _contexto.Users.AsNoTracking()
                .ProjectTo<MiembroDto>(_mapper.ConfigurationProvider)
                //.FindAsync(id);
                .FirstOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<Usuario> traerUsuariosPorUsername(string username)
        {
            return await _contexto.Users
                .Include(usuario => usuario.Fotos)
                .SingleOrDefaultAsync(usuario => usuario.UserName == username);
        }

        public async Task<ListaPaginacion<MiembroDto>> traerMiembrosAsync(ParametrosUsuario parametrosUsuario)
        {
            var consulta = _contexto.Users.AsQueryable();

            consulta = consulta.Where(usuario => usuario.UserName != parametrosUsuario.UsuarioActual);

            var fechaEdadMinimaUsuario = DateOnly.FromDateTime(DateTime.Today.AddYears(-parametrosUsuario.EdadMaxima-1));
            var fechaEdadMaximaUsuario = DateOnly.FromDateTime(DateTime.Today.AddYears(-parametrosUsuario.EdadMinima));

            consulta = consulta.Where(usuario => usuario.FechaDeNacimiento >= fechaEdadMinimaUsuario 
                && usuario.FechaDeNacimiento <= fechaEdadMaximaUsuario );
            
            consulta = parametrosUsuario.OrdenarDatosPor switch
            {
                "fechaCreacion" => consulta.OrderByDescending(usuario => usuario.FechaDeCreacionCuenta),
                _ => consulta.OrderByDescending(usuario => usuario.FechaDeNacimiento)
            };
            /*
            return await consulta.AsNoTracking()
                .ProjectTo<MiembroDto>(_mapper.ConfigurationProvider).ToListAsync();*/
            
            return await ListaPaginacion<MiembroDto>.CrearPaginacionAsync(
                consulta.AsNoTracking().ProjectTo<MiembroDto>(_mapper.ConfigurationProvider),
                parametrosUsuario.NumeroPagina,
                parametrosUsuario.TamanoPagina
            );
        }
    }
}