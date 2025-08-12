using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _sliderService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }




        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _sliderService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(SliderUpdateDto sliderUpdateDto)
        {
            var result = _sliderService.Update(sliderUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(SliderUpdateDto sliderUpdateDto)
        {
            var result = _sliderService.Add(sliderUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
