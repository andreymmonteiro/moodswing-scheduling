﻿using Moodswing.Domain.Dtos.AppoimentType;
using System;

namespace Moodswing.Domain.Dtos.Schedule
{
    public class ScheduleBaseDto  : BaseDto
    {
        public Guid CompanyId { get; set; }

        public Guid PersonId { get; set; }

        public DateTime ScheduleTime { get; set; }

        public AppointmentTypeDto AppointmentType { get; set; }
    }
}