using Business.Abstract;
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
    }
}
