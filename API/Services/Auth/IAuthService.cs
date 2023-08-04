using API.Dto.Auth;
using System.Threading.Tasks;

namespace API.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthReponseDto> Authenticate(CreateUserDto loginDto);
        Task<int> Register(CreateUserDto loginDto);
    }
}
