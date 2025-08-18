using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsActiveController : ControllerBase
    {
        IFormsActiveService _formsActiveService;

        public FormsActiveController(IFormsActiveService formsActiveService)
        {
            _formsActiveService = formsActiveService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _formsActiveService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _formsActiveService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(FormsActiveUpdateDto formsActiveUpdateDto)
        {
            var result = _formsActiveService.Update(formsActiveUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(FormsActiveUpdateDto formsActiveUpdateDto)
        {
            var result = _formsActiveService.Add(formsActiveUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}