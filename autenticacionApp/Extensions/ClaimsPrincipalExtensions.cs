using System.Security.Claims;

namespace autenticacionApp.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string traerUsuarioEnSesion(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int traerIdUsuarioEnSesion(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}