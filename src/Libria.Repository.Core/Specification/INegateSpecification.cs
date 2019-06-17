namespace Libria.Repository.Core.Specification
{
	public interface INegateSpecification<T> : IUnarySpecification<T>
	{
		ISpecification<T> Inner { get; }
	}
}