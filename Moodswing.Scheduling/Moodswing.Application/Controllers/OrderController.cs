using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Models.Strategies;
using System.Threading.Tasks;

namespace Moodswing.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IScheduleFacade _facade;

        public OrderController(IScheduleFacade facade)
        {
            _facade = facade;
        }

        [HttpGet]
        //[Authorize]
        public IActionResult GetStatusOkAsync([FromQuery] string name)
        {

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailabledatesAsync(ScheduleAvailableParameters request)
        {
            var result = await _facade.GetResultAsync<AvailableSchedulesResultDto>(request, Domain.Models.ScheduleStrategies.Available);

            return Ok(result);
        }
    }
}
