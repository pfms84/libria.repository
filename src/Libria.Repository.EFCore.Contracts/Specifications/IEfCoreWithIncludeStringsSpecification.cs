namespace Libria.Repository.EFCore.Contracts.Specifications
{
	public interface IEfCoreWithIncludeStringsSpecification<T> : IEfCoreSpecification<T>
	{
		IEfCoreSpecification<T> Inner { get; }
		string[] IncludeStrings { get; }
	}
}