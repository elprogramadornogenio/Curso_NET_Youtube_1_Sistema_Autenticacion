using Microsoft.AspNetCore.Identity;

namespace autenticacionApp.Models
{
    public class UsuarioRoles: IdentityUserRole<int>
    {
        public Usuario Usuario { get; set; } // 2
        public Roles Rol { get; set; } // 2
    }
}