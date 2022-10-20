using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class ScheduleCreateResultDto : ScheduleBaseResultDto
    {

        public DateTime CreateAt { get; set; }
    }
}
