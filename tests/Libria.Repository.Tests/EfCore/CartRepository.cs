namespace Libria.Repository.Tests.EfCore
{
	using System;
	using System.Linq;
	using EFCore.Contracts;
	using EFCore.Repository;
	using Microsoft.EntityFrameworkCore;
	using Models;

	public class CartRepository : EfCoreRepository<Cart, Guid>
	{
		public CartRepository(IEfCoreUnitOfWork unitOfWork) : base(unitOfWork)
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