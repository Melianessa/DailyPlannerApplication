using System.Threading.Tasks;
using DailyPlanner.Identity.Repositories;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace DailyPlanner.Identity.Overrides
{
    internal class ClientStore : IClientStore
    {
        private readonly UserAuthRepository _repository;

        public ClientStore(UserAuthRepository repository)
        {
            _repository = repository;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var dbClient = await _repository.FindById(clientId);

            return new Client
            {
                ClientId = dbClient.Email,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("123456")
                },
                AllowedScopes =
                {
                    "DailyPlanner.API",
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Phone,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,

                },
                RequireClientSecret = false,
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                // 4 weeks
                SlidingRefreshTokenLifetime = 7 * 24 * 60 * 60,
                AccessTokenType = AccessTokenType.Jwt,
                // 1 hour
                AccessTokenLifetime = 60 * 60 * 24 * 10,
                UpdateAccessTokenClaimsOnRefresh = true
            };
        }
    }
}