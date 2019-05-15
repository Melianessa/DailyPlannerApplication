using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DailyPlanner.Identity.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace DailyPlanner.Identity.Overrides
{
    public class ResourceOwnerPasswordValidator: IResourceOwnerPasswordValidator
    {
        private readonly UserAuthRepository _userRepository;

        public ResourceOwnerPasswordValidator(UserAuthRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.FindById(context.UserName);

            if (user == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
            }
            else
            {
                bool passed = false;

                try
                {
                    passed = PasswordsHelper.VerifyPassword(context.Password, user.Password);
                }
                catch (Exception ex)
                {
                   context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, ex.Message);
                }

                if (passed)
                    context.Result = new GrantValidationResult(user.Id.ToString(), context.Request.GrantType, DateTime.UtcNow, new[]
                    {
                        new Claim("sub", user.Id.ToString()),
                        new Claim("email", user.Email)
                    });
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
                }
            }
        }
    }
}
