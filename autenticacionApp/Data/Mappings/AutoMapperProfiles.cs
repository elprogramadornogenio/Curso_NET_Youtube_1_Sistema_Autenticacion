using autenticacionApp.DTOs;
using autenticacionApp.Extensions;
using autenticacionApp.Models;
using AutoMapper;

namespace autenticacionApp.Data.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Usuario, MiembroDto>()
                .ForMember(usuarioDestino => usuarioDestino.UrlFoto,
                    usuarioOrigen => usuarioOrigen
                    .MapFrom(usuarioOrigen => usuarioOrigen
                    .Fotos.FirstOrDefault(foto => foto.EsPrincipal).Url))
                .ForMember(usuarioDestino => usuarioDestino.Edad,
                    usuarioOrigen => usuarioOrigen
                    .MapFrom(usuarioOrigen => usuarioOrigen.FechaDeNacimiento.CalcularEdad()));
            CreateMap<RegistroDto, Usuario>()
                .ForMember(usuarioDestino => usuarioDestino.UserName, 
                    usuarioDtoOrigen => usuarioDtoOrigen
                    .MapFrom(usuarioOrigen => usuarioOrigen.Username.ToLower())
                );
            
            CreateMap<Foto, FotoDto>();
        }
    }
}