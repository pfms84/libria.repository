namespace Libria.Repository.Tests.EfCore
{
	using EFCore.Contracts;
	using EFCore.Repository;

	public abstract class BaseTestRepository<TEntity, TKey> : EfCoreRepository<TEntity, TKey, TestDbContext> 
		where TEntity : class
	{
		protected BaseTestRepository(IEfCoreUnitOfWork<TestDbContext> unitOfWork) : base(unitOfWork)
		{
		}
	}
}