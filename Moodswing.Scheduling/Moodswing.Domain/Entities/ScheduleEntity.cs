using Moodswing.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moodswing.Domain.Entities
{
    [Table(ScheduleConstants.SCHEDULE)]
    public class ScheduleEntity : BaseEntity
    {
        [Column(ScheduleConstants.COMPANY_ID)]
        public Guid CompanyId { get; set; }

        [Column(ScheduleConstants.PERSON_ID)]
        public Guid PersonId { get; set; }
        
        [Column(ScheduleConstants.SCHEDULE_TIME)]
        public DateTime ScheduleTime { get; set; }

        [Column(ScheduleConstants.APPOINTMENT_TYPE_ID)]
        public Guid AppointmentTypeId { get; set; }

        public AppointmentTypeEntity AppointmentType { get; set; }
    }
}
