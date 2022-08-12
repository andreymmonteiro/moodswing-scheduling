using Microsoft.AspNetCore.Mvc;
using Moodswing.Domain.Models;
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

            var devs = new CelDevelopers()
            {
                Developers = new List<Developer>() 
                {
                    new Developer()
                    {
                        Name = "Andrey Monteiro",
                        Cel = "Boston"
                    },
                    new Developer()
                    {
                        Name = "Gideval Santos",
                        Cel = "Boston"
                    },
                    new Developer()
                    {
                        Name = "Matheus Hoffman",
                        Cel = "Boston"
                    }
                }
            };

            var result = new CelDevelopers(!string.IsNullOrWhiteSpace(name) ?
                    devs.Developers.Where(dev => dev.Name.Contains(name)) :
                    devs.Developers);

            return Ok(result);
        }
    }
}
