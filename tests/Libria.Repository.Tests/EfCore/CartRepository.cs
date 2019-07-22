namespace Libria.Repository.Tests.EfCore
{
	using System;
	using System.Linq;
	using EFCore.Contracts;
	using Microsoft.EntityFrameworkCore;
	using Models;


	public class CartRepository : BaseTestRepository<Cart, Guid>
	{
		public CartRepository(IEfCoreUnitOfWork<TestDbContext> unitOfWork) : base(unitOfWork)
		{
		}

		//protected override IQueryable<Cart> GetBaseQuery(DbSet<Cart> set)
		//{
		//	return base.GetBaseQuery(set)
		//		.Include(e => e.Items)
		//			.ThenInclude(e => e.Price);
		//}
	}
}