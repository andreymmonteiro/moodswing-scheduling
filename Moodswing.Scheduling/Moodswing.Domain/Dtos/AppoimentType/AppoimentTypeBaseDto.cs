using Newtonsoft.Json;
using System;

namespace Moodswing.Domain.Dtos.AppoimentType
{
    public class AppoimentTypeBaseDto : BaseDto
    {
        public string Name { get; set; }

        private int _consultationTime;

        [JsonProperty("consultationTime")]
        public int GetConsultationTime 
        {
            get => _consultationTime != 30 ? 60 : 30; 
            set => _consultationTime  = value;
        }
    }
}
