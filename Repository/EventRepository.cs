using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Interfaces;
using DailyPlanner.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.Repository
{
    public class EventRepository : IEventBase
    {
        private readonly PlannerDbContext _context;

        public EventRepository(PlannerDbContext context)
        {
            _context = context;
        }
        public IEnumerable<EventDTO> GetByDate(string date)
        {
            var d = DateTime.Parse(date, CultureInfo.InvariantCulture);
            return _context.Events.Include(p => p.User).Where(p => p.StartDate.Date == d.Date && !p.IsDeleted).Select(p => new EventDTO(p)).ToList();
        }

        public Event Get(Guid id)
        {
            var ev = _context.Events.FirstOrDefault(u => u.Id == id);
            return ev;
        }

        public Guid Add(EventDTO ev)
        {
            var evt = new Event
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                IsActive = true,
                Title = ev.Title,
                Type = ev.Type,
                Description = ev.Description,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
            };
            var user = new User
            {
                Id = ev.UserId
            };
            _context.Users.Attach(user);
            evt.User = user;
            return Add(evt);
        }

        public Guid Add(Event ev)
        {
            _context.Events.Add(ev);
            _context.SaveChanges();
            return ev.Id;
        }

        public Event Update(Event b)
        {
            var ev = _context.Events.Find(b.Id);
            if (ev != null)
            {
                ev.Title = b.Title;
                ev.Description = b.Description;
                ev.Type = b.Type;
                ev.StartDate = b.StartDate;
                ev.EndDate = b.EndDate;
                ev.IsActive = b.IsActive;
            }
            _context.SaveChanges();
            return b;
        }

        public void Delete(Event b)
        {
            var ev = _context.Events.Find(b.Id);
            if (ev != null)
            {
                ev.IsDeleted = true;
            }
            _context.SaveChanges();
        }
    }
}
