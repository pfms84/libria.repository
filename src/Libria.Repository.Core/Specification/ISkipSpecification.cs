namespace Libria.Repository.Core.Specification
{
	public interface ISkipSpecification<T> : IUnarySpecification<T>
	{
		int Amount { get; }
		ISpecification<T> Inner { get; }
	}
}