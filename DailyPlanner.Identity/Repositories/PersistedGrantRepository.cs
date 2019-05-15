//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using DailyPlanner.Repository;
//using IdentityServer4.Models;
//using Microsoft.EntityFrameworkCore;

//namespace DailyPlanner.Identity.Repositories
//{
//    public class PersistedGrantRepository
//    {
//        private readonly PlannerDbContext _dbconnection;

//        public PersistedGrantRepository(PlannerDbContext dbconnection)
//        {
//            _dbconnection = dbconnection;
//        }

//        public async Task<IEnumerable<PersistedGrant>> GetAll(string subjectId)
//        {
//            return await _dbconnection.PersistedGrants.Where(p => p.SubjectId == subjectId).ToListAsync();
//        }

//        public async Task<PersistedGrant> Get(string key)
//        {
//            return await _dbconnection.PersistedGrants.FirstOrDefaultAsync(p => p.Key == key);
//        }

//        public async Task RemoveAll(string subjectId, string clientId)
//        {
//            var entities = await _dbconnection.PersistedGrants.Where(p => p.SubjectId == subjectId && p.ClientId == clientId)
//                .ToListAsync();
//            _dbconnection.RemoveRange(entities);
//            await _dbconnection.SaveChangesAsync();
//        }

//        public async Task RemoveAll(string subjectId, string clientId, string type)
//        {
//            var entities = await _dbconnection.PersistedGrants.Where(p => p.SubjectId == subjectId && p.ClientId == clientId&&p.Type==type)
//                .ToListAsync();
//            _dbconnection.RemoveRange(entities);
//            await _dbconnection.SaveChangesAsync();
//        }

//        public async Task Remove(string key)
//        {
//            var entity = await _dbconnection.PersistedGrants.FirstOrDefaultAsync(p => p.Key == key);
//            _dbconnection.Remove(entity);
//            await _dbconnection.SaveChangesAsync();
//        }

//        public async Task Add(PersistedGrant grant)
//        {
//            await _dbconnection.AddAsync(grant);
//            await _dbconnection.SaveChangesAsync();
//        }
//    }
//}
