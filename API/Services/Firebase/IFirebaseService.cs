using API.Dto.Firebase;
using System.Threading.Tasks;

namespace API.Services.Firebase
{
    public interface IFirebaseService
    {
        Task<string> UploadFileAsync(FileUploadDto fileUpload);
        Task<int> DeleteFileAsync(string fileName);
    }
}
