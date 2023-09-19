using System.Text;
using autenticacionApp.Data;
using autenticacionApp.Data.Context;
using autenticacionApp.Helpers;
using autenticacionApp.Interfaces;
using autenticacionApp.Middleware;
using autenticacionApp.Models;
using autenticacionApp.Repository.Implementations;
using autenticacionApp.Repository.Interfaces;
using autenticacionApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opcion =>
{
    opcion.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUsuariosRepository, UsuarioRepository>();

builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();

builder.Services.AddScoped<ITokenServices, TokenService>();

builder.Services.AddScoped<IFotoServices, FotoService>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddIdentityCore<Usuario>(opcionesAutenticacion =>
{
    opcionesAutenticacion.Password.RequireUppercase = false;
    opcionesAutenticacion.Password.RequireNonAlphanumeric = false;
})
.AddRoles<Roles>()
.AddRoleManager<RoleManager<Roles>>()
.AddEntityFrameworkStores<DataContext>();


builder.Services.AddAuthorization(opciones => 
{
    opciones.AddPolicy("RolAdministradorRequerido", 
        policy => policy.RequireRole("Administrador"));
    
    opciones.AddPolicy("RolModeradorRequerido",
        policy => policy.RequireRole("Administrador", "Moderador"));
});
/*
builder.Services.Configure<IdentityOptions>(opcionesIdentity => 
{
    opcionesIdentity.Password.RequireUppercase = false;
    opcionesIdentity.Password.RequireNonAlphanumeric = false;
});

*/
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opcionesJwt =>
    {
        opcionesJwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // esto va despues
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });




builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Autenticación API",
        Version = "v1"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese JWT con bearer es decir bearer 'Token'"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            new string[] {}
        }

    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExcepcionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var contexto = services.GetRequiredService<DataContext>();
    var administradorUsuario = services.GetRequiredService<UserManager<Usuario>>();
    var administradorRoles = services.GetRequiredService<RoleManager<Roles>>();
    await contexto.Database.MigrateAsync();
    await CargarUsuariosBaseDatos.CargarUsuarios(administradorUsuario, administradorRoles); 
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "Ocurrió un error al momento de realizar la migración");
}

app.Run();
