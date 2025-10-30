using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Contracts;
using SchoolV01.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using Microsoft.EntityFrameworkCore.Query;

namespace SchoolV01.Infrastructure.Repositories
{
    public class RepositoryAsync<T, TId>(BlazorHeroContext dbContext) : IRepositoryAsync<T, TId> where T : AuditableEntity<TId>
    {
        private readonly BlazorHeroContext _dbContext = dbContext;

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task SoftDeleteAsync(T entity)
        {
            entity.Deleted = true;
            _dbContext.Set<T>().Update(entity);
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }
       public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await Task.CompletedTask;
        }
        public async Task ExecuteDeleteAsync(TId id)
        {
            await _dbContext.Set<T>().Where(x => x.Id.Equals(id)).ExecuteDeleteAsync();
            //await _dbContext.Set<T>()
            //    .Where(x => x.Id.Equals(id))
            //    .ExecuteUpdateAsync(setter => setter.SetProperty(s => s.Deleted, true));
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .OrderByDescending(x => x.Id)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(TId id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            T exist = await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return;
        }
        public async Task<int> ExecuteUpdateAsync(TId id, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            return await _dbContext.Set<T>().Where(x => x.Id.Equals(id)).ExecuteUpdateAsync(setPropertyCalls);
        }

        public Task UpdateRangeAsync(List<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            return Task.CompletedTask;
        }
    }
}