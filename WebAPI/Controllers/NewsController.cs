using Business.Abstract;
using Entities.DTOs;
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
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _newsService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(NewsUpdateDto newsUpdateDto)
        {
            var result = _newsService.Update(newsUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(NewsUpdateDto newsUpdateDto)
        {
            var result = _newsService.Add(newsUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}