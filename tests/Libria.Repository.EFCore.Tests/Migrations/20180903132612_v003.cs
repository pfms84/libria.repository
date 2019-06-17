namespace Libria.Repository.EFCore.Tests.Migrations
{
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class v003 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"TestEntityNavigation",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Prop1 = table.Column<string>(nullable: true),
					EntityId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TestEntityNavigation", x => x.Id);
					table.ForeignKey(
						"FK_TestEntityNavigation_TestEntity_EntityId",
						x => x.EntityId,
						"TestEntity",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				"TestEntityNavigation",
				new[] {"Id", "EntityId", "Prop1"},
				new object[] {1, 1, "NavigationProp1"});

			migrationBuilder.CreateIndex(
				"IX_TestEntityNavigation_EntityId",
				"TestEntityNavigation",
				"EntityId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"TestEntityNavigation");
		}
	}
}