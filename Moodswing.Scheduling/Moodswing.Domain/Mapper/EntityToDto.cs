using AutoMapper;
using Moodswing.Domain.Dtos.AppoimentType;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Entities;

namespace Moodswing.Domain.Mapper
{
    public sealed class EntityToDto : Profile
    {
        public EntityToDto()
        {
            ScheduleEntityToDto();
        }

        private void ScheduleEntityToDto()
        {
            CreateMap<ScheduleEntity, ScheduleDto>().ReverseMap();
            CreateMap<ScheduleEntity, ScheduleCreateResultDto>().ReverseMap();
            CreateMap<ScheduleEntity, ScheduleUpdateResultDto>().ReverseMap();
            CreateMap<ScheduleEntity, ScheduleBaseDto>().ReverseMap();
            CreateMap<AppointmentTypeEntity, AppointmentTypeDto>().ReverseMap();
            CreateMap<AppointmentTypeEntity, AppoimentTypeCreateResultDto>().ReverseMap();
            CreateMap<AppointmentTypeEntity, AppoimentTypeUpdateResultDto>().ReverseMap();
            CreateMap<AppointmentTypeEntity, AppoimentTypeBaseDto>().ReverseMap();
        }
    }
}
