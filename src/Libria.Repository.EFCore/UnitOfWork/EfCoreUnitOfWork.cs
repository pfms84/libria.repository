namespace Libria.Repository.EFCore.UnitOfWork
{
	using System;
	using System.Data;
	using System.Threading;
	using System.Threading.Tasks;
	using Core;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Storage;

	public class EfCoreUnitOfWork : IUnitOfWork
	{
		private readonly IsolationLevel _isolationLevel;
		private DbContext _dbContext;
		private IDbContextTransaction _transaction;

		public EfCoreUnitOfWork(
			DbContext dbContext,
			IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			_dbContext = dbContext;
			_isolationLevel = isolationLevel;
		}

		public async Task RollbackAsync(CancellationToken ct = default(CancellationToken))
		{
			_transaction?.Rollback();
			await Task.CompletedTask;
		}

		public async Task BeginTransactionAsync(CancellationToken ct = default(CancellationToken))
		{
			_transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel, ct);
		}

		public async Task CommitAsync(CancellationToken ct = default(CancellationToken))
		{
			await _dbContext.SaveChangesAsync(ct);
			_transaction?.Commit();
		}

		public void Rollback()
		{
			_transaction?.Rollback();
		}

		public void BeginTransaction()
		{
			_transaction = _dbContext.Database.BeginTransaction(_isolationLevel);
		}

		public void Commit()
		{
			_dbContext.SaveChanges();
			_transaction?.Commit();
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
				_dbContext?.Dispose();
				_dbContext = null;
			}
		}
	}
}