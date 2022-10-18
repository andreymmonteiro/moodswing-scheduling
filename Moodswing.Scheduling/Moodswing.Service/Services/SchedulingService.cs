using AutoMapper;
using Moodswing.Data.Repositories;
using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Entities;
using Moodswing.Domain.Mapper;
using Moodswing.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moodswing.Service.Services
{
    public class SchedulingService : ISchedulingService
    {
        private readonly IRepository<ScheduleEntity> _repository;
        private readonly IMapper _mapper;

        public SchedulingService(IRepository<ScheduleEntity> repository, IAutoMapperConfiguration mapper)
        {
            _repository = repository;
            _mapper = mapper.CreateMapper();
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

        public async Task<ScheduleCreateResultDto> InsertAsync(ScheduleEntity entity)
        {
            if (entity is null)
            {
                return default;
            }
            var result = await _repository.InsertAsync(entity).ConfigureAwait(false);
            return _mapper.Map<ScheduleCreateResultDto>(result);
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
