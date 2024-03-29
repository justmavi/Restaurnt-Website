﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Restaurant_Website.Domain.Interfaces;
using System.Threading.Tasks;

namespace Restaurant_Website.Infrastructure.Data.Implementation
{
    public class BaseRepository<T> : IRepositoty<T> where T : class
    {
        protected readonly DbSet<T> Entities;
        private readonly DbContext context;

        public BaseRepository(DbContext context)
        {
            this.context = context;
            Entities = context.Set<T>();
        }
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await Entities.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<T> query = Entities;

            if (!(filter is null))
            {
                query = query.Where(filter);
            }
            
            if (!(includeProperties is null))
            {
                foreach (var includeProperty in includeProperties.Split
                    (',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (!(orderBy is null))
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual void Insert(T item) => Entities.Add(item);
        public virtual async Task InsertAsync(T item) => await Entities.AddAsync(item);

        public virtual void Delete(T item) => Entities.Remove(item);
        public virtual async void Delete(object id)
        {
            T removedItem = await GetByIdAsync(id);
            Delete(removedItem);
        }
        public virtual void Update(T item)
        {
            Entities.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public virtual void Dispose()
        {
            // TODO: 
        }
    }
}
