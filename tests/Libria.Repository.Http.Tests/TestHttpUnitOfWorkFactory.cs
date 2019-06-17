namespace Libria.Repository.Http.Tests
{
	using Core;

	public class TestHttpUnitOfWorkFactory : IUnitOfWorkFactory<TestHttpUnitOfWork>
	{
		public TestHttpUnitOfWork Create()
		{
			return new TestHttpUnitOfWork(new HttpContext());
		}
	}
}