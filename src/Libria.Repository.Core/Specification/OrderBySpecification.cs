namespace Libria.Repository.Core.Specification
{
	using System;
	using System.Linq.Expressions;

	public class OrderBySpecification<T> : BaseSpecification<T>, IOrderBySpecification<T>
	{
		public OrderBySpecification(ISpecification<T> inner, Expression<Func<T, object>> property)
		{
			Inner = inner;
			Property = property;
		}

		public ISpecification<T> Inner { get; }

		public Expression<Func<T, object>> Property { get; }
	}
}