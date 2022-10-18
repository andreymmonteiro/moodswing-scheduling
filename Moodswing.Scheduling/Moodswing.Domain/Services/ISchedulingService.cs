using Moodswing.Domain.Dtos.Schedule;
using Moodswing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moodswing.Domain.Services
{
    public interface ISchedulingService
    {
        Task<ScheduleCreateResultDto> InsertAsync(ScheduleEntity entity);

        Task<ScheduleUpdateResultDto> UpdateAsync(ScheduleEntity entity);

        Task<IEnumerable<ScheduleDto>> GetAllAsync();

        Task<ScheduleDto> GetByIdAsync(Guid id);

        Task<bool> DeleteAsync(Guid id);

    }
}
