namespace autenticacionApp.Errors
{
    public class Excepcion
    {
        public Excepcion(int estatusCodigo, string mensaje, string detalles)
        {
            EstatusCodigo = estatusCodigo;
            Mensaje = mensaje;
            Detalles = detalles;
        }

        public int EstatusCodigo { get; set; }
        public string Mensaje { get; set; }
        public string Detalles { get; set; }
    }
}