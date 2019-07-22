namespace Libria.Repository.Tests.NHibernate
{
	using System;
	using Models;
	using Repository.NHibernate.Contracts;
	using Repository.NHibernate.Repository;

	public class CartRepository : NHibernateRepository<Cart, Guid>
	{
		public CartRepository(INHibernateUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}
	}
}