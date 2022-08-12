using Microsoft.AspNetCore.Mvc;

namespace Moodswing.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetStatusOkAsync()
        {
            return Ok();
        }
    }
}
