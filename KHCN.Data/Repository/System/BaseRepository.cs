using KHCN.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public abstract class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<T> GetAll()
        {
            return _unitOfWork.Context.Set<T>();
        }

        public virtual async Task<ICollection<T>> GetAllAsyn()
        {
            return await _unitOfWork.Context.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetPaging(int? page, int? pageSize)
        {
            page = page == null ? 1 : page;
            pageSize = pageSize == null ? 10 : pageSize;

            return _unitOfWork.Context.Set<T>().Take(pageSize.Value).Skip((page.Value - 1) * pageSize.Value);
        }

        public virtual T Get(int id)
        {
            return _unitOfWork.Context.Set<T>().Find(id);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public virtual T Add(T t)
        {
            _unitOfWork.Context.Set<T>().Add(t);
            _unitOfWork.Context.SaveChanges();
            return t;
        }

        public virtual async Task<T> AddAsync(T t)
        {
            await _unitOfWork.Context.Set<T>().AddAsync(t);
            await _unitOfWork.Context.SaveChangesAsync();
            return t;
        }
        public virtual List<T> Add(List<T> entities)
        {
            _unitOfWork.Context.Set<T>().AddRange(entities);
            _unitOfWork.Context.SaveChanges();
            return entities;
        }

        public virtual async Task<List<T>> AddAsync(List<T> entities)
        {
            await _unitOfWork.Context.Set<T>().AddRangeAsync(entities);
            await _unitOfWork.Context.SaveChangesAsync();
            return entities;
        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return _unitOfWork.Context.Set<T>().SingleOrDefault(match);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _unitOfWork.Context.Set<T>().SingleOrDefaultAsync(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _unitOfWork.Context.Set<T>().Where(match).ToList();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _unitOfWork.Context.Set<T>().Where(match).ToListAsync();
        }

        public virtual void Delete(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
            _unitOfWork.Context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            _unitOfWork.Context.Set<T>().Remove(entity);
            return await _unitOfWork.Context.SaveChangesAsync();
        }

        public virtual void Delete(List<T> entities)
        {
            _unitOfWork.Context.Set<T>().RemoveRange(entities);
            _unitOfWork.Context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(List<T> entities)
        {
            _unitOfWork.Context.Set<T>().RemoveRange(entities);
            return await _unitOfWork.Context.SaveChangesAsync();
        }

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;
            T exist = _unitOfWork.Context.Set<T>().Find(key);
            if (exist != null)
            {
                _unitOfWork.Context.Entry(exist).CurrentValues.SetValues(t);
                _unitOfWork.Context.SaveChanges();
            }
            return exist;
        }

        public virtual async Task<T> UpdateAsync(T t, object key)
        {
            if (t == null)
                return null;
            T exist = await _unitOfWork.Context.Set<T>().FindAsync(key);
            if (exist != null)
            {
                _unitOfWork.Context.Entry(exist).CurrentValues.SetValues(t);
                await _unitOfWork.Context.SaveChangesAsync();
            }
            return exist;
        }

        public int Count()
        {
            return _unitOfWork.Context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _unitOfWork.Context.Set<T>().CountAsync();
        }

        public virtual void Save()
        {
            _unitOfWork.Context.SaveChanges();
        }

        public async virtual Task<int> SaveAsync()
        {
            return await _unitOfWork.Context.SaveChangesAsync();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _unitOfWork.Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
        {
            return await _unitOfWork.Context.Set<T>().Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _unitOfWork.Context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}