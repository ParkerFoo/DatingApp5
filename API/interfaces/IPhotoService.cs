using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);//IFormFile represents a file sent with HttpRequest
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}