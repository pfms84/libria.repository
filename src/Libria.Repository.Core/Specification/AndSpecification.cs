namespace Libria.Repository.Core.Specification
{
	public class AndSpecification<T> : BaseSpecification<T>, IAndSpecification<T>
	{
		public AndSpecification(ISpecification<T> left, ISpecification<T> right)
		{
			Left = left;
			Right = right;
		}

		public ISpecification<T> Left { get; }
		public ISpecification<T> Right { get; }
	}
}