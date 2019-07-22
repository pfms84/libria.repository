namespace Libria.Repository.Tests.Seedwork
{
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.VisualBasic.CompilerServices;

	public abstract class ValueObject
	{
		protected static bool EqualOperator(ValueObject left, ValueObject right)
		{
			if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
				return false;
			return ReferenceEquals(left, null) || left.Equals(right);
		}

		protected static bool NotEqualOperator(ValueObject left, ValueObject right)
		{
			return !EqualOperator(left, right);
		}

		protected abstract IEnumerable<object> GetAtomicValues();

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
				return false;
			var other = (ValueObject)obj;
			using (var thisValues = GetAtomicValues().GetEnumerator())
			using (var otherValues = other.GetAtomicValues().GetEnumerator())
			{
				while (thisValues.MoveNext() && otherValues.MoveNext())
				{
					if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
						return false;
					if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
						return false;
				}
				return !thisValues.MoveNext() && !otherValues.MoveNext();
			}
		}

		public override int GetHashCode()
		{
			return GetAtomicValues()
				.Select(x => x != null ? x.GetHashCode() : 0)
				.Aggregate((x, y) => x ^ y);
		}

		public ValueObject GetCopy()
		{
			return MemberwiseClone() as ValueObject;
		}

		public static bool operator ==(ValueObject obj1, ValueObject obj2)
		{
			if (ReferenceEquals(obj1, obj2))
			{
				return true;
			}

			if (ReferenceEquals(obj1, null))
			{
				return false;
			}
			if (ReferenceEquals(obj2, null))
			{
				return false;
			}

			return obj1.Equals(obj2);
		}

		public static bool operator !=(ValueObject obj1, ValueObject obj2)
		{
			return !(obj1 == obj2);
		}
	}
}