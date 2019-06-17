namespace Libria.Repository.EFCore.Specifications
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using Contracts.Specifications;
	using Core.Specification;
	using Microsoft.EntityFrameworkCore;

	public class EfCoreSpecificationVisitor<T> : SpecificationVisitor<T> where T : class
	{
		private VisitorContext _context = new VisitorContext(null, null, null, null, null);

		protected override void VisitUnary(IUnarySpecification<T> specification)
		{
			switch (specification)
			{
				case IEfCoreWithIncludesSpecification<T> spec:
				{
					VisitWithIncludes(spec);
					break;
				}
				case IEfCoreWithIncludeStringsSpecification<T> spec:
				{
					VisitWithIncludeStrings(spec);
					break;
				}
				default:
				{
					base.VisitUnary(specification);
					break;
				}
			}
		}

		protected virtual void VisitWithIncludeStrings(IEfCoreWithIncludeStringsSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				innerContext.Includes,
				spec.IncludeStrings.Concat(innerContext.IncludeStrings).ToList(),
				innerContext.Sort,
				innerContext.PostProcess
			);

			_context = newContext;
		}

		protected virtual void VisitWithIncludes(IEfCoreWithIncludesSpecification<T> spec)
		{
			spec.Inner.Accept(this);
			var innerContext = _context;

			var newContext = new VisitorContext(
				innerContext.Criteria,
				spec.Includes.Concat(innerContext.Includes).ToList(),
				innerContext.IncludeStrings,
				innerContext.Sort,
				innerContext.PostProcess
			);

			_context = newContext;
		}

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
				innerContext.Includes,
				innerContext.IncludeStrings,
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
				innerContext.Includes,
				innerContext.IncludeStrings,
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
				innerContext.Includes,
				innerContext.IncludeStrings,
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
				innerContext.Includes,
				innerContext.IncludeStrings,
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
				innerContext.Includes,
				innerContext.IncludeStrings,
				innerContext.Sort,
				innerContext.PostProcess
			);

			_context = newContext;
		}

		protected override void VisitCriteria(ICriteriaSpecification<T> spec)
		{
			var newContext = new VisitorContext(
				spec.Criteria,
				_context.Includes,
				_context.IncludeStrings,
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
				innerContext.Includes,
				innerContext.IncludeStrings,
				innerContext.Sort,
				innerContext.PostProcess != null
					? (Func<IQueryable<T>, IQueryable<T>>)(items => innerContext.PostProcess(items).Skip(skip).Take(take))
					: items => items.Skip(skip).Take(take)
			);

			_context = newContext;
		}

		public IQueryable<T> BuildQuery(IQueryable<T> dbSet)
		{
			var baseQuery = dbSet.AsNoTracking();

			// fetch a Queryable that includes all expression-based includes
			var queryableResultWithIncludes = _context.Includes
				.Aggregate(baseQuery,
					(current, include) => current.Include(include));

			// modify the IQueryable to include any string-based include statements
			var secondaryResult = _context.IncludeStrings
				.Aggregate(queryableResultWithIncludes,
					(current, include) => current.Include(include));

			var query = _context.Criteria == null ? secondaryResult : secondaryResult.Where(_context.Criteria);

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

			var includes = leftContext.Includes.Concat(rightContext.Includes).ToList();
			var includeStrings = leftContext.IncludeStrings.Concat(rightContext.IncludeStrings).ToList();

			var newContext = new VisitorContext(
				newExpr,
				includes,
				includeStrings,
				sort,
				postProcess
			);

			return newContext;
		}

		protected class VisitorContext
		{
			//public Func<IQueryable<T>, IOrderedQueryable<T>> Sort;
			//public Func<IQueryable<T>, IQueryable<T>> PostProcess;

			public VisitorContext(Expression<Func<T, bool>> criteria,
				IReadOnlyCollection<Expression<Func<T, object>>> includes,
				IReadOnlyCollection<string> includeStrings,
				Func<IQueryable<T>, IOrderedQueryable<T>> sort,
				Func<IQueryable<T>, IQueryable<T>> postProcess
			)
			{
				Criteria = criteria;
				Includes = includes ?? new List<Expression<Func<T, object>>>();
				IncludeStrings = includeStrings ?? new List<string>();
				Sort = sort;
				PostProcess = postProcess;
			}

			public Expression<Func<T, bool>> Criteria { get; }
			public IReadOnlyCollection<Expression<Func<T, object>>> Includes { get; }
			public IReadOnlyCollection<string> IncludeStrings { get; }
			public Func<IQueryable<T>, IOrderedQueryable<T>> Sort { get; }
			public Func<IQueryable<T>, IQueryable<T>> PostProcess { get; }
		}
	}
}