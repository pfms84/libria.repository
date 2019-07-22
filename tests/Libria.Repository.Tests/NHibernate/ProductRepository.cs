namespace Libria.Repository.Tests.NHibernate
{
	using Models;
	using Repository.NHibernate.Contracts;
	using Repository.NHibernate.Repository;

	public class ProductRepository : NHibernateRepository<Product, int>
	{
		public ProductRepository(INHibernateUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
	}
}