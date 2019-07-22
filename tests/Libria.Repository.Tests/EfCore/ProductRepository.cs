namespace Libria.Repository.Tests.EfCore
{
	using EFCore.Contracts;
	using EFCore.Repository;
	using Models;

	public class ProductRepository : EfCoreRepository<Product, int>
	{
		public ProductRepository(IEfCoreUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
	}
}