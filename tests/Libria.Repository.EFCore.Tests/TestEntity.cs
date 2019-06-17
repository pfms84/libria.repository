namespace Libria.Repository.EFCore.Tests
{
	using System.Collections.Generic;

	public class TestEntity
	{
		public int Id { get; set; }
		public string Prop1 { get; set; }
		public virtual ICollection<TestEntityNavigation> EntityNavigations { get; set; }
		public virtual ICollection<TestEntityNavigation2> EntityNavigations2 { get; set; }
	}
}