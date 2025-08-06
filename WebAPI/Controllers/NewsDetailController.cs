using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsDetailController : ControllerBase
    {
        INewsDetailService _newsDetailService;

        public NewsDetailController(INewsDetailService newsDetailService)
        {
            _newsDetailService = newsDetailService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _newsDetailService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("GetById")]
        public IActionResult Get(int id)
        {
            var result = _newsDetailService.Get(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


    }
}
