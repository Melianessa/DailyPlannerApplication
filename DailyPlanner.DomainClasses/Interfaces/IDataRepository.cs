﻿using System;
using System.Collections.Generic;

namespace DailyPlanner.DomainClasses.Interfaces
{
    public interface IDataRepository<TEntity> where TEntity : class
    {
        TEntity Get(Guid id);
        Guid Add(TEntity b);
        TEntity Update(TEntity b);
        void Delete(TEntity b);
    }
}
