using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsActiveController : ControllerBase
    {
        IFormsActiveService _formsActiveService;

        public FormsActiveController(IFormsActiveService formsActiveService)
        {
            _formsActiveService = formsActiveService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var result = _formsActiveService.GetAll();
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
