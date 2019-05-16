using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DailyPlanner.Identity.Models;
using DailyPlanner.Identity.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailyPlanner.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        IdentityHelper _apiBaseURI = new IdentityHelper();
        private readonly ILogger _logger;
        public AccountController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public async Task<Response> Register([FromBody] UserRegisterModel model)
        {
            try
            {
                HttpClient client = _apiBaseURI.InitializeClient();
                if (ModelState.IsValid)
                {
                    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                        "application/json");
                    HttpResponseMessage res = await client.PostAsync("account/register", content);
                    if (res.IsSuccessStatusCode)
                    {
                        return new Response
                        {
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                        };
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Register method: {e.Message}");
            }

            return new Response
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
            };
        }
    }

    public class IdentityHelper
    {
        //private string _apiBaseURI = "https://dailyplannerapi.azurewebsites.net";
        private string _apiBaseURI = "http://localhost:5000";
        public HttpClient InitializeClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_apiBaseURI)
            };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}