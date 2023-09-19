namespace autenticacionApp.DTOs
{
    public class MiembroDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string UrlFoto { get; set; }
        public int Edad { get; set; }
        public string UsuarioConocidoPor { get; set; }
        public string InformacionPersonal { get; set; }
        public DateTime FechaDeCreacionCuenta { get; set; }
        public List<FotoDto> Fotos { get; set; } = new(); 
    }
}