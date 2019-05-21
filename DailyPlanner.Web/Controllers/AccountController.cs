using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/[controller]/[action]")]
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
                        var result = await res.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<Response>(result);
                        return response;
                    }
                    else
                    {
                        _logger.LogWarning("Error in Register method, response status code is not success");
                        return new Response
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = "Response status code is not success"
                        };
                    }
                }
                return new Response
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = "Model is not valid"
                };
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Register method: {e.Message}");
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                };
            }
        }

        [HttpPost]
        public async Task<Response> Login([FromBody]UserLoginModel model)
        {
            try
            {
                HttpClient client = _apiBaseURI.InitializeClient();
                if (ModelState.IsValid)
                {
                    model.Client_id = model.Username;
                    model.Client_secret = "123456";
                    model.Grant_type = "password";
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(model));
                    var req = new HttpRequestMessage(HttpMethod.Post, "connect/token")
                    {
                        Content = new FormUrlEncodedContent(dict)
                    };
                    var res = await client.SendAsync(req);
                    if (res.IsSuccessStatusCode)
                    {
                        var result = await res.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                        if (response.ContainsKey("access_token"))
                        {
                            return new Response
                            {
                                IsSuccess = true,
                                StatusCode = StatusCodes.Status200OK,
                                Token = response["access_token"]
                            };
                        }
                        _logger.LogWarning("Error in Login method, token is NULL");
                        return new Response
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status204NoContent,
                            ErrorMessage = "Token is null"
                        };
                    }
                    else
                    {
                        _logger.LogWarning("Error in Login method, response status code is not success");
                        return new Response
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = "Response status code is not success"
                        };
                    }
                }
                return new Response()
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = "Model is not valid"
                };

            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Login method: {e.Message}");
                return new Response()
                {
                    IsSuccess = false,
                    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                };
            }
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