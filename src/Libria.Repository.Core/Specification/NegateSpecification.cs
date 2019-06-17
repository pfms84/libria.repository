namespace Libria.Repository.Core.Specification
{
	public class NegateSpecification<T> : BaseSpecification<T>, INegateSpecification<T>
	{
		public NegateSpecification(ISpecification<T> inner)
		{
			Inner = inner;
		}

		public ISpecification<T> Inner { get; }
	}
}