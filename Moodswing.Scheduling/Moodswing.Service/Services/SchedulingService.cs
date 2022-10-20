using AutoMapper;
using Moodswing.Data.Repositories;
using Moodswing.Domain.Dtos.AppoimentType;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Entities;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Mapper;
using Moodswing.Domain.Models.Strategies;
using Moodswing.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Moodswing.Service.Services
{
    public class SchedulingService : ISchedulingService
    {
        private readonly IRepository<ScheduleEntity> _repository;
        private readonly IMapper _mapper;
        private readonly IScheduleFacade _facade;

        public SchedulingService(
            IRepository<ScheduleEntity> repository, 
            IAutoMapperConfiguration mapper, 
            IScheduleFacade facade)
        {
            _repository = repository;
            _mapper = mapper.CreateMapper();
            _facade = facade;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _repository.DeleteAsync(id).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<ScheduleDto>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync().ConfigureAwait(false);
            return _mapper.Map<IEnumerable<ScheduleDto>>(result);
        }

        public async Task<ScheduleDto> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetAsync(id).ConfigureAwait(false);
            return _mapper.Map<ScheduleDto>(result);
        }

        public async Task<ScheduleCreateResultDto> InsertAsync(ScheduleBaseDto entity)
        {
            if (entity is null)
            {
                return default;
            }

            //Include mongoDB 
            var schedules = await _facade.GetResultAsync<AvailableSchedulesResultDto>(new ScheduleAvailableParameters()
            {
                ScheduleDate = new ScheduleDate(entity.ScheduleTime),
                CompanyId = entity.CompanyId,
                PersonId = entity.PersonId,
            }, Domain.Models.ScheduleStrategies.Available);

            entity.ScheduleTime = entity.ScheduleTime.AddSeconds(-entity.ScheduleTime.Second).AddMilliseconds(-entity.ScheduleTime.Millisecond);

            if (schedules.AvailableDates.Any(date => date == entity.ScheduleTime))
            {
                var result = await _repository.InsertAsync(_mapper.Map<ScheduleEntity>(entity)).ConfigureAwait(false);

                if(entity.AppointmentType.GetConsultationTime == 60)
                {
                    entity.ScheduleTime = entity.ScheduleTime.AddMinutes(30);
                    result = await _repository.InsertAsync(_mapper.Map<ScheduleEntity>(entity)).ConfigureAwait(false);
                }

                return _mapper.Map<ScheduleCreateResultDto>(result);
            }

            return new ScheduleCreateResultDto()
            {
                Message   = "Date not available",
                StatusCode = HttpStatusCode.BadRequest
            };   
        }

        public async Task<ScheduleUpdateResultDto> UpdateAsync(ScheduleEntity entity)
        {
            if(entity is null)
            {
                var result = await _repository.UpdateAsync(entity).ConfigureAwait(false);
                return _mapper.Map<ScheduleUpdateResultDto>(result);
            }
            return default;
        }
    }
}
