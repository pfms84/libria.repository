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

	public class EfCoreRepository<TEntity, TKey, TDbContext> : 
		IRepository<TEntity, TKey>,
		IAsyncRepository<TEntity, TKey> where TEntity : class
		where TDbContext : DbContext
	{
		protected readonly DbContext DbContext;
		protected readonly DbSet<TEntity> DbSet;

		public EfCoreRepository(IEfCoreUnitOfWork<TDbContext> unitOfWork)
		{
			DbContext = unitOfWork.DbContext;
			
			DbSet = DbContext.Set<TEntity>();
		}

		public virtual Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken) )
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(GetById(id));
		}

		public virtual Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(Add(entity));
		}

		public virtual Task<TEntity> UpdateAsync(TEntity entity,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(Update(entity));
		}

		public virtual Task AddRangeAsync(IEnumerable<TEntity> entities,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			AddRange(entities);

			return Task.CompletedTask;
		}

		public virtual Task<TEntity> RemoveAsync(TEntity entity,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			return Task.FromResult(Remove(entity));
		}

		public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			RemoveRange(entities);

			return Task.CompletedTask;
		}

		public virtual async Task<int> CountAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = GetQueryFromSpecification(specification);

			return await query
				.CountAsync(cancellationToken);
		}

		public virtual async Task<TEntity> FindAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = GetQueryFromSpecification(specification);

			return await query
				.FirstOrDefaultAsync(cancellationToken);
		}

		public virtual async Task<IEnumerable<TEntity>> FindAllAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var query = GetQueryFromSpecification(specification);

			return await query
				.ToListAsync(cancellationToken);
		}

		public virtual TEntity GetById(TKey id)
		{
			var entity = GetBaseQuery(DbSet)
				.SingleOrDefault(GetByIdExpression(id));
			
			return entity;
		}

		public virtual TEntity Add(TEntity entity)
		{
			var entry = GetEntityEntry(entity);

			if (entry.State == EntityState.Detached)
			{
				DbSet.Add(entity);
			}

			entry.State = EntityState.Added;

			return entry.Entity;
		}

		public virtual TEntity Update(TEntity entity)
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

		public virtual void AddRange(IEnumerable<TEntity> entities)
		{
			var entitiesArray = entities as TEntity[] ?? entities.ToArray();

			foreach (var entity in entitiesArray)
			{
				Add(entity);
			}
		}

		public virtual TEntity Remove(TEntity entity)
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

		public virtual void RemoveRange(IEnumerable<TEntity> entities)
		{
			var entitiesArray = entities as TEntity[] ?? entities.ToArray();

			foreach (var entity in entitiesArray)
			{
				RemoveAsync(entity);
			}
		}

		public virtual int Count(ISpecification<TEntity> specification)
		{
			var query = GetQueryFromSpecification(specification);

			return query
				.Count();
		}

		public virtual TEntity Find(ISpecification<TEntity> specification)
		{
			var query = GetQueryFromSpecification(specification);

			return query
				.FirstOrDefault();
		}

		public virtual IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification)
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