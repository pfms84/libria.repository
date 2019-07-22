namespace Libria.Repository.Tests.EfCore.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Seedwork;

	public class Cart : Entity<Guid>, IAggregateRoot
	{
		private List<Item> _items = new List<Item>();

		protected Cart() { }
		
		public virtual DateTimeOffset CreatedOn { get; protected set; }

		public virtual IReadOnlyCollection<Item> Items
		{
			get => _items.AsReadOnly();
			protected set => _items = value.ToList();
		}


		public virtual Cart AddItem(int productId, decimal price, uint quantity)
		{
			var productIdentifier = new ProductIdentifier(productId);

			if (Items.Any(i => i.ProductIdentifier == productIdentifier))
			{
				var existingItem = Items.Single(i => i.ProductIdentifier == productIdentifier);

				existingItem
					.AddQuantity(quantity)
					.UpdatePrice(new Price(price));
			}
			else
			{
				var item = new Item(this, new Price(price), productIdentifier, quantity);
				_items.Add(item);
			}

			return this;
		}
		
		public static Cart CreateCart()
		{
			return new Cart()
			{
				Id = Guid.NewGuid(),
				CreatedOn = DateTimeOffset.UtcNow
			};
		}
	}
}