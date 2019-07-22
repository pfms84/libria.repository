namespace Libria.Repository.NHibernate.Specifications
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Core.Specification;

	public class NHibernateSpecificationVisitor<T> : SpecificationVisitor<T> where T : class
	{
		private VisitorContext _context = new VisitorContext(null, null, null);

		protected override void VisitAnd(IAndSpecification<T> spec)
		{
			spec.Left.Accept(this);
			var leftContext = _context;

			spec.Right.Accept(this);
			var rightContext = _context;

			var newContext = MergeBinaryContexts(leftContext, rightContext, Expression.AndAlso);
			_context = newContext;
		}

		protected override void VisitOr(IOrSpecification<T> spec)
		{
			spec.Left.Accept(this);
			var leftContext = _context;

			spec.Right.Accept(this);
			var rightContext = _context;

			var newContext = MergeBinaryContexts(leftContext, rightContext, Expression.OrElse);
			_context = newContext;
		}

		protected override void VisitOrderByDescending(IOrderByDescendingSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				innerContext.Sort != null
					? (Func<IQueryable<T>, IOrderedQueryable<T>>) (items => innerContext.Sort(items).ThenByDescending(spec.Property))
					: items => items.OrderByDescending(spec.Property),
				innerContext.PostProcess
			);

			_context = newContext;
		}

		protected override void VisitOrderBy(IOrderBySpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				innerContext.Sort != null
					? (Func<IQueryable<T>, IOrderedQueryable<T>>) (items => innerContext.Sort(items).ThenBy(spec.Property))
					: items => items.OrderBy(spec.Property),
				innerContext.PostProcess
			);

			_context = newContext;
		}

		protected override void VisitTake(ITakeSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				innerContext.Sort,
				innerContext.PostProcess != null
					? (Func<IQueryable<T>, IQueryable<T>>) (items => innerContext.PostProcess(items).Take(spec.Amount))
					: items => items.Take(spec.Amount)
			);

			_context = newContext;
		}

		protected override void VisitSkip(ISkipSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				innerContext.Sort,
				innerContext.PostProcess != null
					? (Func<IQueryable<T>, IQueryable<T>>) (items => innerContext.PostProcess(items).Skip(spec.Amount))
					: items => items.Skip(spec.Amount)
			);

			_context = newContext;
		}

		protected override void VisitNegate(INegateSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var objParam = Expression.Parameter(typeof(T), "obj");

			var newExpr = Expression.Lambda<Func<T, bool>>(
				Expression.Not(
					Expression.Invoke(innerContext.Criteria, objParam)
				),
				objParam
			);

			var newContext = new VisitorContext(
				newExpr,
				innerContext.Sort,
				innerContext.PostProcess
			);

			_context = newContext;
		}

		protected override void VisitCriteria(ICriteriaSpecification<T> spec)
		{
			var newContext = new VisitorContext(
				spec.Criteria,
				_context.Sort,
				_context.PostProcess
			);

			_context = newContext;
		}

		protected override void VisitPage(IPageSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;
			var skip = (spec.PageNumber - 1) * spec.PageSize;
			var take = spec.PageSize;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				innerContext.Sort,
				innerContext.PostProcess != null
					? (Func<IQueryable<T>, IQueryable<T>>)(items => innerContext.PostProcess(items).Skip(skip).Take(take))
					: items => items.Skip(skip).Take(take)
			);

			_context = newContext;
		}

		public IQueryable<T> BuildQuery(IQueryable<T> dbSet)
		{
			var baseQuery = dbSet;

			var query = _context.Criteria == null ? baseQuery : baseQuery.Where(_context.Criteria);

			if (_context.Sort != null)
				query = _context.Sort(query);

			if (_context.PostProcess != null)
				query = _context.PostProcess(query);

			return query;
		}

		private VisitorContext MergeBinaryContexts(
			VisitorContext leftContext,
			VisitorContext rightContext,
			Func<Expression, Expression, BinaryExpression> expressionOperator)
		{
			var objParam = Expression.Parameter(typeof(T), "obj");

			var rightSort = rightContext.Sort;
			var leftSort = leftContext.Sort;
			Func<IQueryable<T>, IOrderedQueryable<T>> sort;

			// Serious doubts about merging sorts and post processes
			if (leftSort != null && rightSort != null)
				sort = items => leftSort(rightSort(items));
			else
				sort = leftSort ?? rightSort;

			var rightPostProcess = rightContext.PostProcess;
			var leftPostProcess = leftContext.PostProcess;
			Func<IQueryable<T>, IQueryable<T>> postProcess;

			if (leftPostProcess != null && rightPostProcess != null)
				postProcess = items => leftPostProcess(rightPostProcess(items));
			else
				postProcess = leftPostProcess ?? rightPostProcess;

			var newExpr = Expression.Lambda<Func<T, bool>>(
				expressionOperator(
					Expression.Invoke(leftContext.Criteria, objParam),
					Expression.Invoke(rightContext.Criteria, objParam)
				),
				objParam
			);

			var newContext = new VisitorContext(
				newExpr,
				sort,
				postProcess
			);

			return newContext;
		}

		protected class VisitorContext
		{
			public VisitorContext(Expression<Func<T, bool>> criteria,
				Func<IQueryable<T>, IOrderedQueryable<T>> sort,
				Func<IQueryable<T>, IQueryable<T>> postProcess
			)
			{
				Criteria = criteria;
				Sort = sort;
				PostProcess = postProcess;
			}

			public Expression<Func<T, bool>> Criteria { get; }
			public Func<IQueryable<T>, IOrderedQueryable<T>> Sort { get; }
			public Func<IQueryable<T>, IQueryable<T>> PostProcess { get; }
		}
	}
}