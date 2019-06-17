namespace Libria.Repository.EFCore.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Contracts.Repository;
	using Core.Specification;
	using Microsoft.EntityFrameworkCore;
	using Specifications;

	public abstract class EfCoreRepository<TEntity> : IEfCoreRepository<TEntity> where TEntity : class
	{
		protected readonly DbContext DbContext;
		protected readonly DbSet<TEntity> DbSet;
		
		protected EfCoreRepository(DbContext dbContext)
		{
			DbContext = dbContext;
			DbSet = DbContext.Set<TEntity>();
		}

		public async Task<TEntity> GetByIdAsync(params object[] keyValues)
		{
			return await GetByIdAsync(keyValues, default(CancellationToken));
		}

		public Task<TEntity> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(GetById(keyValues));
		}

		public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(Add(entity));
		}

		public Task<TEntity> UpdateAsync(TEntity entity,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(Update(entity));
		}

		public Task AddRangeAsync(IEnumerable<TEntity> entities,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			AddRange(entities);

			return Task.CompletedTask;
		}

		public Task<TEntity> RemoveAsync(TEntity entity,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(Remove(entity));
		}

		public Task RemoveRangeAsync(IEnumerable<TEntity> entities,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			RemoveRange(entities);

			return Task.CompletedTask;
		}

		public async Task<int> CountAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = GetQueryFromSpecification(specification);

			return await query
				.CountAsync(cancellationToken);
		}

		public async Task<TEntity> FindAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = GetQueryFromSpecification(specification);

			return await query
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IEnumerable<TEntity>> FindAllAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = GetQueryFromSpecification(specification);

			return await query
				.ToListAsync(cancellationToken);
		}

		public TEntity GetById(params object[] keyValues)
		{
			var entity = GetBaseQuery(DbSet)
				.FirstOrDefault(e => HasId(e, keyValues));
			
			return entity;
		}

		public TEntity Add(TEntity entity)
		{
			var entry = DbContext.Entry(entity);

			if (entry.State == EntityState.Detached)
			{
				DbSet.Add(entity);
			}

			entry.State = EntityState.Added;

			return entry.Entity;
		}

		public TEntity Update(TEntity entity)
		{
			var entry = DbContext.Entry(entity);

			if (entry.State == EntityState.Detached)
			{
				DbSet.Update(entity);
			}

			if (entry.State != EntityState.Added && entry.State != EntityState.Deleted)
				entry.State = EntityState.Modified;

			return entry.Entity;
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			var entitiesArray = entities as TEntity[] ?? entities.ToArray();

			foreach (var entity in entitiesArray)
			{
				Add(entity);
			}
		}

		public TEntity Remove(TEntity entity)
		{
			var entry = DbContext.Entry(entity);

			if (entry.State == EntityState.Detached)
			{
				DbSet.Attach(entity);
				DbSet.Remove(entity);
				return entry.Entity;
			}

			if (entry.State != EntityState.Deleted)
			{
				entry.State = EntityState.Deleted;
				return entry.Entity;
			}

			return entity;
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			var entitiesArray = entities as TEntity[] ?? entities.ToArray();

			foreach (var entity in entitiesArray)
			{
				RemoveAsync(entity);
			}
		}

		public int Count(ISpecification<TEntity> specification)
		{
			var query = GetQueryFromSpecification(specification);

			return query
				.Count();
		}

		public TEntity Find(ISpecification<TEntity> specification)
		{
			var query = GetQueryFromSpecification(specification);

			return query
				.FirstOrDefault();
		}

		public IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification)
		{
			var query = GetQueryFromSpecification(specification);

			return query
				.ToList();
		}

		protected virtual IQueryable<TEntity> GetQueryFromSpecification(ISpecification<TEntity> specification)
		{
			var visitor = new EfCoreSpecificationVisitor<TEntity>();
			specification.Accept(visitor);

			var query = visitor.BuildQuery(GetBaseQuery(DbSet));
			return query;
		}

		protected virtual IQueryable<TEntity> GetBaseQuery(DbSet<TEntity> set)
		{
			return set;
		}

		protected abstract bool HasId(TEntity entity, params object[] keyValues);
	}

	public abstract class EfCoreRepository<TEntity, TKey1> : EfCoreRepository<TEntity>, IEfCoreRepository<TEntity, TKey1>
		where TEntity : class
	{
		protected EfCoreRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public async Task<TEntity> GetByIdAsync(TKey1 keyValue1,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			return await GetByIdAsync(new object[] {keyValue1}, cancellationToken);
		}

		public TEntity GetById(TKey1 keyValue1)
		{
			return GetById(new object[] {keyValue1});
		}
	}

	public abstract class EfCoreRepository<TEntity, TKey1, TKey2> : EfCoreRepository<TEntity>,
		IEfCoreRepository<TEntity, TKey1, TKey2> where TEntity : class
	{
		protected EfCoreRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public TEntity GetById(TKey1 keyValue1, TKey2 keyValue2)
		{
			return GetById(new object[] {keyValue1, keyValue2});
		}

		public async Task<TEntity> GetByIdAsync(TKey1 keyValue1, TKey2 keyValue2,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			return await GetByIdAsync(new object[] {keyValue1, keyValue2}, cancellationToken);
		}
	}

	public abstract class EfCoreRepository<TEntity, TKey1, TKey2, TKey3> : EfCoreRepository<TEntity>,
		IEfCoreRepository<TEntity, TKey1, TKey2, TKey3> where TEntity : class
	{
		protected EfCoreRepository(DbContext dbContext) : base(dbContext)
		{
		}

		public TEntity GetById(TKey1 keyValue1, TKey2 keyValue2, TKey3 keyValue3)
		{
			return GetById(new object[] {keyValue1, keyValue2, keyValue3});
		}

		public async Task<TEntity> GetByIdAsync(TKey1 keyValue1, TKey2 keyValue2, TKey3 keyValue3,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			return await GetByIdAsync(new object[] {keyValue1, keyValue2, keyValue3}, cancellationToken);
		}
	}
}