namespace Libria.Repository.EFCore.Contracts
{
	using Core;
	using Microsoft.EntityFrameworkCore;

	public interface IEfCoreUnitOfWork: IUnitOfWork
	{
		DbContext DbContext { get; }
	}
}