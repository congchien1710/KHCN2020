using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KHCN.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        List<T> Add(List<T> entities);
        Task<List<T>> AddAsync(List<T> entities);
        int Count();
        Task<int> CountAsync();
        void Delete(T entity);
        Task<int> DeleteAsync(T entity);
        void Delete(List<T> entities);
        Task<int> DeleteAsync(List<T> entities);
        void Dispose();
        T Find(Expression<Func<T, bool>> match);
        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);
        T Get(int id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsyn();
        IQueryable<T> GetPaging(int? page, int? pageSize);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetAsync(int id);
        void Save();
        Task<int> SaveAsync();
        T Update(T entity, object key);
        Task<T> UpdateAsync(T entity, object key);
    }
}
