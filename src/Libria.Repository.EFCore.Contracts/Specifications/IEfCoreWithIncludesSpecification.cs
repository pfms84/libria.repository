namespace Libria.Repository.EFCore.Contracts.Specifications
{
	using System;
	using System.Linq.Expressions;

	public interface IEfCoreWithIncludesSpecification<T> : IEfCoreSpecification<T>
	{
		IEfCoreSpecification<T> Inner { get; }
		Expression<Func<T, object>>[] Includes { get; }
	}
}