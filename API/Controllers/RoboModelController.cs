using API.Common.ReponseDto;
using API.CustomAttribute;
using API.Dto.Catalog;
using API.Enums;
using API.Services.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoboModelController : ControllerBase
    {
        private readonly IModelRepository _modelRepository;
        public RoboModelController(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }
        // GET: UserController


        [HttpGet("Details/{id}")]
        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var model = await _modelRepository.GetModelById(id);
                return Ok(model);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // POST: UserController/Edit/5
        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromForm] UpdateModelDto updateModelDto)
        {
            try
            {
                var modelExist = await _modelRepository.GetModelById(updateModelDto.ModelID);
                if (modelExist.ModelID > 0)
                {
                    var res = await _modelRepository.UpdateModel(updateModelDto);

                    return Ok();
                }

                return BadRequest();
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
                var res = await _modelRepository.DeleteModel(id);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [CustomAuthorize(EnumsList.Role.Admin)]
        [HttpGet("GetListModel")]
        public async Task<ActionResult> GetListModel([FromQuery] ModelRequestDto request)
        {
            try
            {
                var listModel = await _modelRepository.GetListModel(request);
                var totalRecord = await _modelRepository.TotalRecord(request);

                var rs = new ResponsePagedList<ListModelDto>(listModel, totalRecord);
                return Ok(rs);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetListModelByUser")]
        public async Task<ActionResult> GetListModelByUser([FromQuery] ModelRequestDto request)
        {
            try
            {
                var listModel = await _modelRepository.GetListModelByUser(request);
                var totalRecord = await _modelRepository.TotalRecord(request, isCountForUser: true);

                var rs = new ResponsePagedList<ListModelDto>(listModel, totalRecord);

                return Ok(rs);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost("AddModel")]
        public async Task<ActionResult> AddModel([FromForm] CreateModelDto request)
        {
            try
            {
                var data = await _modelRepository.AddNewModel(request);

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

    }
}
