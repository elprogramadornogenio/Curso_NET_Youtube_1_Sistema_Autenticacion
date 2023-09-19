using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using autenticacionApp.Interfaces;
using autenticacionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace autenticacionApp.Services
{
    public class TokenService : ITokenServices
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<Usuario> _administradorUsuario; // 2

        public TokenService(
            IConfiguration configuracion,
            UserManager<Usuario> administradorUsuario
            )
        {
            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuracion["TokenKey"]));
            _administradorUsuario = administradorUsuario; // 2
        }
        public async Task<string> CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName)
            };

            // agregar datos de usuario en roles segunda parte
            var roles = await _administradorUsuario.GetRolesAsync(usuario);

            claims.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol)));

            var credenciales = new SigningCredentials(
                _key, 
                SecurityAlgorithms.HmacSha512Signature);

            var descripcionToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descripcionToken);

            return tokenHandler.WriteToken(token);
        }
    }
}