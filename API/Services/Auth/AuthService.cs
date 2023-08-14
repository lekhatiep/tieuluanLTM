using API.Data;
using API.Dto.Auth;
using API.Entities.Auth;
using API.Helper;
using API.Services.Users;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Auth
{
    public class AuthService : IAuthService
    {
        string SECRET_KEY = "KeyOfMyshop10121994"; // in appsettings.json
        public const string ISSUER = "admin@gmail.com";
        public const string AUDIENCE = "admin@gmail.com";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(
       
            AppDbContext context,
             IMapper mapper,
             IConfiguration configuration
            )
        {

            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthReponseDto> Authenticate(CreateUserDto login)
        {
            string SECRET_KEY = "KeyOfKTM123456789@"; // in appsettings.json
            SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
            login.Password = AESEncryption.Encrypt(login.Password);

            var user = await _context.Registration
                      .Where(x => x.Password.ToLower().Contains(login.Password.ToLower())
                      &&  x.Email.ToLower().Contains(login.Email.ToLower()))
                      .FirstOrDefaultAsync();

            if (user is null)
            {
                return null;
            }

            //Also note that sercurity length should be > 256b
            //Tao chung chi + kieu ma hoa cho chu ky 
            var credentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            //Header of JWT
            var header = new JwtHeader(credentials);

            //Token will be good for 1 minutes + refresh_token

            DateTime Expriry = DateTime.UtcNow.AddMinutes(9000);
            int ts = (int)(Expriry - new DateTime(1970, 1, 1)).TotalSeconds;

            /* Options 2: Using claim */
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("Id", user.UserID.ToString()));

            var permisions = string.Empty;

            var listPermissionName = new List<string>();

            var secToken = new JwtSecurityToken(
                ISSUER,
                AUDIENCE,
                claims,
                expires: DateTime.UtcNow.AddMinutes(9000),
                signingCredentials: credentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);//Sercurity Token

            return new AuthReponseDto
            {
                Token = tokenString,
                TokenType = "Bearer",
                TotalSecond = ts
            };
        }

        public async Task<int> Register(CreateUserDto userDto)
        {
            var existsUser = await IsExistsUser(userDto);

            if (existsUser)
            {
                return -1;
            }

            var newUser = _mapper.Map<Registration>(userDto);

            //var userName = newUser.Email.Substring(newUser.Email.IndexOf('@'), newUser.Email.Length);
            newUser.UserName = newUser.Email;
            newUser.Password = AESEncryption.Encrypt(userDto.Password);
            newUser.CreatedDate = DateTime.Now;
            newUser.IsDelete = false;
            
            try
            {
                await _context.Registration.AddAsync(newUser);
                await _context.SaveChangesAsync();

                //Grant default permission
                await GrantPermissionDefault(newUser.UserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            return 1;
        }

        public async Task<bool> IsExistsUser(CreateUserDto userDto)
        {
            var existsUser = await _context.Registration.Where(x => x.Email == userDto.Email.ToLower()).FirstOrDefaultAsync();

            if (existsUser != null)
            {
                return true;
            }

            return false;
        }

        private async Task GrantPermissionDefault(int userID)
        {
            var regRole = new RegistrationRole() {
                UserID = userID
            };

            var role = await _context.Role.Where(x => x.Name == "User").FirstOrDefaultAsync();

            if (role != null)
            {
                regRole.RoleID = role.RoleID;
                await _context.RegistrationRole.AddAsync(regRole);
                await _context.SaveChangesAsync();
            }

        }
    }
}
