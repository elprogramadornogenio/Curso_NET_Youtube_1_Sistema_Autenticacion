namespace autenticacionApp.DTOs
{
    public class UsuarioDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string UsuarioConocidoPor { get; set; } // 2
        public string FotoUrl { get; set; } // 2
    }
}