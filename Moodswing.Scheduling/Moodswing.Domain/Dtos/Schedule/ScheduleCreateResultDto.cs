using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class ScheduleCreateResultDto : ScheduleBaseDto
    {

        public DateTime CreateAt { get; set; }
    }
}
