using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moodswing.Domain.Entities
{
    [Table("office_hour")]
    public class OfficeHourEntity : BaseEntity
    {
        [Column("initial_morning_hour")]
        public int InitialMorningHour { get; set; } = 8;


        [Column("final_morning_hour")]
        public int FinalMorningHour { get; set; } = 12;

        [Column("final_afternoon_hour")]
        public int FinalAfternoonHour { get; set; } = 18;

        [Column("initial_afternoon_hour")]
        public int InitialAfternoonHour { get; set; } = 13;

        [Column("initial_morning_minutes")]
        public int InitialMorningMinutes { get; set; } = 30;

        [Column("final_morning_minutes")]
        public int FinalMorningMinutes { get; set; } = 30;

        [Column("initial_morning_minutes")]
        public int InitialAfternoonMinutes { get; set; } = 30;

        [Column("final_morning_minutes")]
        public int FinalAfternoonMinutes { get; set; } = 30;

        [Column("company_id")]
        public Guid CompanyId { get; set; }
    }
}
