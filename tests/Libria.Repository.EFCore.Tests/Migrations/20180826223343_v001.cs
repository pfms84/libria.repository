namespace Libria.Repository.EFCore.Tests.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class v001 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				"TestEntity",
				new[] {"Id", "Prop1"},
				new object[,]
				{
					{1, "Prop1"},
					{2, "Prop1"},
					{3, "Prop1_3"},
					{4, "Prop1_4"}
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				1);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				2);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				3);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				4);
		}
	}
}