namespace Libria.Repository.Core.Specification
{
	using System;
	using System.Linq.Expressions;

	public class CriteriaSpecification<T> : BaseSpecification<T>, ICriteriaSpecification<T>
	{
		public CriteriaSpecification(Expression<Func<T, bool>> criteria)
		{
			Criteria = criteria;
		}

		public Expression<Func<T, bool>> Criteria { get; }
	}
}