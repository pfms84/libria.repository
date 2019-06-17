namespace Libria.Repository.EFCore.Tests.Migrations
{
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"TestEntity",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Prop1 = table.Column<string>(nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_TestEntity", x => x.Id); });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"TestEntity");
		}
	}
}