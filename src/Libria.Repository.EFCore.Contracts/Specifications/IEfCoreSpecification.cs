namespace Libria.Repository.EFCore.Contracts.Specifications
{
	using System;
	using System.Linq.Expressions;
	using Core.Specification;

	public interface IEfCoreSpecification<T> : IUnarySpecification<T>
	{
		IEfCoreSpecification<T> WithIncludes(params Expression<Func<T, object>>[] includes);
		IEfCoreSpecification<T> WithIncludeStrings(params string[] includeStrings);
	}
}