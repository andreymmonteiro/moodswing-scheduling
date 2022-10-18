using System;

namespace Moodswing.Domain.Dtos.OfficeHour
{
    public sealed class OfficeHourDto
    {
        public int InitialHour { get; set; }

        public int FinalHour { get; set; }

        public int InitialMinutes { get; set; }

        public int FinalMinutes { get; set; }

        public Guid CompanyId { get; set; }
    }
}
