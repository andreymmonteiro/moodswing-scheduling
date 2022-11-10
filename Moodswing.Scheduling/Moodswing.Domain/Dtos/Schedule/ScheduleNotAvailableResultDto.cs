using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public class ScheduleNotAvailableResultDto : ScheduleBaseResultDto
    {
        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
