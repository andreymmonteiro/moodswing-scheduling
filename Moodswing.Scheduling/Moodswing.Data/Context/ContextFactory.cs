using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Moodswing.Data.Context
{
    internal class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        private const string ConnectionString = "User ID=doadmin;Password=AVNS_HXObS60oqm-9kkchy1N;Host=moodswing-do-user-12108898-0.b.db.ondigitalocean.com;Port=25060;Database=MoodswingScheduling;SSLMode=Require;Trust Server Certificate=true";
        private const string HMG = "User ID=postgres;Password=masterkey;Host=localhost;Port=5432;Database=MoodswingScheduling";
        private const int MAX_RETRY_COUNT = 5;
        private const int MAX_RETRY_DELAY = 10;
        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuidler = new DbContextOptionsBuilder<MyContext>();
            optionsBuidler.UseNpgsql(ConnectionString, options => 
            {
                options.EnableRetryOnFailure(MAX_RETRY_COUNT, TimeSpan.FromSeconds(MAX_RETRY_DELAY), null);
            });

            return new MyContext(optionsBuidler.Options);
        }
    }
}
