using API.Helper;
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
    }
}
