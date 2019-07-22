namespace Libria.Repository.Core
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Specification;

	public interface IRepository<TEntity, in TKey> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));

		Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

		Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

		Task AddRangeAsync(IEnumerable<TEntity> entities,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<TEntity> RemoveAsync(TEntity entity,
			CancellationToken cancellationToken = default(CancellationToken));

		Task RemoveRangeAsync(IEnumerable<TEntity> entities,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<int> CountAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<TEntity> FindAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken));

		Task<IEnumerable<TEntity>> FindAllAsync(ISpecification<TEntity> specification,
			CancellationToken cancellationToken = default(CancellationToken));

		TEntity GetById(TKey id);

		TEntity Add(TEntity entity);

		TEntity Update(TEntity entity);

		void AddRange(IEnumerable<TEntity> entities);

		TEntity Remove(TEntity entity);

		void RemoveRange(IEnumerable<TEntity> entities);

		int Count(ISpecification<TEntity> specification);

		TEntity Find(ISpecification<TEntity> specification);

		IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification);
	}
}