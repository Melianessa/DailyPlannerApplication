using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Models;
using DailyPlanner.Helpers;
using DailyPlanner.Identity.Controllers;
using DailyPlanner.Web.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DailyPlanner.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly APIHelper _userAPI;
        private readonly ILogger _logger;

        public UserController(ILogger<UserController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _userAPI = new APIHelper(configuration);
        }
        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns>A list of users</returns>
        [HttpGet]
        [ServiceFilter(typeof(DailyPlannerExceptionFilterAttribute))]
        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                HttpResponseMessage res = await client.GetAsync("api/user/getAllUsers");
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<List<UserDTO>>(result).OrderByDescending(p => p.CreationDate).ToList();
                    return user;
                }
                else
                {
                    _logger.LogWarning("Error in Get all users method, response status code is not success");
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in GetAllUsers method: {e.Message}");
                return null;
            }
        }
        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns>A list of users</returns>
        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                HttpClient client = _userAPI.InitializeClient();
                HttpResponseMessage res = await client.GetAsync("api/user/getAll");
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<List<User>>(result).OrderBy(p => p.CreationDate).ToList();
                    return user;
                }
                else
                {
                    _logger.LogWarning("Error in Get all users method, response status code is not success");
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in GetAll method: {e.Message}");
                return null;
            }
        }
        /// <summary>
        /// Get a specific User.
        /// </summary>
        /// <param name="id">The user id to search for</param>
        /// <returns>A user information</returns>
        [HttpGet("{id}")]
        public async Task<User> Get(Guid id)
        {
            try
            {
                HttpClient client = _userAPI.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/user/get/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<User>(result);
                    if (user == null)
                    {
                        _logger.LogWarning("Error in Get method, user is NULL");
                        return null;
                    }
                    return user;
                }
                else
                {
                    _logger.LogWarning("Error in Get user method, response status code is not success");
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Get method: {e.Message}");
                return null;
            }
        }
        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "firstName": "Dan",
        ///        "lastName": "Reynolds",
        ///        "creationDate": "4/24/2019 1:00:00 PM",
        ///        "dateOfBirth": "4/24/1996 12:00:00 PM",
        ///        "phone": 5556677,
        ///        "email": "user@mail.com",
        ///        "sex": true,
        ///        "isActive": true,
        ///        "role": 1,
        ///        "eventCount": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="user">A user to create</param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If the user is null</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<Response> Create([FromBody]User user)
        {
            try
            {
                HttpClient client = _userAPI.InitializeClient();
                if (ModelState.IsValid)
                {
                    var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                        "application/json");
                    HttpResponseMessage res = await client.PostAsync("api/user/post", content);
                    if (res.IsSuccessStatusCode)
                    {
                        return new Response
                        {
                            IsSuccess = true,
                            StatusCode = StatusCodes.Status200OK,
                        };
                    }
                    else
                    {
                        _logger.LogWarning("Error in Create user method, response status code is not success");
                        return new Response
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status400BadRequest,
                            ErrorMessage = "Response status code is not success"
                        };
                    }
                }
                else
                {
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ErrorMessage = "Model is not valid"
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Create method: {e.Message}");
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                };
            }
        }
        /// <summary>
        /// Edit a specific User.
        /// </summary>
        /// <param name="id">The user id to edit for</param>
        [HttpGet("{id}")]
        public async Task<UserDTO> Edit(Guid id)
        {
            try
            {
                HttpClient client = _userAPI.InitializeClient();
                HttpResponseMessage res = await client.GetAsync($"api/user/getUser/{id}");

                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserDTO>(result);
                    return user;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Edit method: {e.Message}");
                return null;
            }
        }
        /// <summary>
        /// Edit a specific User.
        /// </summary>
        /// <param name="user">The user to edit for</param>
        [HttpPut("{id}")]
        public async Task<User> Edit([FromBody]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient client = _userAPI.InitializeClient();

                    var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PutAsync($"api/user/put/{user.Id}", content);
                    if (res.IsSuccessStatusCode)
                    {
                        var result = await res.Content.ReadAsStringAsync();
                        var newUser = JsonConvert.DeserializeObject<User>(result);
                        return newUser;
                    }
                    else
                    {
                        _logger.LogWarning("Error in Edit user method, response status code is not success");
                        return null;
                    }
                }
                else
                {
                    _logger.LogWarning("Error in Edit user method, model is not valid");
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Edit user method: {e.Message}");
                return null;
            }
        }
        /// <summary>
        /// Deletes a specific User.
        /// </summary>
        /// <param name="id">The user id to delete for</param>
        [HttpDelete("{id}")]
        public async Task<Response> Delete(Guid id)
        {
            try
            {
                HttpClient client = _userAPI.InitializeClient();
                HttpResponseMessage res = await client.DeleteAsync($"api/user/delete/{id}");
                if (res.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    _logger.LogWarning("Error in Delete user method, response status code is not success");
                    return new Response
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        ErrorMessage = "Response status code is not success"
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in Delete user method: {e.Message}");
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                };
            }
        }
    }
}
