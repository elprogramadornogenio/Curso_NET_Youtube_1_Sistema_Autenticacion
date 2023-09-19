using System.Text.Json;
using autenticacionApp.Helpers.Paginacion;

namespace autenticacionApp.Extensions
{
    public static class HttpExtensions
    {
        public static void CrearPaginacionEnLaCabeceraPeticion(
            this HttpResponse response,
            CabeceraPaginacion cabecera
        )
        {
            var opcionesJson = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Paginacion", 
                JsonSerializer.Serialize(cabecera, opcionesJson));
            
            response.Headers.Add("Access-Control-Expose-Headers", "Paginacion");
        }
    }
}