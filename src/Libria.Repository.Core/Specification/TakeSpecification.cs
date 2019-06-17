namespace Libria.Repository.Core.Specification
{
	public class TakeSpecification<T> : BaseSpecification<T>, ITakeSpecification<T>
	{
		public TakeSpecification(ISpecification<T> inner, int take)
		{
			Inner = inner;
			Amount = take;
		}

		public ISpecification<T> Inner { get; }

		public int Amount { get; }
	}
}