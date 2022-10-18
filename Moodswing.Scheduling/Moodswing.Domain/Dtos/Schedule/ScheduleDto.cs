using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class ScheduleDto : ScheduleBaseDto
    {
        public DateTime UpdateAt { get; set; }
    }
}
