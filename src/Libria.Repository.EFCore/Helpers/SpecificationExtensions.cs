namespace Libria.Repository.EFCore.Helpers
{
	using System;
	using Contracts.Specifications;
	using Core.Specification;

	public static class SpecificationExtensions
	{
		public static IEfCoreSpecification<T> ToEfCoreSpecification<T>(this ISpecification<T> spec) where T : class
		{
			if (!(spec is IEfCoreSpecification<T> efCoreSpec))
				throw new Exception($"{spec.GetType()} must be assignable to {typeof(IEfCoreSpecification<T>)}");

			return efCoreSpec;
		}
	}
}