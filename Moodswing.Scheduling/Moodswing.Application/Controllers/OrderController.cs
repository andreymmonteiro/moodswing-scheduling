using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Moodswing.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetStatusOkAsync([FromQuery] string name)
        {
            var devs = new List<Dictionary<string, string>>()
            {
              new Dictionary<string, string>()
                {
                    { "name", "Andrey Monteiro" },
                    { "cel", "Boston" }
                },
              new Dictionary<string, string>()
                {
                    { "name", "Gideval Santos" },
                    { "cel", "Boston" }
                },
              new Dictionary<string, string>()
                {
                    { "name", "Matheus Hoffman" },
                    { "cel", "Boston" }
                }
            };
            return Ok(
                !string.IsNullOrWhiteSpace(name) ? 
                    devs.Where(dev => dev.Any(item => item.Value.Contains(name))) :
                    devs);
        }
    }
}
