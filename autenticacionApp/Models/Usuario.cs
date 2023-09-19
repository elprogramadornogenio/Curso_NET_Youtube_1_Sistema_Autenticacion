using Microsoft.AspNetCore.Identity;

namespace autenticacionApp.Models
{
    public class Usuario : IdentityUser<int>
    {
        public List<Foto> Fotos { get; set; } = new(); // 1
        public string Nombres { get; set; } // 2
        public string Apellidos { get; set; } // 2
        public string UsuarioConocidoPor { get; set; } // 2
        public string InformacionPersonal { get; set; } // 2
        public DateOnly FechaDeNacimiento { get; set; } // 2
        public DateTime FechaDeCreacionCuenta { get; set; } = DateTime.UtcNow; // 2
        public ICollection<UsuarioRoles> UsuarioRoles { get; set; } // 2
    }
}