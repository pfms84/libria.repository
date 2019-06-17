using Xunit;

namespace Libria.Repository.Http.Tests
{
	using System;
	using System.Threading.Tasks;
	using Core.Extensions;

	public class Fixture : IDisposable
	{
		public Fixture()
		{
			UnitOfWorkFactory = new TestHttpUnitOfWorkFactory();
		}

		public TestHttpUnitOfWorkFactory UnitOfWorkFactory { get; set; }

		public void Dispose()
		{
		}
	}

	public class UnitTest1 : IClassFixture<Fixture>
	{
		private TestHttpUnitOfWorkFactory _unitOfWorkFactory;

		public UnitTest1(Fixture fixture)
		{
			_unitOfWorkFactory = fixture.UnitOfWorkFactory;
		}

		[Fact]
        public async Task Test1()
		{
			await _unitOfWorkFactory.ReadAsync(async uow =>
			{
				var user = await uow.ReqresHttpRepository.GetByIdAsync(2);
				Assert.NotNull(user);
			});
		}

		[Fact]
		public async Task Test2()
		{
			await _unitOfWorkFactory.ExecuteAndCommitAsync(async uow =>
			{
				var user = await uow.ReqresHttpRepository.AddAsync(new User()
				{
					Job = "Leader",
					Name = "Morpheus"
				});

				Assert.NotNull(user.Id);
			});
		}
	}
}
