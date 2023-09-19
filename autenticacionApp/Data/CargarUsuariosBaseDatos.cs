using System.Text.Json;
using autenticacionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace autenticacionApp.Data
{
    public class CargarUsuariosBaseDatos
    {
        public static async Task CargarUsuarios(
            UserManager<Usuario> _administradorUsuario,
            RoleManager<Roles> _administradorRoles
        )
        {
            if(await _administradorUsuario.Users.AnyAsync()) return;
            var datosUsuario = await File.ReadAllTextAsync("Data/CargarUsuariosBaseDatos.json");
            var opcionesDeLectura = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var usuarios = JsonSerializer.Deserialize<List<Usuario>>(datosUsuario, opcionesDeLectura);

            var roles = new List<Roles>
            {
                new Roles{Name="Miembro"},
                new Roles{Name="Administrador"},
                new Roles{Name="Moderador"}
            };

            foreach (var rol in roles)
            {
                await _administradorRoles.CreateAsync(rol);
            }

            foreach (var usuario in usuarios)
            {
                usuario.UserName = usuario.UserName.ToLower();
                await _administradorUsuario.CreateAsync(usuario, "Pa$$w0rd");
                await _administradorUsuario.AddToRoleAsync(usuario, "Miembro");
            }

            var usuarioAdministrador = new Usuario
            {
                UserName = "admin"
            };

            await _administradorUsuario.CreateAsync(usuarioAdministrador, "Pa$$w0rd");
            await _administradorUsuario.AddToRolesAsync(usuarioAdministrador,
                new [] {"Administrador", "Moderador"});

        }
    }
}