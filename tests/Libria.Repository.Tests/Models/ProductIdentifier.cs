namespace Libria.Repository.Tests.Models
{
	using System.Collections.Generic;
	using Seedwork;

	public class ProductIdentifier : ValueObject
	{
		protected ProductIdentifier() { }

		public ProductIdentifier(int id)
		{
			Id = id;
		}

		public virtual int Id { get; protected set; }
		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Id;
		}
	}
}