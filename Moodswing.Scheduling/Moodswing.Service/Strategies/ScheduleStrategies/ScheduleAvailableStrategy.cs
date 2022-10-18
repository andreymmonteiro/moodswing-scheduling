using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moodswing.Data.Context;
using Moodswing.Data.Repositories;
using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Entities;
using Moodswing.Domain.Mapper;
using Moodswing.Domain.Models;
using Moodswing.Domain.Models.Strategies;
using Moodswing.Domain.Strategies.ScheduleStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moodswing.Service.Strategies.ScheduleStrategies
{
    public class ScheduleAvailableStrategy : BaseRepository<ScheduleEntity>, IScheduleStrategy, IDisposable
    {
        private readonly IMapper _mapper;
        private readonly MyContext _context;
        private readonly bool _disposed;

        private ScheduleAvailableParameters _scheduleStrategyParameters;

        private const int THIRTY_MINUTES = 30;
        private const int SIXTY_MINUTES = 60;

        public ScheduleAvailableStrategy(MyContext context, IAutoMapperConfiguration mapper) : base(context)
        {
            _context = context;
            _mapper = mapper.CreateMapper();
        }

        public async Task<BaseResultDto> GetAsync<T>(T parameters)
            where T : BaseDto
        {
            var now = DateTime.UtcNow.AddHours(TimezoneConstants.BR);

            _scheduleStrategyParameters = ParseGeneric(parameters);

            var _db = _context.Set<ScheduleEntity>();

            var schedules = await _db.AsNoTracking().Where(schedule => 
                                            schedule.ScheduleTime >= now && 
                                            schedule.CompanyId == _scheduleStrategyParameters.CompanyId &&
                                            schedule.PersonId == _scheduleStrategyParameters.PersonId).ToListAsync();

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
            return days.HasValue ? days.Value.Days : default;
        }

        private IEnumerable<DateTime> CreateAvailableSchedule(int numberDays, OfficeHourEntity officeHour, List<ScheduleEntity> schedules)
        {
            var dateNow = DateTime.UtcNow.AddHours(TimezoneConstants.BR);

            var availableDays = new List<DateTime>();

            for (int indexDay = default; indexDay <= numberDays; indexDay++)
            {
                var actualDate = dateNow.AddDays(indexDay);

                var actualSchedule = schedules.Where(schedule => schedule.ScheduleTime.Date == actualDate.Date).ToList() ?? new();

                GetAvailableDateTime(actualDate, actualSchedule, officeHour, ref availableDays);
            }
            return availableDays;
        }

        private void GetAvailableDateTime(DateTime actualDate, List<ScheduleEntity> actualSchedule, OfficeHourEntity officeHour, ref List<DateTime> availableDays)
        {
            for (int timeHour = officeHour.InitialMorningHour; timeHour < officeHour.FinalMorningHour; timeHour++)
            {
                for (int timeMinute = officeHour.InitialMorningMinutes; timeMinute <= _scheduleStrategyParameters.AppointmentType.GetConsultationTime; timeMinute += THIRTY_MINUTES)
                {
                    if(timeMinute == 60)
                    {
                        continue;
                    }
                    else if (actualSchedule.Count == default)
                    {
                        availableDays.Add(new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, timeMinute, default));
                    }
                    else if (!actualSchedule.Any(schedule => timeHour == schedule.ScheduleTime.Hour && timeMinute == schedule.ScheduleTime.Minute))
                    {
                        availableDays.Add(new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, timeMinute, default));
                    }
                }
            }

            for (int timeHour = officeHour.InitialAfternoonHour; timeHour < officeHour.FinalAfternoonHour; timeHour++)
            {
                for (int timeMinute = officeHour.InitialAfternoonMinutes; timeMinute <= _scheduleStrategyParameters.AppointmentType.GetConsultationTime; timeMinute += THIRTY_MINUTES)
                {
                    if (timeMinute == 60)
                    {
                        continue;
                    }
                    else if (actualSchedule.Count == default)
                    {
                        availableDays.Add(new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, timeMinute, default));
                    }
                    else if (!actualSchedule.Any(schedule => timeHour == schedule.ScheduleTime.Hour && timeMinute == schedule.ScheduleTime.Minute))
                    {
                        availableDays.Add(new DateTime(actualDate.Year, actualDate.Month, actualDate.Day, timeHour, timeMinute, default));
                    }
                }
            }
        }

        private ScheduleAvailableParameters ParseGeneric<T>(T value) where T : class
            => CheckStrategyObject(value as ScheduleAvailableParameters);

        private ScheduleAvailableParameters CheckStrategyObject(ScheduleAvailableParameters value)
            => value is null ? new ScheduleAvailableParameters() : value;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
            }
        }
    }
}
