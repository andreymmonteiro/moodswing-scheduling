using Moodswing.Domain.Dtos;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Models;
using Moodswing.Domain.Strategies.ScheduleStrategies;
using System;
using System.Threading.Tasks;

namespace Moodswing.Service.Factories.ScheduleFactory
{
    public sealed class ScheduleFacade : IScheduleFacade
    {
        private readonly Func<ScheduleStrategies, IScheduleStrategy> _strategies;

        public ScheduleFacade(Func<ScheduleStrategies, IScheduleStrategy> strategies)
        {
            _strategies = strategies;
        }

        public async Task<TResult> GetResultAsync<TResult>(BaseDto request, ScheduleStrategies strategy) where TResult : BaseResultDto
            => await _strategies(strategy).GetAsync(request) as TResult;
    }
}
