using Moodswing.Domain.Dtos;
using Moodswing.Domain.Dtos.Schedule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moodswing.Domain.Strategies.ScheduleStrategies
{
    public interface IScheduleStrategy
    {
        Task<BaseResultDto> GetAsync<T>(T parameters) 
            where T : BaseDto;
    }
}
