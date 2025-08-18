using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FooterController : ControllerBase
    {
        IFooterService _footerService;

        public FooterController(IFooterService footerService)
        {
            _footerService = footerService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _footerService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _footerService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(FooterUpdateDto footerUpdateDto)
        {
            var result = _footerService.Update(footerUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(FooterUpdateDto footerUpdateDto)
        {
            var result = _footerService.Add(footerUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}