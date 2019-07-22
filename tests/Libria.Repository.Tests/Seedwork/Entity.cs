namespace Libria.Repository.Tests.Seedwork
{
	public abstract class Entity<T> : IEntity
	{
		private int? _requestedHashCode;
		public virtual T Id { get; protected set; }

		public virtual bool IsTransient()
		{
			return Id.Equals(default(T));
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Entity<T>))
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (GetType() != obj.GetType())
				return false;

			var item = (Entity<T>)obj;

			if (item.IsTransient() || IsTransient())
				return false;
			return item.Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyMemberInGetHashCode
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			if (IsTransient()) return base.GetHashCode();

			if (!_requestedHashCode.HasValue)
				_requestedHashCode =
					Id.GetHashCode() ^
					31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

			return _requestedHashCode.Value;
			// ReSharper restore NonReadonlyMemberInGetHashCode

		}

		public static bool operator ==(Entity<T> left, Entity<T> right)
		{
			return Equals(left, null) ? Equals(right, null) : left.Equals(right);
		}

		public static bool operator !=(Entity<T> left, Entity<T> right)
		{
			return !(left == right);
		}
	}
}