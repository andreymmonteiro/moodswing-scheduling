﻿using Microsoft.Extensions.DependencyInjection;
using Moodswing.Data.Repositories;

namespace Moodswing.Application.CrossCutting
{
    public static class ConfigureRepository
    {
        public static void AddGenericRepository(this IServiceCollection services)
            => services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
    }
}
