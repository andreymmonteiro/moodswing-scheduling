using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Models;
using Moodswing.Domain.Models.Strategies;
using Moodswing.Domain.Models.User;
using Moodswing.Domain.Services;
using System.Threading.Tasks;

namespace Moodswing.Application.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IScheduleFacade _facade;
        private readonly ISchedulingService _service;
        private IUserObjectAuthenticationApi _auth;

        public OrderController(IScheduleFacade facade, IUserObjectAuthenticationApi auth, ISchedulingService service)
        {
            _facade = facade;
            _auth = auth;
            _service = service;
        }

        [HttpPost("available-dates")]
        public async Task<IActionResult> GetAvailabledatesAsync(
            [FromHeader(Name = HttpRequestConstants.HEADER_ACCESSKEY)] string authorization,
            ScheduleAvailableParameters request)
        {
            if (string.IsNullOrWhiteSpace(authorization))
            {
                return Forbid();
            }
            _auth.Authorization = authorization;
            var result = await _facade.GetResultAsync<AvailableSchedulesResultDto>(request, Domain.Models.ScheduleStrategies.Available);

            return StatusCode(((int)result.StatusCode), result);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateScheduleAsync()
        //{

        //}
    }
}
