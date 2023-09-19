using CloudinaryDotNet.Actions;

namespace autenticacionApp.Interfaces
{
    public interface IFotoServices
    {
        Task<ImageUploadResult> SubirFotoCloudinary(IFormFile imagen);
        Task<DeletionResult> EliminarFotoCloudinary(string publicId);
        
    }
}