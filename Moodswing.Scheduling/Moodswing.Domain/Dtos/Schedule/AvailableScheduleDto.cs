using System;
using System.Collections.Generic;

namespace Moodswing.Domain.Dtos.Schedule
{
    public class AvailableScheduleDto : ScheduleBaseDto
    {
        public IEnumerable<DateTime> AvailableDates { get; set; }
    }
}
