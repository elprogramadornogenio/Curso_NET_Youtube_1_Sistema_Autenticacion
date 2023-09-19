using autenticacionApp.Models;

namespace autenticacionApp.Extensions
{
    public static class UsuarioExtensions
    {
        public static string traerFotoPrincipal(this Usuario usuario)
        {
            var urlFoto = string.Empty;
            if(usuario.Fotos.Count == 0) 
                urlFoto = "https://res.cloudinary.com/dkdwgznvg/image/upload/v1666217579/no-photo-id-auth-server.png";
            else
                urlFoto = usuario.Fotos.FirstOrDefault(foto => foto.EsPrincipal).Url;
            return urlFoto;
        }
    }
}