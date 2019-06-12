using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DailyPlanner.DomainClasses.Models
{
    public class AuthUser : IdentityUser
    {
        public int Year { get; set; }
    }
}
