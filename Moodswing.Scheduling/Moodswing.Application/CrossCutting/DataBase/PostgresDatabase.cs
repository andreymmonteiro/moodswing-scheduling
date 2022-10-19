using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moodswing.Data.Context;
using System;

namespace Moodswing.Application.CrossCutting.DataBase
{
    public static class PostgresDatabase
    {
        public static string ConnectionString { get; set; }

        private readonly static int MAX_RETRY_COUNT = 5;

        private readonly static int MAX_RETRY_DELAY = 5;

        public static IServiceCollection InitializeDatabase(this IServiceCollection services)
        {
            services.AddDbContext<MyContext>(options => options.UseNpgsql(ConnectionString, dbOptions =>
            {
                dbOptions.EnableRetryOnFailure(MAX_RETRY_COUNT, TimeSpan.FromSeconds(MAX_RETRY_DELAY), null);
            }));
            return services;
        }
    }
}
