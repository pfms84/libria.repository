namespace Libria.Repository.Core.Specification
{
	using System;
	using System.Linq.Expressions;

	public abstract class BaseSpecification<T> : ISpecification<T>
	{
		public void Accept(ISpecificationVisitor<T> visitor)
		{
			visitor.Visit(this);
		}

		public ISpecification<T> Negate()
		{
			return new NegateSpecification<T>(this);
		}

		public ISpecification<T> Skip(int skip)
		{
			return new SkipSpecification<T>(this, skip);
		}

		public ISpecification<T> Take(int take)
		{
			return new TakeSpecification<T>(this, take);
		}

		public ISpecification<T> Page(int pageNumber, int pageSize)
		{
			return new PageSpecification<T>(this, pageNumber, pageSize);
		}

		public ISpecification<T> OrderBy(Expression<Func<T, object>> property)
		{
			return new OrderBySpecification<T>(this, property);
		}

		public ISpecification<T> OrderByDescending(Expression<Func<T, object>> property)
		{
			return new OrderByDescendingSpecification<T>(this, property);
		}

		public ISpecification<T> And(ISpecification<T> specification)
		{
			return new AndSpecification<T>(this, specification);
		}

		public ISpecification<T> Or(ISpecification<T> specification)
		{
			return new OrSpecification<T>(this, specification);
		}
	}
}