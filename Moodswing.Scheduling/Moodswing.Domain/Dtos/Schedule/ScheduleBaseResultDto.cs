using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public class ScheduleBaseResultDto : BaseResultDto
    {
        public Guid CompanyId { get; set; }

        public Guid PersonId { get; set; }

        public DateTime ScheduleTime { get; set; }
    }
}
