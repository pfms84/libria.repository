namespace Libria.Repository.Core.Specification
{
	using System;

	public abstract class SpecificationVisitor<T> : ISpecificationVisitor<T>
	{
		public void Visit(ISpecification<T> specification)
		{
			switch (specification)
			{
				case IUnarySpecification<T> unarySpec:
					VisitUnary(unarySpec);
					break;
				case IBinarySpecification<T> binarySpec:
					VisitBinary(binarySpec);
					break;
				default:
					throw new Exception($"{specification.GetType()} not supported");
			}
		}

		protected virtual void VisitUnary(IUnarySpecification<T> specification)
		{
			switch (specification)
			{
				case ICriteriaSpecification<T> spec:
				{
					VisitCriteria(spec);
					break;
				}
				case INegateSpecification<T> spec:
				{
					VisitNegate(spec);
					break;
				}
				case ISkipSpecification<T> spec:
				{
					VisitSkip(spec);
					break;
				}
				case ITakeSpecification<T> spec:
				{
					VisitTake(spec);
					break;
				}
				case IOrderBySpecification<T> spec:
				{
					VisitOrderBy(spec);
					break;
				}
				case IOrderByDescendingSpecification<T> spec:
				{
					VisitOrderByDescending(spec);
					break;
				}
				case IPageSpecification<T> spec:
				{
					VisitPage(spec);
					break;
				}
				default:
				{
					throw new Exception($"{specification.GetType()} not supported");
				}
			}
		}

		protected virtual void VisitBinary(IBinarySpecification<T> specification)
		{
			switch (specification)
			{
				case IAndSpecification<T> spec:
				{
					VisitAnd(spec);
					break;
				}
				case IOrSpecification<T> spec:
				{
					VisitOr(spec);
					break;
				}
				default:
				{
					throw new Exception($"{specification.GetType()} not supported");
				}
			}
		}

		protected abstract void VisitAnd(IAndSpecification<T> spec);

		protected abstract void VisitOr(IOrSpecification<T> spec);

		protected abstract void VisitOrderByDescending(IOrderByDescendingSpecification<T> spec);

		protected abstract void VisitOrderBy(IOrderBySpecification<T> spec);

		protected abstract void VisitTake(ITakeSpecification<T> spec);

		protected abstract void VisitSkip(ISkipSpecification<T> spec);

		protected abstract void VisitNegate(INegateSpecification<T> spec);

		protected abstract void VisitCriteria(ICriteriaSpecification<T> spec);

		protected abstract void VisitPage(IPageSpecification<T> spec);
	}
}