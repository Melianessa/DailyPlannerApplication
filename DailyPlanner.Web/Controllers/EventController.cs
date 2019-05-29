using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Models;
using DailyPlanner.Helpers;
using DailyPlanner.Identity.Controllers;
using DailyPlanner.Web.Filters;
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
    [DailyPlannerExceptionFilter]
    public class EventController : Controller
    {
        APIHelper _userAPI;
        private readonly ILogger _logger;

        public EventController(ILogger<EventController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _userAPI = new APIHelper(configuration);
        }
        /// <summary>
        /// Get all Events to date.
        /// </summary>
        /// <param name="date">The event date to search for</param>
        /// <returns>A list of events</returns>
        [HttpPost]
        public async Task<IEnumerable<EventDTO>> GetByDate(string date) //try [FromBody]
        {
            List<EventDTO> eventList = new List<EventDTO>();
            try
            {
                var id = User.GetId();
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                var content = new StringContent(JsonConvert.SerializeObject(date), Encoding.UTF8, "application/json");
                // or JsonConvert.SerializeObject(date)
                HttpResponseMessage res = await client.PostAsync("api/event/getByDate", content);
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<EventDTO>>(result).Where(p => p.UserId == id).OrderBy(p => p.StartDate).ToList();
                    return eventList;
                }
                else
                {
                    _logger.LogWarning("Error in GetByDate method, response status code is not success");
                    return eventList;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in GetByDate method: {e.Message}");
                return eventList;
            }

        }
        /// <summary>
        /// Get all Events.
        /// </summary>
        /// <returns>A list of events</returns>
        [HttpGet]
        public async Task<IEnumerable<Event>> GetAll()
        {
            List<Event> eventList = new List<Event>();
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                HttpResponseMessage res = await client.GetAsync("api/event");
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    eventList = JsonConvert.DeserializeObject<List<Event>>(result).OrderBy(p => p.StartDate).ToList();
                    return eventList;
                }
                else
                {
                    _logger.LogWarning("Error in GetAll method, response status code is not success");
                    return eventList;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Error in GetAll method: {e.Message}");
                return eventList;
            }
        }
        /// <summary>
        /// Get a specific Event.
        /// </summary>
        /// <param name="id">The event id to search for</param>
        /// <returns>An event information</returns>
        [HttpGet("{id}")]
        public async Task<Event> Get(Guid id)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                HttpResponseMessage res = await client.GetAsync($"api/event/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    var ev = JsonConvert.DeserializeObject<List<Event>>(result).SingleOrDefault(m => m.Id == id);
                    if (ev == null)
                    {
                        _logger.LogWarning("Error in Get method, event is NULL");
                    }
                    return ev;
                }
                else
                {
                    _logger.LogWarning("Error in GetAll method, response status code is not success");
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
        /// Creates a Event.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///     {
        ///        "id": "1",
        ///        "title": "birthday",
        ///        "description": "my birthday",
        ///        "startDate": "4/24/2019 1:00:00 PM",
        ///        "endDate": "4/25/2019 1:00:00 PM",
        ///        "creationDate": "4/24/2019 1:00:00 PM",
        ///        "isActive": true,
        ///        "type": 0,
        ///        "user": {
        ///            "id": "2",
        ///            "firstName": "Dan",
        ///            "lastName": "Reynolds",
        ///            "creationDate": "4/24/2019 1:00:00 PM",
        ///            "dateOfBirth": "7/14/1987 12:00:00 PM",
        ///            "phone": "5556677",
        ///            "email": "user@mail.com",
        ///            "sex": true,
        ///            "isActive": true,
        ///            "role": 0,
        ///            "events": [
        ///               null
        ///            ]
        ///        }
        ///     }
        ///
        /// </remarks>
        /// <param name="ev">An event to create</param>
        /// <returns>A newly created Event</returns>
        /// <response code="201">Returns the newly created event</response>
        /// <response code="400">If the event is null</response>
        
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public async Task<Response> Create([FromBody]EventDTO ev)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string token = HttpContext.Request.Headers["Authorization"];
                    HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                    var authId = User.GetId();
                    ev.UserId = authId;
                    var content = new StringContent(JsonConvert.SerializeObject(ev), Encoding.UTF8,
                        "application/json");
                    HttpResponseMessage res = await client.PostAsync("api/event", content);
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
                        _logger.LogWarning("Error in Create event method, response status code is not success");
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
                _logger.LogWarning($"Error in Create method: {e.Message}");
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                };
            }
        }
        /// <summary>
        /// Edit a specific Event.
        /// </summary>
        /// <param name="id">The event id to edit for</param>
        [HttpGet("{id}")]
        public async Task<Event> Edit(Guid id)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                HttpResponseMessage res = await client.GetAsync($"api/event/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsStringAsync();
                    var ev = JsonConvert.DeserializeObject<Event>(result);
                    if (ev == null)
                    {
                        _logger.LogWarning("Error in Edit method, event is NULL");
                    }
                    return ev;
                }
                else
                {
                    _logger.LogWarning("Error in Edit event method, response status code is not success");
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
        /// Edit a specific Event.
        /// </summary>
        /// <param name="ev">The event to edit for</param>
        [HttpPut("{id}")]
        public async Task<Event> Edit([FromBody]Event ev)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string token = HttpContext.Request.Headers["Authorization"];
                    HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                    var content = new StringContent(JsonConvert.SerializeObject(ev), Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await client.PutAsync($"api/event/{ev.Id}", content);
                    if (res.IsSuccessStatusCode)
                    {
                        var result = await res.Content.ReadAsStringAsync();
                        var newEvent = JsonConvert.DeserializeObject<Event>(result);
                        return newEvent;
                    }
                    else
                    {
                        _logger.LogWarning("Error in Edit event method, response status code is not success");
                        return null;
                    }
                }
                else
                {
                    _logger.LogWarning("Error in Edit event method, model is not valid");
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
        /// Deletes a specific Event.
        /// </summary>
        /// <param name="id">The event id to delete for</param>
        [HttpDelete("{id}")]
        public async Task<Response> Delete(Guid id)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                HttpClient client = _userAPI.InitializeClient(token?.ToReadableToken());
                HttpResponseMessage res = await client.DeleteAsync($"api/event/{id}");

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
                    _logger.LogWarning("Error in Delete event method, response status code is not success");
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
                _logger.LogWarning($"Error in Delete method: {e.Message}");
                return new Response
                {
                    IsSuccess = false,
                    ErrorMessage = $"{e.Message}, {e.InnerException.Message}"
                };
            }
        }
    }
}