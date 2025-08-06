using Business.Abstract;
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
    }
}
