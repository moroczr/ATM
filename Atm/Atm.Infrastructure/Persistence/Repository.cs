namespace Atm.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Atm.Application.Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Generic repository for CRUD operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Atm.Application.Interfaces.IRepository&lt;T&gt;" />
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AtmDbContext ctx;

        public Repository(AtmDbContext ctx)
        {
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public T Add(T item)
        {
            return ctx.Add<T>(item).Entity;
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items.Any())
            {
                ctx.Set<T>().AddRange(items);
            }
        }

        public T Delete(T item)
        {
            ctx.Remove<T>(item);

            return item;
        }

        public IEnumerable<T> DeleteRange(IEnumerable<T> items)
        {
            if (items.Any())
            {
                ctx.Set<T>().RemoveRange(items);
            }

            return items;
        }

        public IQueryable<T> Read()
        {
            return ctx.Set<T>();
        }

        public Task<List<T>> ReadAsListAsync()
        {
            return ctx.Set<T>().ToListAsync();
        }

        public T Update(T item)
        {
            ctx.Set<T>().Attach(item);
            ctx.Entry(item).State = EntityState.Modified;

            return item;
        }
    }
}