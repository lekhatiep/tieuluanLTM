using API.Services.RoboFlow;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoboController : ControllerBase
    {
        private readonly IRoboFlowService _roboFlowService;

        public RoboController(IRoboFlowService roboFlowService)
        {
            _roboFlowService = roboFlowService;
        }

        [HttpGet("DetectImageURL")]
        // GET: UserController/Details/5
        public ActionResult DetectImageURL(string imgPath)
        {
            try
            {
                var rs = _roboFlowService.DetectURLImage(imgPath);
                return Ok(rs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
