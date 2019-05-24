using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DailyPlanner.Helpers;
using DailyPlanner.Identity.Models;
using DailyPlanner.Identity.Controllers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailyPlanner.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        IdentityHelper _apiBaseURI;
        private readonly ILogger _logger;
        
        public AccountController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiBaseURI = new IdentityHelper(configuration);
            

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
                                Token = response["access_token"],
                                RefreshToken = response["refresh_token"],
                                ExpirationTime = response["expires_in"]
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

        [HttpGet]
        public async void Logout()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _apiBaseURI.InitializeClient(token?.ToReadableToken());
                var id = User.GetClientId();
                
                await HttpContext.SignOutAsync("Cookies");
                await HttpContext.SignOutAsync("oidc");
                await HttpContext.SignOutAsync("Bearer");
                //string token = HttpContext.Request.Headers["Authorization"];
                //HttpClient client = _apiBaseURI.InitializeClient(token?.ToReadableToken());
                //string idTokenRaw = HttpContext.Request.Headers["Authorization"];
                //var idTokenHint = idTokenRaw.ToReadableToken();
                //var postLogoutRedirectUri = "http://localhost:50472";
                ////var req = new HttpRequestMessage(HttpMethod.Get, $"connect/endsession?id_token_hint={idTokenHint}&post_logout_redirect_uri={postLogoutRedirectUri}");
                ////var res = await client.SendAsync(req);
                //var len =
                //    $"http://localhost:5000/connect/endsession?id_token_hint={idTokenHint}&post_logout_redirect_uri={postLogoutRedirectUri}";
                //var postLogoutRedirectUriNew = System.Web.HttpUtility.UrlEncode(postLogoutRedirectUri);
                //HttpResponseMessage res = await client.GetAsync($"connect/endsession?id_token_hint={idTokenHint}&post_logout_redirect_uri={postLogoutRedirectUriNew}");
                //if (res.IsSuccessStatusCode)
                //{
                //    return new Response
                //    {
                //        IsSuccess = true,
                //        StatusCode = StatusCodes.Status200OK
                //    };
                //}
                //else
                //{
                //    _logger.LogWarning("Error in Logout method, response status code is not success");
                //    return new Response
                //    {
                //        IsSuccess = false,
                //        StatusCode = StatusCodes.Status400BadRequest,
                //        ErrorMessage = "Response status code is not success"
                //    };
                //}
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Logout method: {e.Message}");
                //return new Response()
                //{
                //    IsSuccess = false,
                //    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                //};
            }
        }
    }
}