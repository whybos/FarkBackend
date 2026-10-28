using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarkindaController : ControllerBase
    {
        private readonly IFarkindaService _farkindaService;

        public FarkindaController(IFarkindaService farkindaService)
        {
            _farkindaService = farkindaService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _farkindaService.GetAll();
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _farkindaService.Get(id);
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        public IActionResult Add(FarkindaUpdateDto farkindaUpdateDto)
        {
            var result = _farkindaService.Add(farkindaUpdateDto);
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(FarkindaUpdateDto farkindaUpdateDto)
        {
            var result = _farkindaService.Update(farkindaUpdateDto);
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        public IActionResult Put(FarkindaUpdateDto farkindaUpdateDto)
        {
            var result = _farkindaService.Update(farkindaUpdateDto);
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _farkindaService.Delete(id);
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteByRoute(int id)
        {
            var result = _farkindaService.Delete(id);
            if (result != null && result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
