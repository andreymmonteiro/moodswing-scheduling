using Moodswing.Domain.Models;
using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public struct ScheduleDate
    {
        private readonly DateTime? _date;

        public ScheduleDate(DateTime date)
            => _date = date;

        public ScheduleDate(string date)
            => _date = DateTime.TryParse(date, out var dateResult) ? dateResult : null;

        public DateTime? GetScheduleDate
            => _date ?? DateTime.UtcNow.AddHours(TimezoneConstants.BR).AddDays(7);
    }
}
