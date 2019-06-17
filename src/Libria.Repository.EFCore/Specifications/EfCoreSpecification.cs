namespace Libria.Repository.EFCore.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Contracts.Specifications;
	using Core.Specification;

	public abstract class EfCoreSpecification<T> : BaseSpecification<T>, IEfCoreSpecification<T>
	{
		public IEfCoreSpecification<T> WithIncludes(params Expression<Func<T, object>>[] includes)
		{
			return new EfCoreWithIncludesSpecification<T>(this, includes);
		}

		public IEfCoreSpecification<T> WithIncludeStrings(params string[] includeStrings)
		{
			return new EfCoreWithIncludeStringsSpecification<T>(this, includeStrings);
		}
	}
}