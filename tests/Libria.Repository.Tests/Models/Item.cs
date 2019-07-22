namespace Libria.Repository.Tests.Models
{
	using System;
	using Seedwork;

	public class Item : Entity<Guid>
	{
		protected Item() { }

		public Item(Cart cart, Price price, ProductIdentifier productIdentifier, uint quantity)
		{
			Cart = cart;
			Price = price;
			ProductIdentifier = productIdentifier;
			Quantity = quantity;
		}

		public virtual Price Price { get; protected set; }
		public virtual ProductIdentifier ProductIdentifier { get; protected set; }
		public virtual uint Quantity { get; protected set; }
		public virtual Cart Cart { get; protected set; }

		public virtual Item AddQuantity(uint quantity)
		{
			Quantity += quantity;
			return this;
		}

		public virtual Item UpdatePrice(Price price)
		{
			Price = price;
			return this;
		}
	}
}