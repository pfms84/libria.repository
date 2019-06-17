namespace Libria.Repository.Http
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using Repository;

	public class HttpContext : IDisposable
	{
		private bool _inTransaction;
		private bool _rollingBack;

		private readonly List<Func<CancellationToken, Task>> _rollbackActions = new List<Func<CancellationToken, Task>>();
		
		public HttpClient GetHttpClient()
		{
			var client = new HttpClient();
			return client;
		}

		public void AddRollbackAction(Func<CancellationToken, Task> callback)
		{
			if (_rollingBack)
			{
				throw new Exception("Can't add a rollback action during the rollback phase");
			}

			_rollbackActions.Add(callback);
		}

		public async Task RollbackAsync(CancellationToken ct)
		{
			if (!_inTransaction)
			{
				throw new Exception("Not possible to rollback outside a transaction");
			}

			_rollingBack = true;

			foreach (var rollbackAction in _rollbackActions)
			{
				try
				{
					await rollbackAction(ct);
				}
				catch (Exception)
				{
					// TODO: ver como resolver estas excepções
				}
			}

			_rollbackActions.Clear();
			_inTransaction = false;
			_rollingBack = false;
		}

		public Task BeginTransactionAsync(CancellationToken ct)
		{
			if (_inTransaction)
			{
				throw new Exception("Already inside a transaction");
			}

			_inTransaction = true;
			return Task.CompletedTask;
		}

		public Task CommitAsync(CancellationToken ct)
		{
			if (!_inTransaction)
			{
				throw new Exception("Not possible to commit outside a transaction");
			}

			_rollbackActions.Clear();
			_inTransaction = false;
			return Task.CompletedTask;
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
				if (_inTransaction)
				{
					throw new Exception("HttpContext cannot be disposed securely inside an open transaction");
				}
			}
		}
	}
}