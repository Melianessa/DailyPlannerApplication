using System;
using System.ComponentModel;
using System.Security.Claims;

namespace DailyPlanner.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst("sub").Value);
        }

        public static string GetPhone(this ClaimsPrincipal user)
        {
            return user.FindFirst("phone").Value;
        }

        public static string GetClientId(this ClaimsPrincipal user)
        {
            return user.FindFirst("client_id").Value;
        }
    }
}
