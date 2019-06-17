namespace Libria.Repository.Core.Specification
{
	public interface IBinarySpecification<T> : ISpecification<T>
	{
		ISpecification<T> Left { get; }
		ISpecification<T> Right { get; }
	}
}