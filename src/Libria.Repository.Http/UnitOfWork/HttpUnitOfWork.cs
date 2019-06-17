namespace Libria.Repository.Http.UnitOfWork
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Core;

	public class HttpUnitOfWork : IUnitOfWork
	{
		private HttpContext _httpContext;

		public HttpUnitOfWork(HttpContext httpContext)
		{
			_httpContext = httpContext;
		}

		public async Task RollbackAsync(CancellationToken ct = default(CancellationToken))
		{
			await _httpContext.RollbackAsync(ct);
		}

		public async Task BeginTransactionAsync(CancellationToken ct = default(CancellationToken))
		{
			await _httpContext.BeginTransactionAsync(ct);
		}

		public async Task CommitAsync(CancellationToken ct = default(CancellationToken))
		{
			await _httpContext.CommitAsync(ct);
		}

		public void Rollback()
		{
			throw new System.NotImplementedException();
		}

		public void BeginTransaction()
		{
			throw new System.NotImplementedException();
		}

		public void Commit()
		{
			throw new System.NotImplementedException();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_httpContext?.Dispose();
				_httpContext = null;
			}
		}
	}
}