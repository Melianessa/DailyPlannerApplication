using System;
using System.Collections.Generic;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Interfaces;
using DailyPlanner.DomainClasses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers
{
    
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

        [Route("api/[controller]/[action]")]
        [HttpPost]
        public IEnumerable<EventDTO> GetByDate([FromBody] string date)
        {
            return _iEventRepo.GetByDate(date);
        }
        [Route("api/[controller]")]
        [HttpGet]
        public IEnumerable<Event> GetAll()
        {
            return _iRepo.GetAll();
        }

        [Route("api/[controller]/[action]/{id}")]
        [HttpGet("{id}")]
        public Event Get(Guid id)
        {

            return _iRepo.Get(id);
        }
        [Route("api/[controller]")]
        [HttpPost]
        public Guid Post([FromBody] EventDTO ev)
        {
            return _iEventRepo.Add(ev);
        }
        [Route("api/[controller]/{id}")]
        [HttpPut("{id}")]
        public Event Put([FromBody] Event ev)
        {
            return _iRepo.Update(ev);
        }
        [Route("api/[controller]/{id}")]
        [HttpDelete("{id}")]
        public void Delete(Event ev)
        {
            _iRepo.Delete(ev);
        }
    }
}