namespace Libria.Repository.Core
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Specification;

	public interface IAsyncRepository<TEntity, in TKey> where TEntity : class
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
	}
}