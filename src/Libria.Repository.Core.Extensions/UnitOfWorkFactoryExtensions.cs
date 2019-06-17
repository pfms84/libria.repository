namespace Libria.Repository.Core.Extensions
{
	using System;
	using System.Data;
	using System.Threading;
	using System.Threading.Tasks;

	public static class UnitOfWorkFactoryExtensions
	{
		public static async Task<TResult> ReadAsync<T, TResult>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, CancellationToken, Task<TResult>> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				var result = await exec(uow, ct);

				return result;
			}
		}

		public static async Task ReadAsync<T>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, CancellationToken, Task> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				await exec(uow, ct);
			}
		}

		public static async Task<TResult> ReadAsync<T, TResult>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, Task<TResult>> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				var result = await exec(uow);

				return result;
			}
		}

		public static async Task ReadAsync<T>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, Task> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				await exec(uow);
			}
		}

		public static async Task<TResult> ExecuteAndCommitAsync<T, TResult>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, CancellationToken, Task<TResult>> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				try
				{
					await uow.BeginTransactionAsync(ct);
					var result = await exec(uow, ct);
					await uow.CommitAsync(ct);

					return result;
				}
				catch (Exception)
				{
					await uow.RollbackAsync(ct);
					throw;
				}
			}
		}

		public static async Task<TResult> ExecuteAndCommitAsync<T, TResult>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, Task<TResult>> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				try
				{
					await uow.BeginTransactionAsync(ct);
					var result = await exec(uow);
					await uow.CommitAsync(ct);

					return result;
				}
				catch (Exception)
				{
					await uow.RollbackAsync(ct);
					throw;
				}
			}
		}

		public static async Task ExecuteAndCommitAsync<T>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, CancellationToken, Task> exec,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				try
				{
					await uow.BeginTransactionAsync(ct);
					await exec(uow, ct);
					await uow.CommitAsync(ct);
				}
				catch (Exception)
				{
					await uow.RollbackAsync(ct);
					throw;
				}
			}
		}

		public static async Task ExecuteAndCommitAsync<T>(
			this IUnitOfWorkFactory<T> factory,
			Func<T, Task> execAction,
			CancellationToken ct = default(CancellationToken))
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				try
				{
					await uow.BeginTransactionAsync(ct);
					await execAction(uow);
					await uow.CommitAsync(ct);
				}
				catch (Exception)
				{
					await uow.RollbackAsync(ct);
					throw;
				}
			}
		}

		public static void ExecuteAndCommit<T>(
			this IUnitOfWorkFactory<T> factory,
			Action<T> exec)
			where T : IUnitOfWork
		{
			using (var uow = factory.Create())
			{
				try
				{
					uow.BeginTransaction();
					exec(uow);
					uow.Commit();
				}
				catch (Exception)
				{
					uow.Rollback();
					throw;
				}
			}
		}
	}
}