using Microsoft.EntityFrameworkCore.Storage;
using SchoolV01.Application.Requests;
using SchoolV01.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Repositories
{
    public interface IUnitOfWork<TId> : IDisposable
    {
        IRepositoryAsync<T, TId> Repository<T>() where T : AuditableEntity<TId>;

        Task<int> Commit(CancellationToken cancellationToken);
        Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, string cacheKeyContain);

        Task Rollback();

        T Add<T>(T obj) where T : class;
        void Update<T>(T obj) where T : class;
        void Remove<T>(T obj) where T : class;
        void RemoveRange<T>(IEnumerable<T> obj) where T : class;
        IQueryable<T> Query<T>() where T : class;
        void Commit();
        Task CommitAsync();

        void Attach<T>(T obj) where T : class;
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
        Task CommitTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        T Map<T>(object obj) where T : class;
        string AddAttachment(UploadRequest uploadRequest);
        ClaimsPrincipal CurrentUser { get; }
    }
}