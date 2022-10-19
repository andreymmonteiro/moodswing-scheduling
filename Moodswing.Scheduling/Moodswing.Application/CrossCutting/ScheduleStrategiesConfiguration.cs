using Microsoft.Extensions.DependencyInjection;
using Moodswing.Domain.Models;
using Moodswing.Domain.Strategies.ScheduleStrategies;
using Moodswing.Service.Strategies.ScheduleStrategies;
using System;

namespace Moodswing.Application.CrossCutting
{
    public static class ScheduleStrategiesConfiguration
    {
        public static IServiceCollection AddScheduleStrategyConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ScheduleAvailableStrategy>();

            services.AddTransient<Func<ScheduleStrategies, IScheduleStrategy>>(provider => key => 
            {
                return key switch
                {
                    ScheduleStrategies.Available => provider.GetRequiredService<ScheduleAvailableStrategy>(),
                    _ => provider.GetRequiredService<ScheduleAvailableStrategy>()
                };
            });
            return services;
        }
    }
}
