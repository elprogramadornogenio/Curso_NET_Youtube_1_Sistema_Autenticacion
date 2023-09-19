namespace autenticacionApp.Extensions
{
    public static class EdadExtensions
    {
        public static int CalcularEdad(this DateOnly fechaNacimiento)
        {
            var fechaDeHoy = DateOnly.FromDateTime(DateTime.UtcNow);
            var edad = fechaDeHoy.Year - fechaNacimiento.Year;
            if(fechaNacimiento > fechaDeHoy.AddYears(-edad)) edad--;
            return edad;
        }
    }
}