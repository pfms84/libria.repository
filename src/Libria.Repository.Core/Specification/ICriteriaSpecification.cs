namespace Libria.Repository.Core.Specification
{
	using System;
	using System.Linq.Expressions;

	public interface ICriteriaSpecification<T> : IUnarySpecification<T>
	{
		Expression<Func<T, bool>> Criteria { get; }
	}
}