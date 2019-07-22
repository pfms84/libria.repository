namespace Libria.Repository.EFCore.Contracts
{
	using Core;
	using Microsoft.EntityFrameworkCore;

	public interface IEfCoreUnitOfWork<out TDbContext>: IUnitOfWork
		where TDbContext : DbContext
	{
		TDbContext DbContext { get; }
	}
}