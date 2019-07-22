namespace Libria.Repository.NHibernate.UnitOfWork
{
	using System;
	using System.Data;
	using System.Threading;
	using System.Threading.Tasks;
	using Contracts;
	using global::NHibernate;

	public class NHibernateUnitOfWork<TSession>: INHibernateUnitOfWork<TSession> 
		where TSession : ISession
	{
		private readonly IsolationLevel _isolationLevel;
		private ITransaction _transaction;

		public NHibernateUnitOfWork(TSession session,
			IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			Session = session;
			_isolationLevel = isolationLevel;
		}

		public async Task RollbackAsync(CancellationToken ct = default(CancellationToken))
		{
			await _transaction.RollbackAsync(ct);
			_transaction.Dispose();
			_transaction = null;
		}

		public Task BeginTransactionAsync(CancellationToken ct = default(CancellationToken))
		{
			ct.ThrowIfCancellationRequested();
			BeginTransaction();
			return Task.CompletedTask;
		}

		public async Task CommitAsync(CancellationToken ct = default(CancellationToken))
		{
			await _transaction.CommitAsync(ct);
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

			_transaction = Session.BeginTransaction(_isolationLevel);
		}

		public void Commit()
		{
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
				Session?.Dispose();
				_transaction?.Dispose();
				Session = default(TSession);
				_transaction = null;
			}
		}

		public TSession Session { get; private set; }
	}
}