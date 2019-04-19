﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TrainSchdule.DAL.Interfaces
{
    /// <summary>
    /// Interface for DB repositories.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Method for fetching all data from table.
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Method for fetching all data from table with paggination.
        /// </summary>
        IEnumerable<T> GetAll(int page, int pageSize);

        /// <summary>
        /// Method for fetching entity by id (primary key).
        /// </summary>
        T Get(Guid id);

        /// <summary>
        /// Async method for fetching entity by id (primary key).
        /// </summary>
        Task<T> GetAsync(Guid id);

        /// <summary>
        /// Method for fetching entity(ies) by predicate.
        /// </summary>
        IQueryable<T> Find(Expression<Func<T, bool> > predicate);

        /// <summary>
        /// Method for creating entity.
        /// </summary>
        void Create(T item);

        /// <summary>
        /// Async method for creating entity.
        /// </summary>
        Task CreateAsync(T item);

        /// <summary>
        /// Method for updating entity.
        /// </summary>
        void Update(T item);

        /// <summary>
        /// Method for deleting entity.
        /// </summary>
        void Delete(Guid id);

        /// <summary>
        /// Async method for deleting entity.
        /// </summary>
        Task DeleteAsync(Guid id);
    }
}
