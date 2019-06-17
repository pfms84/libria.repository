namespace Libria.Repository.Core.Specification
{
	public class SkipSpecification<T> : BaseSpecification<T>, ISkipSpecification<T>
	{
		public SkipSpecification(ISpecification<T> inner, int skip)
		{
			Inner = inner;
			Amount = skip;
		}

		public ISpecification<T> Inner { get; }

		public int Amount { get; }
	}
}