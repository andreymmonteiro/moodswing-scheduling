using Microsoft.EntityFrameworkCore;
using Moodswing.Domain.Entities;

namespace Moodswing.Data.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions db) : base(db)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScheduleEntity>();
            modelBuilder.Entity<AppointmentTypeEntity>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
