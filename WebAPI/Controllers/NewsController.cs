using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _newsService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

