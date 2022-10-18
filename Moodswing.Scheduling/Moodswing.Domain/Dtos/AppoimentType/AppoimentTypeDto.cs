using System;

namespace Moodswing.Domain.Dtos.AppoimentType
{
    public sealed class AppoimentTypeDto : AppoimentTypeBaseDto
    {
        public DateTime CreateAt { get; set; }

        public DateTime UpdateAt { get; set; }
    }
}
