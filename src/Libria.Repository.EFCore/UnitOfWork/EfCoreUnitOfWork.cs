namespace Libria.Repository.EFCore.UnitOfWork
{
	using System;
	using System.Data;
	using System.Threading;
	using System.Threading.Tasks;
	using Contracts;
	using Core;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Storage;

	public class EfCoreUnitOfWork<TDbContext> : IEfCoreUnitOfWork<TDbContext> 
		where TDbContext : DbContext
	{
		private readonly IsolationLevel _isolationLevel;
		private IDbContextTransaction _transaction;

		public TDbContext DbContext { get; private set; }

		public EfCoreUnitOfWork(
			TDbContext dbContext,
			IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			DbContext = dbContext;
			_isolationLevel = isolationLevel;
		}

		public async Task RollbackAsync(CancellationToken ct = default(CancellationToken))
		{
			_transaction.Rollback();
			_transaction.Dispose();
			_transaction = null;
			await Task.CompletedTask;
		}

		public async Task BeginTransactionAsync(CancellationToken ct = default(CancellationToken))
		{
			if (_transaction != null)
			{
				throw new Exception();
			}

			_transaction = await DbContext.Database.BeginTransactionAsync(_isolationLevel, ct);
		}

		public async Task CommitAsync(CancellationToken ct = default(CancellationToken))
		{
			await DbContext.SaveChangesAsync(ct);
			_transaction.Commit();
			_transaction.Dispose();
			_transaction = null;
		}

		public void Rollback()
		{
			_transaction.Rollback();
			_transaction.Dispose();
			_transaction = null;
		}

		public void BeginTransaction()
		{
			if (_transaction != null)
			{
				throw new Exception();
			}

			_transaction = DbContext.Database.BeginTransaction(_isolationLevel);
		}

		public void Commit()
		{
			DbContext.SaveChanges();
			_transaction.Commit();
			_transaction.Dispose();
			_transaction = null;
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
				DbContext?.Dispose();
				_transaction?.Dispose();
				DbContext = null;
				_transaction = null;
			}
		}
	}
}