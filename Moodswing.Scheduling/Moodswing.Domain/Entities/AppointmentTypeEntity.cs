using Moodswing.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moodswing.Domain.Entities
{
    [Table(AppointmentTypeConstants.APPOINTMENT_TYPE)]
    public sealed class AppointmentTypeEntity : BaseEntity
    {
        [Column(EntityConstants.NAME)]
        public string Name { get; set; }

        [Column(AppointmentTypeConstants.CONSULTATION_TIME)]
        public int ConsultationTime { get; set; }
    }
}
