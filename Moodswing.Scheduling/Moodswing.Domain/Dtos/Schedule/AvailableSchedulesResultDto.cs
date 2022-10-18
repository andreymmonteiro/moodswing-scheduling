using System;
using System.Collections.Generic;

namespace Moodswing.Domain.Dtos.Schedule
{
    public class AvailableSchedulesResultDto : BaseResultDto
    {
        public Guid CompanyId { get; set; }

        public Guid PersonId { get; set; }

        public IEnumerable<DateTime> AvailableDates { get; set; }
    }
}
