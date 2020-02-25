using System;
using System.Collections.Generic;
using System.Text;
using DailyPlanner.DomainClasses.Enums;
using DailyPlanner.DomainClasses.Models;

namespace DailyPlanner.DomainClasses
{
    public class EventDTO
    {
        public EventDTO(Event ev)
        {
            Id = ev.Id;
            CreationDate = ev.CreationDate;
            Description = ev.Description;
            EndDate = ev.EndDate;
            IsActive = ev.IsActive;
            StartDate = ev.StartDate;
            Title = ev.Title;
            Type = ev.Type;
            UserId = ev.User.Id;

        }
        public EventDTO() { }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public EventEnum Type { get; set; }
        public Guid UserId { get; set; }
    }
}
