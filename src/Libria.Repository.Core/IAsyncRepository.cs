namespace Libria.Repository.Core
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Specification;

	public interface IAsyncRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(params object[] keyValues);
		Task<TEntity> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken);

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

	public interface IAsyncRepository<TEntity, in TKey1>
		: IAsyncRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(TKey1 keyValue1, CancellationToken cancellationToken = default(CancellationToken));
	}

	public interface IAsyncRepository<TEntity, in TKey1, in TKey2>
		: IAsyncRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(TKey1 keyValue1, TKey2 keyValue2,
			CancellationToken cancellationToken = default(CancellationToken));
	}

	public interface IAsyncRepository<TEntity, in TKey1, in TKey2, in TKey3>
		: IAsyncRepository<TEntity> where TEntity : class
	{
		Task<TEntity> GetByIdAsync(TKey1 keyValue1, TKey2 keyValue2, TKey3 keyValue3,
			CancellationToken cancellationToken = default(CancellationToken));
	}
}