using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Grid.Features.Common
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IEnumerable<T> GetAll(string includeProperties);
        IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties);

        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter, string includeProperties);
        IEnumerable<T> GetAllBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IEnumerable<T> GetAllBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties);
        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IEnumerable<T> GetAllBy(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties);

        T Get(int id);
        T Get(int id, string includeProperties);
        T GetBy(Expression<Func<T, bool>> predicate);
        T GetBy(Expression<Func<T, bool>> predicate, string includeProperties);

        T Create(T entity);
        void Update(T entity);
        T Delete(T entity);
        void Delete(int id);

        int Count();
        int Count(Expression<Func<T, bool>> filter);

        bool Any(Expression<Func<T, bool>> filter);

        IEnumerable<T> Search(Func<IQueryable<T>, IQueryable<T>> transform);
        IPagedList<T> SearchPage(Func<IQueryable<T>, IQueryable<T>> transform, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int pageNo, int pageSize);
    }
}
