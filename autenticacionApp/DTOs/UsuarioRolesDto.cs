namespace autenticacionApp.DTOs
{
    public class UsuarioRolesDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}