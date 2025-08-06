using Business.Abstract;
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
    }
}
