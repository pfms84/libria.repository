namespace Libria.Repository.EFCore.Tests
{
	using Core.Specification;

	public class WithPropStartingWithSpecification : CriteriaSpecification<TestEntity>
	{
		public WithPropStartingWithSpecification(string query)
			: base(e => e.Prop1.StartsWith(query))
		{
		}
	}
}