namespace Libria.Repository.Tests.EfCore
{
	using System;
	using System.Linq;
	using Models;
	using Xunit;

	public class EfCoreRepositoryTests: IClassFixture<PrepFixture>, IDisposable
	{
		private readonly PrepFixture _fixture;
		
		public EfCoreRepositoryTests(PrepFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public void Given_An_Existing_Product_A_Cart_Must_Be_Added_With_One_Item()
		{
			Guid cartId;

			using (var uow = new TestEfCoreUnitOfWork())
			{
				var productRepository = new ProductRepository(uow);
				var cartRepository = new CartRepository(uow);

				uow.BeginTransaction();

				var product = productRepository.GetById(1);

				var cart = Cart.CreateCart();
				cart.AddItem(product.Id, product.ReferencePrice, 2);

				cartRepository.Add(cart);
				uow.Commit();

				cartId = cart.Id;
			}

			using (var uow = new TestEfCoreUnitOfWork())
			{
				var cartRepository = new CartRepository(uow);

				var cart = cartRepository.GetById(cartId);
				Assert.Equal(1, cart.Items.Count);
			}
		}

		[Fact]
		public void Given_An_Existing_Cart_An_Item_Can_Have_Its_Quantity_Updated()
		{
			using (var uow = new TestEfCoreUnitOfWork())
			{
				var cartRepository = new CartRepository(uow);

				uow.BeginTransaction();

				var cart = cartRepository.GetById(Guid.Empty);
				cart.Items.Single().AddQuantity(2);

				cartRepository.Update(cart);
				uow.Commit();
			}

			using (var uow = new TestEfCoreUnitOfWork())
			{
				var cartRepository = new CartRepository(uow);

				var cart = cartRepository.GetById(Guid.Empty);
				Assert.Equal(3u, cart.Items.Single().Quantity);
			}
		}

		public void Dispose()
		{
		}
	}
}
