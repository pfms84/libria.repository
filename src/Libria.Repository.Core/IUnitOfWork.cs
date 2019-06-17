namespace Libria.Repository.Core
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IUnitOfWork : IDisposable
	{
		Task RollbackAsync(CancellationToken ct = default(CancellationToken));
		Task BeginTransactionAsync(CancellationToken ct = default(CancellationToken));
		Task CommitAsync(CancellationToken ct = default(CancellationToken));

		void Rollback();
		void BeginTransaction();
		void Commit();
	}
}