namespace Libria.Repository.Core.Specification
{
	public interface ITakeSpecification<T> : IUnarySpecification<T>
	{
		int Amount { get; }
		ISpecification<T> Inner { get; }
	}
}