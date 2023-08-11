using API.Dto;
using System.Threading.Tasks;

namespace API.Services.UploadFile
{
    public interface IUpoadFileService
    {
        Task<int> AddInfoUploadFile(UploadFileDto uploadFileDto);
    }
}
