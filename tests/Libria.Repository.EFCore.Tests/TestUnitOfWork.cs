namespace Libria.Repository.EFCore.Tests
{
	using System.Data;
	using Contracts.Repository;
	using Core;
	using Microsoft.EntityFrameworkCore;
	using UnitOfWork;

	public class TestUnitOfWork : EfCoreUnitOfWork
	{
		public TestUnitOfWork(DbContext dbContext,
			IEfCoreRepository<TestEntity, int> testEntityEfCoreRepository,
			IEfCoreRepository<TestEntityNavigation, int> testEntityNavigationEfCoreRepository)
			: base(dbContext)
		{
			TestEntityEfCoreRepository = testEntityEfCoreRepository;
			TestEntityNavigationEfCoreRepository = testEntityNavigationEfCoreRepository;
		}

		public IEfCoreRepository<TestEntity, int> TestEntityEfCoreRepository { get; }
		public IEfCoreRepository<TestEntityNavigation, int> TestEntityNavigationEfCoreRepository { get; }
	}
}