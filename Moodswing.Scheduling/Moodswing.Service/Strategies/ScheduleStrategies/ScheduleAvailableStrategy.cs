﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Moodswing.Data.Context;
using Moodswing.Data.Repositories;
using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.AppoimentType;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Dtos.User;
using Moodswing.Domain.Entities;
using Moodswing.Domain.Mapper;
using Moodswing.Domain.Models;
using Moodswing.Domain.Models.Strategies;
using Moodswing.Domain.Models.User;
using Moodswing.Domain.Strategies.ScheduleStrategies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Moodswing.Service.Strategies.ScheduleStrategies
{
    public class ScheduleAvailableStrategy : BaseRepository<ScheduleEntity>, IScheduleStrategy, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly MyContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private IUserObjectAuthenticationApi _authentication;
        private bool _disposed;

        private ScheduleAvailableParameters _scheduleStrategyParameters;
        private string _baseUrl;
        private const string PATH_USER_COMMAND = "api/Users/users-command";

        private const int THIRTY_MINUTES = 30;
        private const int SIXTY_MINUTES = 60;

        public ScheduleAvailableStrategy(
            MyContext context,
            IAutoMapperConfiguration mapper,
            IHttpClientFactory httpClientFactory,
            UserObjectApi userObjectApi,
            IUserObjectAuthenticationApi authentication) : base(context)
        {
            _context = context;
            _mapper = mapper.CreateMapper();
            _httpClientFactory = httpClientFactory;
            _baseUrl = userObjectApi.BaseUrl;
            _authentication = authentication;
        }

        public async Task<BaseResultDto> GetAsync<T>(T parameters)
            where T : BaseDto
        {
            var now = DateTime.UtcNow.AddHours(TimezoneConstants.BR);

            _scheduleStrategyParameters = ParseGeneric(parameters);

            if (_scheduleStrategyParameters.ScheduleDate.GetScheduleDate.Date < now.Date)
            {
                return new AvailableSchedulesResultDto()
                {
                    CompanyId = _scheduleStrategyParameters.CompanyId,
                    PersonId = _scheduleStrategyParameters.PersonId,
                    AvailableDates = new List<DateTime>(),
                    Message = "Date must be greater than or equal to now",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            if (!(await ExistCompanyOrPersonAsync(_scheduleStrategyParameters.PersonId, default)))
            {
                return new AvailableSchedulesResultDto()
                {
                    CompanyId = _scheduleStrategyParameters.CompanyId,
                    PersonId = _scheduleStrategyParameters.PersonId,
                    AvailableDates = new List<DateTime>(),
                    Message = "Person does not exist",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            const int company = 1;

            if (!(await ExistCompanyOrPersonAsync(_scheduleStrategyParameters.CompanyId, company)))
            {
                return new AvailableSchedulesResultDto()
                {
                    CompanyId = _scheduleStrategyParameters.CompanyId,
                    PersonId = _scheduleStrategyParameters.PersonId,
                    AvailableDates = new List<DateTime>(),
                    Message = "Person does not exist",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var _db = _context.Set<ScheduleEntity>();

            var schedules = await _db.AsNoTracking().Where(schedule =>
                                            schedule.ScheduleTime.Day >= now.Day &&
                                            schedule.ScheduleTime.Month >= now.Month &&
                                            schedule.ScheduleTime.Year >= now.Year &&
                                            schedule.CompanyId == _scheduleStrategyParameters.CompanyId).ToListAsync();

            var availableDays = CreateAvailableSchedule(GetNumberOfDays(_scheduleStrategyParameters, now), new OfficeHourEntity(), schedules);

            return new AvailableSchedulesResultDto()
            {
                CompanyId = _scheduleStrategyParameters.CompanyId,
                PersonId = _scheduleStrategyParameters.PersonId,
                AvailableDates = availableDays
            };
        }

        private static int GetNumberOfDays(ScheduleAvailableParameters parameters, DateTime now)
        {
            var days = parameters.ScheduleDate.GetScheduleDate - now.Date;
            return days.Days;
        }

        private IEnumerable<DateTime> CreateAvailableSchedule(int numberDays, OfficeHourEntity officeHour, List<ScheduleEntity> schedules)
        {
            var dateNow = DateTime.UtcNow.AddHours(TimezoneConstants.BR);

            var availableDays = new List<DateTime>();

            for (int indexDay = default; indexDay <= numberDays; indexDay++)
            {
                var actualDate = dateNow.AddDays(indexDay);

                if (actualDate.DayOfWeek == DayOfWeek.Saturday || actualDate.DayOfWeek == DayOfWeek.Monday)
                {
                    continue;
                }
                else
                {
                    var actualSchedule = schedules.Where(schedule => schedule.ScheduleTime.Date == actualDate.Date).ToList() ?? new();

                    GetAvailableDateTime(actualDate, actualSchedule, officeHour, ref availableDays);
                }
            }
            return availableDays;
        }

        private void GetAvailableDateTime(DateTime actualDate, List<ScheduleEntity> actualSchedule, OfficeHourEntity officeHour, ref List<DateTime> availableDays)
        {
            for (int timeHour = officeHour.InitialMorningHour; timeHour < officeHour.FinalMorningHour; timeHour++)
            {
                var newActualDate = new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, default, default);

                for (int timeMinute = default; timeMinute <= THIRTY_MINUTES; timeMinute += THIRTY_MINUTES)
                {
                    
                    if (officeHour.InitialMorningHour == timeHour)
                    {
                        newActualDate = newActualDate.AddMinutes(officeHour.InitialMorningMinutes);
                    }

                    var currentMinute = timeMinute == 1 ? default : timeMinute;

                    var currentDate = new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, currentMinute, default);

                    if (!actualSchedule.Any(schedule =>
                                schedule.ScheduleTime == currentDate))
                    {
                        if (IsOfficeTime(timeHour, currentMinute, officeHour.InitialMorningHour, officeHour.FinalMorningHour, officeHour.InitialMorningMinutes, officeHour.FinalMorningMinutes))
                        {
                            availableDays.Add(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, default));
                        }
                    }
                }
            }

            for (int timeHour = officeHour.InitialAfternoonHour; timeHour <= officeHour.FinalAfternoonHour; timeHour++)
            {

                for (int timeMinute = default; timeMinute <= THIRTY_MINUTES; timeMinute += THIRTY_MINUTES)
                {
                    var newActualDate = new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, default, default);

                    if (officeHour.InitialAfternoonHour == timeHour)
                    {
                        newActualDate = newActualDate.AddMinutes(officeHour.InitialAfternoonMinutes);
                    }

                    var currentMinute = timeMinute == 1 ? default : timeMinute;
                    
                    var currentDate = new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, currentMinute, default);

                    if (!actualSchedule.Any(schedule =>
                                schedule.ScheduleTime == currentDate))
                    {
                        if (IsOfficeTime(timeHour, currentMinute, officeHour.InitialAfternoonHour, officeHour.FinalAfternoonHour, officeHour.InitialAfternoonMinutes, officeHour.FinalAfternoonMinutes))
                        {
                            availableDays.Add(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, default));
                        }
                    }
                }
            }
        }

        private static bool IsOfficeTime(int timeHour, int currentMinute, int initialHour, int finalHour, int initialMinute, int finalMinute)
        {
            var initialTime = new TimeSpan(initialHour, initialMinute, default);
            var finalTime = new TimeSpan(finalHour, finalMinute, default);

            var currentTime = new TimeSpan(timeHour, currentMinute, default);

            return currentTime >= initialTime && currentTime < finalTime;
        }

        private ScheduleAvailableParameters ParseGeneric<T>(T value) where T : class
            => CheckStrategyObject(value as ScheduleAvailableParameters);

        private ScheduleAvailableParameters CheckStrategyObject(ScheduleAvailableParameters value)
            => value is null ? new ScheduleAvailableParameters() : value;

        private async Task<bool> ExistCompanyOrPersonAsync(Guid companyOrPersonId, int personType)
        {
            var httpClient = CreateHttpRequest();
            var content = new StringContent(CreateRequestObject(companyOrPersonId, personType), Encoding.UTF8, HttpRequestConstants.APPLICATION_JSON);

            using var response = await httpClient.PostAsync($"{PATH_USER_COMMAND}", content, new CancellationToken());

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var userResult = JsonConvert.DeserializeObject<UsersDto>(responseContent);

            return
                userResult.Users.FirstOrDefault() is not null && !string.IsNullOrWhiteSpace(userResult.Users.FirstOrDefault().Name);
        }

        private HttpClient CreateHttpRequest()
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_baseUrl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpRequestConstants.APPLICATION_JSON));
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, _authentication.Authorization);

            return client;
        }

        private static string CreateRequestObject(Guid companyOrPersonId, int personType)
        {
            var columns = new Dictionary<string, string>()
            {
                { "u.name", "name" }
            };
            var whereClause = $" u.person_type = {personType} and u.id = '{companyOrPersonId}' ";

            return JsonConvert.SerializeObject(new
            {
                columns = columns,
                where = whereClause
            });
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _context.Dispose();
                _scheduleStrategyParameters = null;
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}