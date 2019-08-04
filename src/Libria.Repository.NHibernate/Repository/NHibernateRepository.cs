namespace Libria.Repository.NHibernate.Repository
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
	using global::NHibernate;
	using global::NHibernate.Event.Default;
	using global::NHibernate.Linq;
	using Specifications;
	using UnitOfWork;

	public class NHibernateRepository<TEntity, TKey, TSession> : 
		IRepository<TEntity, TKey>, 
		IAsyncRepository<TEntity, TKey> where TEntity : class
		where TSession : ISession
	{
		protected readonly TSession Session;

		protected IQueryable<TEntity> Query => Session.Query<TEntity>();

		public NHibernateRepository(INHibernateUnitOfWork<TSession> unitOfWork)
		{
			Session = unitOfWork.Session;
		}

		public virtual Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
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
			var entity = Session.Get<TEntity>(id);
			
			return entity;
		}

		public virtual TEntity Add(TEntity entity)
		{
			Session.Save(entity);
			return entity;
		}

		public virtual TEntity Update(TEntity entity)
		{
			Session.Update(entity);
			return entity;
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
			Session.Delete(entity);

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
			var visitor = new NHibernateSpecificationVisitor<TEntity>();
			specification.Accept(visitor);

			var query = visitor.BuildQuery(Query);
			return query;
		}
	}
}