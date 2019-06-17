namespace Libria.Repository.Core.Specification
{
	public interface ISpecificationVisitor<T>
	{
		void Visit(ISpecification<T> specification);
	}
}