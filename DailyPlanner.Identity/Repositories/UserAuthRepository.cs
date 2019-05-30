using System;
using System.Threading.Tasks;
using DailyPlanner.DomainClasses.Models;
using DailyPlanner.Repository;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.Identity.Repositories
{
    public class UserAuthRepository
    {
        private readonly PlannerDbContext _dbconnection;

        public UserAuthRepository(PlannerDbContext dbconnection)
        {
            _dbconnection = dbconnection;
        }
        public async Task Add(User user)
        {
            user.IsActive = true;
            _dbconnection.Users.Add(user);
            await _dbconnection.SaveChangesAsync();
        }

        public async Task<User> FindById(string email)
        {
            return await _dbconnection.Users.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<bool> IsActive(Guid userId)
        {
            return (await _dbconnection.Users.FirstOrDefaultAsync(p => p.Id == userId))?.IsActive ?? false;
        }
    }
}
