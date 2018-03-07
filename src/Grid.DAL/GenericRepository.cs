using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Grid.Data;
using Grid.DAL.Interfaces;
using Grid.Entities;
using PagedList;
using Grid.Features.Common;

namespace Grid.DAL
{
    public abstract class GenericRepository<T> : IDisposable, IGenericRepository<T> where T : EntityBase
    {
        protected IDbContext _context;
        private readonly IDbSet<T> _dbset;

        protected GenericRepository(IDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        private IQueryable<T> IncludeProperties(IQueryable<T> query, string includeProperties)
        {
            foreach (var includeProperty in includeProperties.Split
               (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable();
        }

        public IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            var query = _dbset.AsQueryable();
            return orderBy(query);
        }

        public IEnumerable<T> GetAll(string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            return query.AsEnumerable();
        }

        public IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            return orderBy(query);
        }

        public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter)
        {
            return _dbset.Where(filter);
        }

        public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter, string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            return query.Where(filter);
        }

        public IEnumerable<T> GetAllBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            var query = _dbset.AsQueryable();
            return orderBy(query);
        }

        public IEnumerable<T> GetAllBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            return orderBy(query);
        }

        public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            var query = _dbset.AsQueryable();
            query = query.Where(filter);
            return orderBy(query);
        }

        public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            query = query.Where(filter);
            return orderBy(query);
        }

        public T Get(int id)
        {
            return _dbset.Find(id);
        }

        public T Get(int id, string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            return query.FirstOrDefault(t => t.Id == id);
        }

        public T GetBy(Expression<Func<T, bool>> predicate)
        {
            return _dbset.FirstOrDefault(predicate);
        }

        public T GetBy(Expression<Func<T, bool>> predicate, string includeProperties)
        {
            var query = _dbset.AsQueryable();
            query = IncludeProperties(query, includeProperties);
            return query.FirstOrDefault(predicate);
        }


        public T Create(T entity)
        {
            return _dbset.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public void Delete(int id)
        {
            var entityToDelete = _dbset.Find(id);
            Delete(entityToDelete);
        }

        public int Count()
        {
            return _dbset.Count();
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            return _dbset.Count(filter);
        }

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return _dbset.Any(filter);
        }

        public IEnumerable<T> Search(Func<IQueryable<T>, IQueryable<T>> transform)
        {
            var query = _dbset.AsQueryable();
            query = transform(query);
            return query.ToList();
        }

        public IPagedList<T> SearchPage(Func<IQueryable<T>, IQueryable<T>> transform, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNo, int pageSize)
        {
            var query = _dbset.AsQueryable();
            query = transform(query);
            var ordered = orderBy(query);
            return ordered.ToPagedList(pageNo, pageSize);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_context == null)
            {
                return;
            }

            _context.Dispose();
            _context = null;
        }
    }
}
