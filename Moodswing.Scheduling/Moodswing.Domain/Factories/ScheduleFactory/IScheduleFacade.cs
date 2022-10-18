using Moodswing.Domain.Dtos;
using Moodswing.Domain.Models;
using System.Threading.Tasks;

namespace Moodswing.Domain.Factories.ScheduleFactory
{
    public interface IScheduleFacade
    {
        Task<TResult> GetResultAsync<TResult>(BaseDto request, ScheduleStrategies strategy) where TResult : BaseResultDto;

        
    }
}
