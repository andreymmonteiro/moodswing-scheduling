using Moodswing.Domain.Models;
using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public struct ScheduleDate
    {
        private readonly DateTime _date;

        private const int SEVEN_DAYS = 7;

        public static DateTime GenerateCurrentDate()
        {
            var now = DateTime.UtcNow.AddHours(- TimezoneConstants.BR);

            return new DateTime(now.Year, now.Month, now.Day, default, default, default);
        }

        public ScheduleDate(DateTime date)
            => _date = date;

        public static implicit operator ScheduleDate(string date)
            => new(DateTime.TryParse(date, out var dateResult) ?
                dateResult : 
                GenerateCurrentDate().AddDays(SEVEN_DAYS));

        public DateTime GetScheduleDate
            => _date == DateTime.MinValue ? GenerateCurrentDate().AddDays(SEVEN_DAYS) : _date;
    }
}
