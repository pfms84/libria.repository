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

	public class NHibernateRepository<TEntity, TKey> : 
		IRepository<TEntity, TKey>, 
		IAsyncRepository<TEntity, TKey> where TEntity : class
	{
		protected readonly ISession Session;

		protected IQueryable<TEntity> Query => Session.Query<TEntity>();

		public NHibernateRepository(INHibernateUnitOfWork unitOfWork)
		{
			Session = unitOfWork.Session;
		}
		
		public Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
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
			var entity = Session.Get<TEntity>(id);
			
			return entity;
		}

		public TEntity Add(TEntity entity)
		{
			Session.Save(entity);
			return entity;
		}

		public TEntity Update(TEntity entity)
		{
			Session.Update(entity);
			return entity;
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
			Session.Delete(entity);

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
			var visitor = new NHibernateSpecificationVisitor<TEntity>();
			specification.Accept(visitor);

			var query = visitor.BuildQuery(Query);
			return query;
		}
	}
}