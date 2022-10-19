using Microsoft.Extensions.DependencyInjection;
using Moodswing.Domain.Mapper;
using Moodswing.Service.Mapper;

namespace Moodswing.Application.CrossCutting
{
    public static class ConfigureMapper
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
            => services.AddScoped<IAutoMapperConfiguration, AutoMapperConfiguration>();
    }
}
