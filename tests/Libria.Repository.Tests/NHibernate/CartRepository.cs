namespace Libria.Repository.Tests.NHibernate
{
	using System;
	using global::NHibernate;
	using Models;
	using Repository.NHibernate.Contracts;

	public class CartRepository : BaseTestRepository<Cart, Guid>
	{
		public CartRepository(INHibernateUnitOfWork<ISession> unitOfWork) : base(unitOfWork)
		{
		}
	}
}