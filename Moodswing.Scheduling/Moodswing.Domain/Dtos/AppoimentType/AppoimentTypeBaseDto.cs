namespace Moodswing.Domain.Dtos.AppoimentType
{
    public class AppoimentTypeBaseDto : BaseDto
    {
        public string Name { get; set; }

        private int? ConsultationTime { get; set; }

        public int GetConsultationTime
            => ConsultationTime ?? 60;
    }
}
