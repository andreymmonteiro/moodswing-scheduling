using Microsoft.Extensions.DependencyInjection;
using Moodswing.Domain.Factories.ScheduleFactory;
using Moodswing.Domain.Models.User;
using Moodswing.Domain.Services;
using Moodswing.Service.Factories.ScheduleFactory;
using Moodswing.Service.Services;

namespace Moodswing.Application.CrossCutting
{
    public static class SchedulesServiceConfiguration
    {
        public static IServiceCollection AddScheduleService(this IServiceCollection services)
        {
            services.AddTransient<ISchedulingService, SchedulingService>();
            services.AddTransient<IScheduleFacade, ScheduleFacade>();

            services.AddScoped<IUserObjectAuthenticationApi, UserObjectAuthenticationApi>();

            return services;
        }
    }
}
