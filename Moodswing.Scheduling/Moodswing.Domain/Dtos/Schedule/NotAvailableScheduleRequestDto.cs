using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public sealed class NotAvailableScheduleRequestDto
    {
        public Guid? Id { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? PersonId { get; set; }

        public DateTime? StartScheduleDate { get; set; }

        public DateTime? EndScheduleDate { get; set; }

        public bool IsValidRequest 
            => Id.HasValue || CompanyId.HasValue || (CompanyId.HasValue && PersonId.HasValue);
    }
}
