using API.Common.ReponseDto;
using API.CustomAttribute;
using API.Dto.Auth;
using API.Entities.Auth;
using API.Enums;
using API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: UserController

       
        [HttpGet("Details/{id}")]
        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                return Ok(user);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // POST: UserController/Edit/5
        [HttpPut("Update")]
        [CustomAuthorize(EnumsList.Role.User)]
        public async Task<ActionResult> Update([FromBody] UserDto registration)
        {
            try
            {
                var res = await _userRepository.UpdateUser(registration);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        // POST: UserController/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var res = await _userRepository.DeleteUser(id);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


        [CustomAuthorize(EnumsList.Role.Admin)]
        [HttpGet("GetListUser")]
        public async Task<ActionResult> GetListUser([FromQuery] UserRequestDto request)
        {
            try
            {
                var listUser = await _userRepository.GetListUser(request);
                return Ok(listUser);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] CreateUserDto request)
        {
            try
            {
                var data = await _userRepository.Register(request);

                if (data == -1)
                {
                    return BadRequest();
                }

                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var useDto = new CreateUserDto()
                {
                    Email = loginDto.Email,
                    Password = loginDto.Password,
                };

                var data = await _userRepository.Login(useDto);
                return Ok(data);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
