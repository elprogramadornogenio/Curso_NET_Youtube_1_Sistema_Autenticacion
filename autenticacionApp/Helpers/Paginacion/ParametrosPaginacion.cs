namespace autenticacionApp.Helpers.Paginacion
{
    public class ParametrosPaginacion
    {
        private const int CantidadMaximaValoresPorPagina = 50;
        public int NumeroPagina { get; set; } = 1;
        private int _tamanoPagina = 10;
        public int TamanoPagina 
        { 
            get => _tamanoPagina; 
            set => _tamanoPagina = (value > CantidadMaximaValoresPorPagina)? 
                CantidadMaximaValoresPorPagina: value; 
        }
    }
}