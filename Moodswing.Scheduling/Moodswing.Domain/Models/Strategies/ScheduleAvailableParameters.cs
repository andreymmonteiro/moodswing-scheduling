using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.Schedule;
using System;

namespace Moodswing.Domain.Models.Strategies
{
    public sealed class ScheduleAvailableParameters : BaseDto
    {
        public ScheduleDate ScheduleDate { get; set; }

        public Guid CompanyId { get; set; }

        public Guid PersonId { get; set; }
    }
}
