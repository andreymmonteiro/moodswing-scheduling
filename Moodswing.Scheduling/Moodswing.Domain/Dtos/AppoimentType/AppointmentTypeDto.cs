using System;

namespace Moodswing.Domain.Dtos.AppoimentType
{
    public sealed class AppointmentTypeDto : AppoimentTypeBaseDto
    {
        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
