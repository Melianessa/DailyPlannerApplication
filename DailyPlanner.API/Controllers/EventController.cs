using System;
using System.Collections.Generic;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Interfaces;
using DailyPlanner.DomainClasses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IDataRepository<Event> _iRepo;
        private readonly IEventBase<EventDTO> _iEventRepo;

        public EventController(IDataRepository<Event> repo, IEventBase<EventDTO> evRepo)
        {
            _iRepo = repo;
            _iEventRepo = evRepo;
        }

        [HttpPost]
        public IEnumerable<EventDTO> GetByDate([FromBody] string date)
        {
            return _iEventRepo.GetByDate(date);
        }

        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            return _iRepo.GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Event Get(Guid id)
        {

            return _iRepo.Get(id);
        }

        // POST api/values
        [HttpPost]
        public Guid Post([FromBody] EventDTO ev)
        {
            return _iEventRepo.Add(ev);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public Event Put([FromBody] Event ev)
        {
            return _iRepo.Update(ev);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Event ev)
        {
            _iRepo.Delete(ev);
        }
    }
}