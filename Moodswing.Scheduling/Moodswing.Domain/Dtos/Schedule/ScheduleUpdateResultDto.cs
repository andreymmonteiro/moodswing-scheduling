using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class ScheduleUpdateResultDto : ScheduleBaseDto
    {
        public DateTime UpdateAt { get; set; }
    }
}
