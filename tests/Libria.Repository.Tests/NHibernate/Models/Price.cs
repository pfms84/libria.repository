namespace Libria.Repository.Tests.NHibernate.Models
{
	using System.Collections.Generic;
	using Seedwork;

	public class Price : ValueObject
	{
		protected Price() { }

		public Price(decimal value)
		{
			Value = value;
		}

		public virtual decimal Value { get; protected set; }

		protected override IEnumerable<object> GetAtomicValues()
		{
			yield return Value;
		}
	}
}