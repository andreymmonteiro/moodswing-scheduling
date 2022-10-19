using Moodswing.Domain.Models;
using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public struct ScheduleDate
    {
        private readonly DateTime _date;

        private const int SEVEN_DAYS = 7;

        public ScheduleDate(DateTime date)
            => _date = date;

        public static implicit operator ScheduleDate(string date)
            => new(DateTime.TryParse(date, out var dateResult) ?
                dateResult : 
                DateTime.UtcNow.AddHours(TimezoneConstants.BR).AddDays(SEVEN_DAYS));

        public DateTime GetScheduleDate
            => _date;
    }
}
