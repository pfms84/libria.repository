namespace Libria.Repository.EFCore.Tests
{
	using System;
	using System.Linq.Expressions;
	using Specifications;

	public class GetAllSpecification : EfCoreCriteriaSpecification<TestEntity>
	{
		public GetAllSpecification() : base(null)
		{
		}
	}
}