namespace Libria.Repository.EFCore.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Contracts.Specifications;

	public class EfCoreCriteriaSpecification<T> : EfCoreSpecification<T>, IEfCoreCriteriaSpecification<T>
	{
		public EfCoreCriteriaSpecification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}

		public Expression<Func<T, bool>> Criteria { get; }
	}
}