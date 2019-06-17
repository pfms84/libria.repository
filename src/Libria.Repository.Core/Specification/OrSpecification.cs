namespace Libria.Repository.Core.Specification
{
	public class OrSpecification<T> : BaseSpecification<T>, IOrSpecification<T>
	{
		public OrSpecification(
			ISpecification<T> left,
			ISpecification<T> right)
		{
			Left = left;
			Right = right;
		}

		public ISpecification<T> Left { get; }
		public ISpecification<T> Right { get; }
	}
}