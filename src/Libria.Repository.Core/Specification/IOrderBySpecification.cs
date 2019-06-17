namespace Libria.Repository.Core.Specification
{
	using System;
	using System.Linq.Expressions;

	public interface IOrderBySpecification<T> : IUnarySpecification<T>
	{
		Expression<Func<T, object>> Property { get; }
		ISpecification<T> Inner { get; }
	}
}