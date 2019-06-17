namespace Libria.Repository.EFCore.Contracts.Specifications
{
	using Core.Specification;

	public interface IEfCoreCriteriaSpecification<T> : ICriteriaSpecification<T>, IEfCoreSpecification<T>
	{
	}
}