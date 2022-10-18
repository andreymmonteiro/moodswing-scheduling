using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Moodswing.Data.Context
{
    internal class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        private const string ConnectionString = "User ID=role_admin;Password=jklp48@Tecnico!;Host=152.67.61.190;Port=5432;Database=MoodswingUsers";
        private const string HMG = "User ID=postgres;Password=masterkey;Host=localhost;Port=5432;Database=MoodswingScheduling";
        private const int MAX_RETRY_COUNT = 5;
        private const int MAX_RETRY_DELAY = 10;
        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuidler = new DbContextOptionsBuilder<MyContext>();
            optionsBuidler.UseNpgsql(HMG, options => 
            {
                options.EnableRetryOnFailure(MAX_RETRY_COUNT, TimeSpan.FromSeconds(MAX_RETRY_DELAY), null);
            });

            return new MyContext(optionsBuidler.Options);
        }
    }
}
