using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Models;
using Moodswing.Domain.Models.Strategies;
using Moodswing.Domain.Models.User;
using Moodswing.Domain.Services;
using System;
using System.Net;
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
        private readonly INotAvailableScheduleUseCase _availableScheduleUseCase;
        private IUserObjectAuthenticationApi _auth;

        public OrderController(
            IScheduleFacade facade, 
            IUserObjectAuthenticationApi auth, 
            ISchedulingService service, 
            INotAvailableScheduleUseCase availableScheduleUseCase)
        {
            _facade = facade;
            _auth = auth;
            _service = service;
            _availableScheduleUseCase = availableScheduleUseCase;
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

        [HttpPost]
        public async Task<IActionResult> CreateScheduleAsync(
            [FromHeader(Name = HttpRequestConstants.HEADER_ACCESSKEY)] string authorization,
            ScheduleBaseDto request)
        {
            if (string.IsNullOrWhiteSpace(authorization))
            {
                return Forbid();
            }
            _auth.Authorization = authorization;

            ScheduleBaseResultDto result = null;

            try
            {
                result = await _service.InsertAsync(request);
            }
            catch (Exception e)
            {
                result = new ScheduleBaseResultDto()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message ?? e?.InnerException?.Message,
                };
            }
            

            return StatusCode(((int)result.StatusCode), result);
        }


        /// <summary>
        /// Get Schedules Async
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         // If you set id the application will ignore the rest of parameters below
        ///         "id": "854f5598-ff18-410b-b772-032d915d9685",
        ///         
        ///         // You can set only companyId but not only personId
        ///         "companyId": "637a4458-ff18-410b-b772-032d915d5152",
        ///         "personId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         
        ///         // Optional: 
        ///         // If you do not set start and end date, the application will get the entire schedules of the currente date
        ///         // You can set only the start date and the application will consider entire schedules of the start day
        ///         "StartScheduleDate": "2022-11-08",
        ///         "EndScheduleDate": "2022-11-09"
        ///     }
        ///     
        /// </remarks>
        /// <returns>Users defined by command</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("schedules-not-available")]
        public async Task<IActionResult> GetSchedulesAsync([FromBody] NotAvailableScheduleRequestDto dto)
        {
            if (!dto.IsValidRequest)
            {
                return BadRequest(new BaseResultDto("You must define CompanyId and PersonId or just Id (schedule id)", HttpStatusCode.BadRequest));
            }

            try
            {

                var result = await _availableScheduleUseCase.GetNotAvailableSchedulesAsync(dto);
                return Ok(result);

            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResultDto($"{e?.Message}; {Environment.NewLine} {e?.InnerException?.Message}", HttpStatusCode.InternalServerError));
            }
        }
    }
}
