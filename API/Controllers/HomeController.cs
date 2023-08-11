using API.Dto.Firebase;
using API.Helper;
using API.Services.Firebase;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IFirebaseService firebaseService;

        public HomeController(IFirebaseService firebaseService)
        {
            this.firebaseService = firebaseService;
        }
        [HttpGet("Test")]
        // GET: UserController/Details/5
        public async Task<ActionResult> Test()
        {
            try
            {
                var ps = AESEncryption.Encrypt("123");
                return Ok(ps);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<ActionResult> TestUploadFirebase([FromForm] FileUploadDto uploadDto)
        {
            try
            {
                var ps =  await firebaseService.UploadFile(uploadDto);
                return Ok(ps);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
