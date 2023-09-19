using System.IO;
using autenticacionApp.Helpers;
using autenticacionApp.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace autenticacionApp.Services
{
    public class FotoService : IFotoServices
    {
        private readonly Cloudinary _cloudinary;
        public FotoService(IOptions<CloudinarySettings> configuracionCloudinary)
        {
            var _configuracionCloudinary = new Account(
                configuracionCloudinary.Value.CloudName,
                configuracionCloudinary.Value.ApiKey,
                configuracionCloudinary.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(_configuracionCloudinary);
        }
        public async Task<DeletionResult> EliminarFotoCloudinary(string publicId)
        {
            var eliminarFotoCloudinaryParametros = new DeletionParams(publicId);
            return await _cloudinary.DestroyAsync(eliminarFotoCloudinaryParametros);
        }

        public async Task<ImageUploadResult> SubirFotoCloudinary(IFormFile imagen)
        {
            var subirImagenResultado = new ImageUploadResult();
            if(imagen.Length > 0)
            {
                using var stream = imagen.OpenReadStream();
                var subirParametros = new ImageUploadParams
                {
                    File = new FileDescription(imagen.FileName, stream),
                    Transformation = new Transformation().Height(500)
                    .Width(500).Crop("fill").Gravity("face"),
                    Folder = "autenticacion"
                };
                subirImagenResultado = await _cloudinary.UploadAsync(subirParametros);
            }
            return subirImagenResultado;
        }
    }
}