using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DailyPlanner.Identity.Repositories;
using DailyPlanner.Repository;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace DailyPlanner.Identity.Overrides
{
    internal class ProfileService : IProfileService
    {
        private readonly UserAuthRepository _repository;

        public ProfileService(UserAuthRepository repository)
        {
            _repository = repository;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.Add(new Claim("sub", context.Subject.Claims.FirstOrDefault(cl => cl.Type == "sub")?.Value));
            context.IssuedClaims.Add(new Claim("email", context.Subject.Claims.FirstOrDefault(cl => cl.Type == "email")?.Value));

            return Task.FromResult(0);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.Claims.FirstOrDefault(cl => cl.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(userId))
                context.IsActive = false;
            else
                context.IsActive = await _repository.IsActive(Guid.Parse(userId));
        }
    }
}
