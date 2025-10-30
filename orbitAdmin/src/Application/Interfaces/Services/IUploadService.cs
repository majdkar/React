using Microsoft.AspNetCore.Http;
using SchoolV01.Application.Requests;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);

        Task<string> UploadAsync(IFormFile file, string path);

    }
}