using API.Common.Paging;
using API.Data;
using API.Dto.Auth;
using API.Entities.Auth;
using API.Helper;
using API.Services.Auth;
using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;

        public UserRepository(DapperContext context,
            IAuthService authService,
             IMapper mapper,
             AppDbContext appDbContext
            )
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        public async Task<int> DeleteUser(int id)
        {
            if (id > 0)
            {
                var user = await GetUserById(id);

                if (user.UserID > 0)
                {
                    //using (var conn = _context.CreateConnection())
                    //{
                    //    var query = "Update Registration set IsDelete = 1 where UserID = @UserID";

                    //    await conn.QueryAsync<int>(query, new
                    //    {
                    //        UserID = user.UserID
                    //    });

                    //    return 1;
                    //}

                    user.IsDelete = true;
                    await _appDbContext.SaveChangesAsync();

                    return 1;
                }
            }

            return -1;
        }

        public async Task<List<string>> GetAllPermissionByUserId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ListUserDto>> GetListUser(UserRequestDto userRequestDto)
        {
            var select = " * ";
            var from = " Registration ";
            var where = " 1=1";
            var oderBy = " UserID";
            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(userRequestDto.KeySearch))
            {
                where += $" AND {userRequestDto.KeySearch} LIKE '%{userRequestDto.ValueSearch}%' ";
            }
            if (!String.IsNullOrEmpty(userRequestDto.SortBy))
            {
                oderBy = $" {userRequestDto.SortBy} ";
            }
            if (userRequestDto.Desc)
            {
                oderBy += " DESC ";
            }
            //parameters.Add("IsDelete", 0);

            using (var conn = new SqlConnection(_context.ConnectionString))
            {
                var data = await conn.QueryPagingAsync<ListUserDto>(select, from, where, oderBy, parameters, userRequestDto.PageNumber, userRequestDto.PageSize);
                return data;
            }
        }

        public async Task<AuthReponseDto> Login(CreateUserDto userDto)
        {
            var data = await _authService.Authenticate(userDto);
            return data;
        }

        public async Task<int> Register(CreateUserDto userDto)
        {
            var data = await _authService.Register(userDto);
            return data;
        }

        public async Task<UserDto> UpdateUser(UserDto registration)
        {
            if (registration.UserID > 0)
            {
                var reg  = await _appDbContext.Registration.FirstOrDefaultAsync(p => p.UserID == registration.UserID);

                if (reg == null)
                {
                    return new UserDto(); ;
                }

                registration.Password = AESEncryption.Encrypt(registration.Password);

                _mapper.Map(registration, reg);
                await _appDbContext.SaveChangesAsync();

                var useDto = await GetUserById(reg.UserID);
                return useDto;
            }
            return new UserDto();
        }

        public async Task<UserDto> GetUserById(int id)
        {
            if (id > 0)
            {
                var user = await _appDbContext.Registration.Where(x => x.UserID == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new UserDto();
                }

                var userDto = _mapper.Map<Registration, UserDto>(user);
                return userDto;
            }

            return new UserDto();
        }
    }
}
