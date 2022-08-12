using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Moodswing.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetStatusOkAsync()
        {
            return Ok(new List<Dictionary<string, string>>()
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
            });
        }
    }
}
