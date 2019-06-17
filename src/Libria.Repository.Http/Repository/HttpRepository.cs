using System;
using System.Collections.Generic;
using System.Text;

namespace Libria.Repository.Http.Repository
{
	using System.Threading;
	using System.Threading.Tasks;
	using Core;
	using Core.Specification;

	public interface IHttpRepository
	{
		
	} 

	public abstract class HttpRepository<TEntity> : IHttpRepository, IAsyncRepository<TEntity> where TEntity : class
	{
		protected HttpContext HttpContext { get; }

		protected HttpRepository(HttpContext httpContext)
		{
			HttpContext = httpContext;
		}

		public abstract Task<TEntity> GetByIdAsync(params object[] keyValues);
		public abstract Task<TEntity> GetByIdAsync(object[] keyValues, CancellationToken cancellationToken);
		public abstract Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task<TEntity> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task<TEntity> FindAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default(CancellationToken));
		public abstract Task<IEnumerable<TEntity>> FindAllAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default(CancellationToken));
	}
}
