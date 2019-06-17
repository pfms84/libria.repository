namespace Libria.Repository.EFCore.Tests
{
	using Specifications;

	public class WithIdSpecification : EfCoreCriteriaSpecification<TestEntity>
	{
		public WithIdSpecification(int id)
			: base(e => e.Id == id)
		{
		}
	}
}