namespace Libria.Repository.EFCore.Specifications
{
	using System;
	using Contracts.Specifications;

	public class EfCoreWithIncludeStringsSpecification<T> : EfCoreSpecification<T>,
		IEfCoreWithIncludeStringsSpecification<T>
	{
		public EfCoreWithIncludeStringsSpecification(IEfCoreSpecification<T> inner, params string[] includeStrings)
		{
			Inner = inner;
			IncludeStrings = includeStrings ?? Array.Empty<string>();
		}

		public IEfCoreSpecification<T> Inner { get; }
		public string[] IncludeStrings { get; }
	}
}