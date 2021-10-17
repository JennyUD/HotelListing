using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _Context;
        private readonly DbSet<T> _db;

        public GenericRepository(DatabaseContext Context)
        {
            _Context = Context;
            _db = _Context.Set<T>();
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> Query = _db;
            if (includes != null)
            {
                foreach (var IncludeProperty in includes)
                {
                    Query = Query.Include(IncludeProperty);
                }
            }

            return await Query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> Query = _db;

            if (expression != null)
            {
                Query = Query.Where(expression);//where expression is true
            }
            if (includes != null)
            {
                foreach (var IncludeProperty in includes)
                {
                    Query = Query.Include(IncludeProperty);
                }
            }
            if (orderBy != null)
            {
                Query = orderBy(Query);
            }

            return await Query.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        public  void Update(T entity)
        {
            _db.Attach(entity);
            _Context.Entry(entity).State= EntityState.Modified;        }
    }

}
