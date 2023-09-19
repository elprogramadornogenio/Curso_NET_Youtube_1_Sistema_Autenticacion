using autenticacionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace autenticacionApp.Data.Context
{
    public class DataContext: IdentityDbContext<Usuario, Roles, int,
        IdentityUserClaim<int>, UsuarioRoles, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options): base (options) {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>()
                .HasMany(usuario => usuario.UsuarioRoles)
                .WithOne(usuarioRoles => usuarioRoles.Usuario)
                .HasForeignKey(usuarioRoles => usuarioRoles.UserId)
                .IsRequired();
            
            builder.Entity<Roles>()
                .HasMany(roles => roles.UsuarioRoles)
                .WithOne(usuarioRoles => usuarioRoles.Rol)
                .HasForeignKey(usuarioRoles => usuarioRoles.RoleId)
                .IsRequired();
        }
    }
}