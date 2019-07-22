namespace Libria.Repository.Tests.EfCore
{
	using EFCore.Contracts;
	using EFCore.Repository;
	using Models;

	public class ProductRepository : BaseTestRepository<Product, int>
	{
		public ProductRepository(IEfCoreUnitOfWork<TestDbContext> unitOfWork) : base(unitOfWork)
		{
		}
	}
}