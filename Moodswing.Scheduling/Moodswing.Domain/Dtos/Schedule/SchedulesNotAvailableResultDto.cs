using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class SchedulesNotAvailableResultDto : BaseResultDto
    {
        public IEnumerable<ScheduleNotAvailableResultDto> SchedulesNotAvailable { get; set; }

        public SchedulesNotAvailableResultDto(IEnumerable<ScheduleNotAvailableResultDto> schedulesNotAvailable)
            => SchedulesNotAvailable = schedulesNotAvailable;
    }
}
