namespace Libria.Repository.Core.Specification
{
	using System;
	using System.Linq.Expressions;

	public interface ISpecification<T>
	{
		//bool IsSatisfiedBy(T obj);
		void Accept(ISpecificationVisitor<T> visitor);

		ISpecification<T> Negate();
		ISpecification<T> Skip(int skip);
		ISpecification<T> Take(int take);
		ISpecification<T> Page(int pageNumber, int pageSize);
		ISpecification<T> OrderBy(Expression<Func<T, object>> property);
		ISpecification<T> OrderByDescending(Expression<Func<T, object>> property);
		ISpecification<T> And(ISpecification<T> specification);
		ISpecification<T> Or(ISpecification<T> specification);
	}
}