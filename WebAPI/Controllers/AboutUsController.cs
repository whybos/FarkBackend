using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutUsController : ControllerBase
    {



        IAboutUsService _aboutUsService;

        public AboutUsController(IAboutUsService AboutUsService)
        {
            _aboutUsService = AboutUsService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _aboutUsService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _aboutUsService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(AboutUsUpdateDto aboutUsUpdateDto)
        {
            var result = _aboutUsService.Update(aboutUsUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(AboutUsUpdateDto aboutUsUpdateDto)
        {
            var result = _aboutUsService.Add(aboutUsUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
