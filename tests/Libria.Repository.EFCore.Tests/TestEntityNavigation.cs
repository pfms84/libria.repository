namespace Libria.Repository.EFCore.Tests
{
	public class TestEntityNavigation
	{
		public int Id { get; set; }
		public string Prop1 { get; set; }
		public TestEntity Entity { get; set; }
		public int EntityId { get; set; }
	}
}