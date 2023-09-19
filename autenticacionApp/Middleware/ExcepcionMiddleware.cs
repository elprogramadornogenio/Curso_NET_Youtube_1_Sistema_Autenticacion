using System.Net;
using System.Text.Json;
using autenticacionApp.Errors;

namespace autenticacionApp.Middleware
{
    public class ExcepcionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcepcionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExcepcionMiddleware(
            RequestDelegate next,
            ILogger<ExcepcionMiddleware> logger,
            IHostEnvironment environment
            )
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext contextoHttp)
        {
            try
            {
                await _next(contextoHttp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                contextoHttp.Response.ContentType = "application/json";
                contextoHttp.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var respuesta = _environment.IsDevelopment() 
                ? new Excepcion(
                    contextoHttp.Response.StatusCode,
                    ex.Message,
                    ex.StackTrace.ToString()
                    ): new Excepcion(
                       contextoHttp.Response.StatusCode,
                        ex.Message,
                        "Error Interno del Servidor" 
                    );

                var configuracionRespuestoJson = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var respuestaJson = JsonSerializer
                .Serialize(respuesta, configuracionRespuestoJson);

                await contextoHttp.Response.WriteAsync(respuestaJson);
            }
        }
    }
}