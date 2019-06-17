namespace Libria.Repository.Core.Specification
{
	public interface IPageSpecification<T> : IUnarySpecification<T>
	{
		ISpecification<T> Inner { get; }
		int PageNumber { get; }
		int PageSize { get; }
	}
}