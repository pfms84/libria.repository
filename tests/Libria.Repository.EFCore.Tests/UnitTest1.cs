namespace Libria.Repository.EFCore.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Core.Extensions;
	using Core.Specification;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class UnitTest1
	{
		private static TestUnitOfWorkFactory _uowFactory;

		[TestInitialize]
		public void Initialize()
		{
			_uowFactory = new TestUnitOfWorkFactory();
			_uowFactory.ResetDatabase();
		}

		[TestMethod]
		public async Task TestMethod1()
		{
			var res = await _uowFactory.ExecuteAndCommitAsync(async uow =>
			{
				var spec = new GetAllSpecification()
					.Page(1, 2)
					.OrderBy(e => e.Id);

				var result = await uow.TestEntityEfCoreRepository.FindAllAsync(spec);

				var testEntities = result as IList<TestEntity> ?? result.ToList();
			    return testEntities;
			});

		    Assert.IsTrue(res.Any(e => e.Id == 1 || e.Id == 2) && res.Count() == 2);
        }

        [TestMethod]
		public async Task TestMethod2()
        {
	        var prop = Guid.NewGuid().ToString();

			await _uowFactory.ExecuteAndCommitAsync(async uow =>
	        {
		        await uow.TestEntityEfCoreRepository.AddAsync(new TestEntity
		        {
			        Prop1 = prop
		        });


			});


	        await _uowFactory.ExecuteAndCommitAsync(async uow =>
	        {
		        var result = await uow.TestEntityEfCoreRepository.FindAsync(new WithPropStartingWithSpecification(prop));

		        Assert.IsTrue(result != null);

		        await uow.TestEntityEfCoreRepository.RemoveAsync(result);

			});

	        await _uowFactory.ReadAsync(async uow =>
	        {
		        var result = await uow.TestEntityEfCoreRepository.FindAsync(new WithPropStartingWithSpecification(prop));
		        Assert.IsTrue(result == null);
			});
		}

		[TestMethod]
		public async Task TestIncludes()
		{
			await _uowFactory.ExecuteAndCommitAsync(async uow =>
			{
				var id1Spec = new WithIdSpecification(1)
					.WithIncludes(e => e.EntityNavigations)
					.WithIncludes(e => e.EntityNavigations2);

				var id2Spec = new WithIdSpecification(2)
					.WithIncludes(e => e.EntityNavigations2);

				var e1 = await uow.TestEntityEfCoreRepository.FindAllAsync(id1Spec.Or(id2Spec));

				Assert.IsTrue(e1.Any());
			});
		}

		[TestMethod]
		public async Task TestRollback()
		{
			var prop1 = Guid.NewGuid().ToString();

			try
			{
				await _uowFactory.ExecuteAndCommitAsync(async uow =>
				{
					var ne = new TestEntity
					{
						Prop1 = prop1
					};

					await uow.TestEntityEfCoreRepository.AddAsync(ne);

					throw new Exception();
				});
			}
			catch (Exception)
			{
				await _uowFactory.ExecuteAndCommitAsync(async uow =>
				{
					var e1 = await uow.TestEntityEfCoreRepository.FindAsync(new WithPropStartingWithSpecification(prop1));
					Assert.IsNull(e1);
				});
			}
		}

		[TestMethod]
		public async Task TestNegateSpecification()
		{
			using (var uow = _uowFactory.Create())
			{
				var spec = new WithIdSpecification(1).Negate();
				var result = await uow.TestEntityEfCoreRepository.FindAllAsync(spec);

				Assert.IsNull(result.SingleOrDefault(e => e.Id == 1));
			}
		}
	}
}