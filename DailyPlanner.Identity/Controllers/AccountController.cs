using System;
using System.Threading.Tasks;
using DailyPlanner.DomainClasses.Models;
using DailyPlanner.Identity.Models;
using DailyPlanner.Identity.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPlanner.Identity.Controllers
{

    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public Guid? UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string ExpirationTime { get; set; }
    }

    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IServiceProvider _svcprov;
        public AccountController(IServiceProvider svcprov)
        {
            _svcprov = svcprov;
        }

        [HttpPost]
        public async Task<Response> Register([FromBody] UserRegisterModel model)
        {
            try
            {
                var userRep = _svcprov.GetRequiredService<UserAuthRepository>();
                var user = await userRep.FindById(model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        IsActive = true,
                        Email = model.Username,
                        Password = PasswordsHelper.CreateHash(model.Password),
                        CreationDate = DateTime.UtcNow,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };

                    await userRep.Add(user);
                }

                return new Response
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = $"{ex.Message}; {ex.InnerException?.Message}"
                };
            }
        }
    }
}