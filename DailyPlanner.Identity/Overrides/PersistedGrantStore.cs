//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using DailyPlanner.Identity.Repositories;
//using IdentityServer4.Models;
//using IdentityServer4.Stores;

//namespace DailyPlanner.Identity.Overrides
//{
//    public class PersistedGrantStore : IPersistedGrantStore
//    {
//        private readonly PersistedGrantRepository _repository;

//        public PersistedGrantStore(PersistedGrantRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
//        {
//            return await _repository.GetAll(subjectId);
//        }

//        public async Task<PersistedGrant> GetAsync(string key)
//        {
//            return await _repository.Get(key);
//        }

//        public async Task RemoveAllAsync(string subjectId, string clientId)
//        {
//            await _repository.RemoveAll(subjectId, clientId);
//        }

//        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
//        {
//            await _repository.RemoveAll(subjectId, clientId, type);
//        }

//        public async Task RemoveAsync(string key)
//        {
//            await _repository.Remove(key);
//        }

//        public async Task StoreAsync(PersistedGrant grant)
//        {
//            await _repository.Add(grant);
//        }
//    }
//}
