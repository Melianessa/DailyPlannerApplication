using DailyPlanner.DomainClasses.Models;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.Repository
{
    public class PlannerDbContext:DbContext
    {
        public PlannerDbContext(DbContextOptions opt):base(opt)
        { }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<PersistedGrant> PersistedGrants { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
