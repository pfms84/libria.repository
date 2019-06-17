namespace Libria.Repository.Http.Tests
{
	using System;
	using UnitOfWork;

	public class TestHttpUnitOfWork : HttpUnitOfWork
	{
		private readonly Lazy<ReqresHttpRepository> _regresHttpRepository;

		public TestHttpUnitOfWork(HttpContext httpContext) : base(httpContext)
		{
			_regresHttpRepository = new Lazy<ReqresHttpRepository>(() => new ReqresHttpRepository(httpContext));
		}

		public ReqresHttpRepository ReqresHttpRepository => _regresHttpRepository.Value;
	}
}