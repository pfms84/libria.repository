namespace Libria.Repository.Tests.NHibernate
{
	using global::NHibernate;
	using Models;
	using Repository.NHibernate.Contracts;
	using Repository.NHibernate.Repository;

	public class ProductRepository : BaseTestRepository<Product, int>
	{
		public ProductRepository(INHibernateUnitOfWork<ISession> unitOfWork) : base(unitOfWork)
		{
		}
	}
}