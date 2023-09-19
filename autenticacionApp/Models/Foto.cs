namespace autenticacionApp.Models
{
    public class Foto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool EsPrincipal { get; set; }
        public string PublicId { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}