using API.Dto.Firebase;
using System.Threading.Tasks;

namespace API.Services.Firebase
{
    public interface IFirebaseService
    {
        Task<string> UploadFile(FileUploadDto fileUpload);
    }
}
