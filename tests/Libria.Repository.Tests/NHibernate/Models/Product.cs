namespace Libria.Repository.Tests.NHibernate.Models
{
	using Seedwork;

	public class Product : Entity<int>, IAggregateRoot
	{
		protected Product() { }

		public virtual string Name { get; protected set; }
		public virtual decimal ReferencePrice { get; protected set; }
	}
}