using DailyPlanner.DomainClasses.Models;
using System;
using System.Collections.Generic;

namespace DailyPlanner.DomainClasses.Interfaces
{
    public interface IEventBase: IDataRepository<Event>
    {
        IEnumerable<EventDTO> GetByDate(string date);
        Guid Add(EventDTO ev);

    }
}
