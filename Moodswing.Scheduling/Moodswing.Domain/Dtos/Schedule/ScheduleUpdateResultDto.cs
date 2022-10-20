using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class ScheduleUpdateResultDto : ScheduleBaseResultDto
    {
        public DateTime UpdateAt { get; set; }
    }
}
