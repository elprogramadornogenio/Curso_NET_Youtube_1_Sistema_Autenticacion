using Microsoft.AspNetCore.Identity;

namespace autenticacionApp.Models
{
    public class Roles: IdentityRole<int>
    {
        public ICollection<UsuarioRoles> UsuarioRoles { get; set; }
    }
}