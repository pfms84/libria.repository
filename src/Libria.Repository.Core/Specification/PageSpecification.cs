namespace Libria.Repository.Core.Specification
{
	public class PageSpecification<T> : BaseSpecification<T>, IPageSpecification<T>
	{
		public PageSpecification(
			ISpecification<T> inner,
			int pageNumber,
			int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			Inner = inner;
		}

		public ISpecification<T> Inner { get; }
		public int PageNumber { get; }
		public int PageSize { get; }
	}
}