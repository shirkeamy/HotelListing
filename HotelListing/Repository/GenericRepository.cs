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
        private readonly HotelListingContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(HotelListingContext context)
        {
            _context = context;
            _db = _context.Set<T>();
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

        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToArrayAsync();
        }

        public async Task Insert(T entiry)
        {
            await _db.AddAsync(entiry);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);

        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
