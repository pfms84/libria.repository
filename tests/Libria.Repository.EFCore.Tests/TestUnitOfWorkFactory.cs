namespace Libria.Repository.EFCore.Tests
{
	using System.Data;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Contracts.Repository;
	using Core;
	using Microsoft.EntityFrameworkCore;
	using Repository;
	using UnitOfWork;

	public class TestEntityRepository : EfCoreRepository<TestEntity, int>
	{
		public TestEntityRepository(DbContext dbContext) : base(dbContext)
		{
		}

		protected override IQueryable<TestEntity> GetBaseQuery(DbSet<TestEntity> set)
		{
			return set.Include(e => e.EntityNavigations)
				.Include(e => e.EntityNavigations2);
		}

		protected override bool HasId(TestEntity entity, params object[] keyValues)
		{
			return entity.Id.Equals(keyValues[0]);
		}
	}

	public class TestEntityNavigationRepository : EfCoreRepository<TestEntityNavigation, int>
	{
		public TestEntityNavigationRepository(DbContext dbContext) : base(dbContext)
		{
		}

		protected override bool HasId(TestEntityNavigation entity, params object[] keyValues)
		{
			return entity.Id.Equals(keyValues[0]);
		}
	}

	public class TestUnitOfWorkFactory : IUnitOfWorkFactory<TestUnitOfWork>
	{
		public void ResetDatabase()
		{
			var dbContext = new TestDbContext();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
		}

		public TestUnitOfWork Create()
		{
			var dbContext = new TestDbContext();
			
			return new TestUnitOfWork(dbContext,
				new TestEntityRepository(dbContext),
				new TestEntityNavigationRepository(dbContext));
		}
	}
}