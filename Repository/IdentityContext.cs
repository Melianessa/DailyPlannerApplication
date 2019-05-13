using System;
using System.Collections.Generic;
using System.Text;
using DailyPlanner.DomainClasses.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DailyPlanner.Repository
{
    public class IdentityContext : IdentityDbContext<AuthUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
