namespace autenticacionApp.Helpers.Paginacion
{
    public class CabeceraPaginacion
    {
        public CabeceraPaginacion(
            int paginaActual, 
            int cantidadElementosPorPagina,
            int totalElementos,
            int totalPaginas
            )
        {
            PaginaActual = paginaActual;
            CantidadElementosPorPagina = cantidadElementosPorPagina;
            TotalElementos = totalElementos;
            TotalPaginas = totalPaginas;
        }
        public int PaginaActual { get; set; }
        public int CantidadElementosPorPagina { get; set; }
        public int TotalElementos { get; set; }
        public int TotalPaginas { get; set; }
    }
}