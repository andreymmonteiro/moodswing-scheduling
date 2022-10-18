using Microsoft.Extensions.DependencyInjection;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Services;
using Moodswing.Service.Factories.ScheduleFactory;
using Moodswing.Service.Services;

namespace Moodswing.Application.CrossCutting
{
    public static class SchedulesServiceConfiguration
    {
        public static void AddScheduleService(this IServiceCollection services)
        {
            services.AddTransient<ISchedulingService, SchedulingService>();
            services.AddTransient<IScheduleFacade, ScheduleFacade>();
        }
    }
}
