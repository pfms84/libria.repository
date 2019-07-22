namespace Libria.Repository.EFCore.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Runtime.InteropServices;
	using System.Threading;
	using System.Threading.Tasks;
	using Contracts;
	using Core;
	using Core.Specification;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;
	using Specifications;

	public abstract class EfCoreRepository<TEntity, TKey> : 
		IRepository<TEntity, TKey>,
		IAsyncRepository<TEntity, TKey> where TEntity : class
	{
		protected readonly DbContext DbContext;
		protected readonly DbSet<TEntity> DbSet;

		protected EfCoreRepository(IEfCoreUnitOfWork unitOfWork)
		{
			DbContext = unitOfWork.DbContext;
			
			DbSet = DbContext.Set<TEntity>();
		}

		public Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken) )
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(GetById(id));
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

		public TEntity GetById(TKey id)
		{
			var entity = GetBaseQuery(DbSet)
				.SingleOrDefault(GetByIdExpression(id));
			
			return entity;
		}

		public TEntity Add(TEntity entity)
		{
			var entry = GetEntityEntry(entity);

			if (entry.State == EntityState.Detached)
			{
				DbSet.Add(entity);
			}

			entry.State = EntityState.Added;

			return entry.Entity;
		}

		public TEntity Update(TEntity entity)
		{
			var entry = GetEntityEntry(entity);

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
			var entry = GetEntityEntry(entity);

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
		
		private EntityEntry<TEntity> GetEntityEntry(TEntity entity)
		{
			var entry = DbContext.Entry(entity);

			return entry;
		}

		protected Expression<Func<TEntity, bool>> GetByIdExpression(TKey id)
		{
			var metadata = DbContext.Model.FindEntityType(typeof(TEntity));
			var primaryKey = metadata.FindPrimaryKey();
			var keyProperty = primaryKey.Properties.Single();

			var paramExpression = Expression.Parameter(typeof(TEntity), "entity");
			var constantExpression = Expression.Constant(id, id.GetType());
			var memberExpression = Expression.Property(paramExpression, keyProperty.PropertyInfo.Name);
			var equalExpression = Expression.Equal(memberExpression, constantExpression);

			return Expression.Lambda<Func<TEntity, bool>>(equalExpression, paramExpression);
		}
	}
}