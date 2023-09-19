using autenticacionApp.Helpers.Paginacion;

namespace autenticacionApp.Helpers
{
    public class ParametrosUsuario: ParametrosPaginacion
    {
        public string UsuarioActual { get; set; }
        public int EdadMinima { get; set; } = 21;
        public int EdadMaxima { get; set; } = 100;
        public string OrdenarDatosPor { get; set; } = "fechaCreacion";
    }
}