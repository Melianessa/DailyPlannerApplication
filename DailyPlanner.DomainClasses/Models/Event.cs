using System;
using System.ComponentModel.DataAnnotations;
using DailyPlanner.DomainClasses.Enums;
using DailyPlanner.DomainClasses.Interfaces;

namespace DailyPlanner.DomainClasses.Models
{
    public class Event : IBase
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public EventEnum Type { get; set; }
        public User User { get; set; }
    }
}
