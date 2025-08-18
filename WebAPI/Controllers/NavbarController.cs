using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavbarController : ControllerBase
    {
        INavbarService _navbarService;

        public NavbarController(INavbarService navbarService)
        {
            _navbarService = navbarService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _navbarService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _navbarService.Delete(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch]
        public IActionResult Update(NavbarUpdateDto navbarUpdateDto)
        {
            var result = _navbarService.Update(navbarUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost]
        public IActionResult Add(NavbarUpdateDto navbarUpdateDto)
        {
            var result = _navbarService.Add(navbarUpdateDto);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}