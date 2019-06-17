namespace Libria.Repository.EFCore.UnitOfWork
{
	using System.Data;
	using Core;

    public abstract class EfCoreUnitOfWorkFactory<T> : IUnitOfWorkFactory<T>
		where T : IUnitOfWork
	{
		public abstract T Create();
	}
}