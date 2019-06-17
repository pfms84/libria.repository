using Libria.Repository.EFCore.Specifications;

namespace Libria.Repository.EFCore.Tests
{
	public class PaginatedSpecification : EfCoreUnarySpecification<TestEntity>
	{
		public PaginatedSpecification(
			IEfCoreSpecification<TestEntity> spec, 
			int page, 
			int pageSize)
			: base(
				new EfCoreSkipSpecification<TestEntity>(
					new EfCoreTakeSpecification<TestEntity>(spec, page * pageSize), (page - 1) * pageSize))
		{
		}
	}
}