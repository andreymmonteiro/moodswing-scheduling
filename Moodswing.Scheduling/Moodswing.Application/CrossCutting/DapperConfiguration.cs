using Microsoft.Extensions.DependencyInjection;
using Moodswing.Domain.Services.Dapper;
using Moodswing.Service.Dapper;

namespace Moodswing.Application.CrossCutting
{
    public static class DapperConfiguration
    {
        public static IServiceCollection AddDappeService(this IServiceCollection services)
            => services.AddTransient(typeof(IDapperService<>), typeof(DapperService<>));
    }
}
