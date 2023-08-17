using API.Dto.Auth;
using API.Entities.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Users
{
    public interface IUserRepository
    {
        Task<int> Register(CreateUserDto userDto);
        Task<int> DeleteUser(int id);
        Task<UserDto> UpdateUser(UserDto registration);
        Task<UserDto> GetUserById(int id);
        Task<List<ListUserDto>> GetListUser(UserRequestDto userRequestDto);
        Task<List<string>> GetAllPermissionByUserId(int id);
        Task<AuthReponseDto> Login(CreateUserDto userDto);
        Task<int> TotalRecord(UserRequestDto userRequestDto);
        Task<UserDto> CurrentUser();
        Task<List<string>> GetListUserRoleName();
    }
}
