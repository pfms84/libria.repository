namespace Libria.Repository.EFCore.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Contracts.Specifications;

	public class EfCoreWithIncludesSpecification<T> : EfCoreSpecification<T>, IEfCoreWithIncludesSpecification<T>
	{
		public EfCoreWithIncludesSpecification(IEfCoreSpecification<T> inner, params Expression<Func<T, object>>[] includes)
		{
			Inner = inner;
			Includes = includes ?? Array.Empty<Expression<Func<T, object>>>();
		}

		public IEfCoreSpecification<T> Inner { get; }
		public Expression<Func<T, object>>[] Includes { get; }
	}
}