namespace Libria.Repository.EFCore.Tests.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class v002 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				"TestEntity",
				new[] {"Id", "Prop1"},
				new object[,]
				{
					{5, "Prop1"},
					{6, "Prop1"},
					{7, "Prop1"},
					{8, "Prop1"},
					{9, "Prop1"},
					{10, "Prop1"},
					{11, "Prop1"},
					{12, "Prop1"},
					{13, "Prop1"},
					{14, "Prop1"},
					{15, "Prop1"},
					{16, "Prop1"}
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				5);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				6);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				7);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				8);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				9);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				10);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				11);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				12);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				13);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				14);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				15);

			migrationBuilder.DeleteData(
				"TestEntity",
				"Id",
				16);
		}
	}
}