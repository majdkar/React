using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchoolV01.Domain.Contracts;
using Microsoft.EntityFrameworkCore.Query;

namespace SchoolV01.Application.Interfaces.Repositories
{
    public interface IRepositoryAsync<T, in TId> where T : class, IEntity<TId>
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(TId id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize);

        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entities);

        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(List<T> entity);
        Task<int> ExecuteUpdateAsync(TId id, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);

        Task SoftDeleteAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task ExecuteDeleteAsync(TId id);

    }
}