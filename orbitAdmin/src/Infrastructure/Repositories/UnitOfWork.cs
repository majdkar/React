using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Contracts;
using SchoolV01.Infrastructure.Contexts;
using LazyCache;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SchoolV01.Application.Requests;
using SchoolV01.Application.Extensions;
using System.Security.Claims;
using Hangfire.Logging;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace SchoolV01.Infrastructure.Repositories
{
    public class UnitOfWork<TId>(BlazorHeroContext dbContext, ICurrentUserService currentUserService, IAppCache cache, ILogger<UnitOfWork<TId>> logger, IMapper mapper, IUploadService uploadService)
        : IUnitOfWork<TId>
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly BlazorHeroContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private bool disposed;
        private Hashtable _repositories;
        private readonly IAppCache _cache = cache;
        private readonly ILogger<UnitOfWork<TId>> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IUploadService _uploadService = uploadService;
        private IDbContextTransaction _currentTransaction;

        public IRepositoryAsync<TEntity, TId> Repository<TEntity>() where TEntity : AuditableEntity<TId>
        {
            if (_repositories == null)
                _repositories = [];

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryAsync<,>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepositoryAsync<TEntity, TId>)_repositories[type];
        }
        public TEntity Add<TEntity>(TEntity obj) where TEntity : class
        {
            var set = _dbContext.Set<TEntity>();
            var result = set.Add(obj);
            return result.Entity;
        }
        public void Update<T>(T obj) where T : class
        {
            var set = _dbContext.Set<T>();
            set.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
        }
        void IUnitOfWork<TId>.Remove<T>(T obj)
        {
            var set = _dbContext.Set<T>();
            set.Remove(obj);
        }
        void IUnitOfWork<TId>.RemoveRange<T>(IEnumerable<T> obj)
        {
            var set = _dbContext.Set<T>();
            set.RemoveRange(obj);
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Attach<T>(T newUser) where T : class
        {
            var set = _dbContext.Set<T>();
            set.Attach(newUser);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }


        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
        {
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            foreach (var cacheKey in cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
            return result;
        }
        public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, string cacheKeyContain)
        {
            var result = 0;
            try
            {
                result = await _dbContext.SaveChangesAsync(cancellationToken);

                var cacheKeys = _cache.GetKeys(cacheKeyContain);

                foreach (var cacheKey in cacheKeys)
                {
                    _cache.Remove(cacheKey);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during CommitAndRemoveCache execution. Exception Details: {Exception}", ex.ToString());
            }
            return result;
        }



        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public T Map<T>(object obj) where T : class
        {
            return _mapper.Map<T>(obj);
        }
        public string AddAttachment(UploadRequest uploadRequest)
        {
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{new Random(9)}{uploadRequest.Extension}";
                return _uploadService.UploadAsync(uploadRequest);
            }
            return null;
        }


        public ClaimsPrincipal CurrentUser => _currentUserService.User;
    }
}