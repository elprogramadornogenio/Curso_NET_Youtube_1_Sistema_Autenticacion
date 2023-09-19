using Microsoft.EntityFrameworkCore;

namespace autenticacionApp.Helpers.Paginacion
{
    public class ListaPaginacion<T> : List<T>
    {
        public ListaPaginacion(
            IEnumerable<T> datos,
            int totalElementos,
            int numeroActualPagina,
            int tamanoPagina
        )
        {
            PaginaActual = numeroActualPagina;
            TotalPaginas = (int) Math.Ceiling(totalElementos/(double) tamanoPagina);
            CantidadElementosPorPagina = tamanoPagina;
            TotalElementos = totalElementos;
            AddRange(datos);
        }

        public int PaginaActual { get; set; }
        public int CantidadElementosPorPagina { get; set; }
        public int TotalElementos { get; set; }
        public int TotalPaginas { get; set; }

        public static async Task<ListaPaginacion<T>> CrearPaginacionAsync(
            IQueryable<T> consulta,
            int numeroPaginaActual,
            int numeroElementosPagina
        )
        {
            var numeroTotalElementos = await consulta.CountAsync();
            var resultadoConsulta = await consulta.Skip((numeroPaginaActual-1)*numeroElementosPagina)
                .Take(numeroElementosPagina).ToListAsync();
            return new ListaPaginacion<T>(resultadoConsulta, numeroTotalElementos, 
                numeroPaginaActual, numeroElementosPagina);
        }
    }
}