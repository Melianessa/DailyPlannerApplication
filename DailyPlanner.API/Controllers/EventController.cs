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
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IDataRepository<Event> _iRepo;
        private readonly IEventBase _iEventRepo;

        public EventController(IDataRepository<Event> repo, IEventBase evRepo)
        {
            _iRepo = repo;
            _iEventRepo = evRepo;
        }
                
        [HttpPost("[action]")]
        public IEnumerable<EventDTO> GetByDate([FromBody] string date)
        {
            return _iEventRepo.GetByDate(date);
        }
        [HttpGet("{id}")]
        public Event Get(Guid id)
        {

            return _iRepo.Get(id);
        }
        [HttpPost]
        public Guid Post([FromBody] EventDTO ev)
        {
            return _iEventRepo.Add(ev);
        }
        [HttpPut("{id}")]
        public Event Put([FromBody] Event ev)
        {
            return _iRepo.Update(ev);
        }
        [HttpDelete("{id}")]
        public void Delete(Event ev)
        {
            _iRepo.Delete(ev);
        }
    }
}