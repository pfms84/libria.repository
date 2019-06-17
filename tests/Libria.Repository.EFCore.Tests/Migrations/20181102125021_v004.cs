namespace Libria.Repository.EFCore.Tests.Migrations
{
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class v004 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"TestEntityNavigation2",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Prop1 = table.Column<string>(nullable: true),
					EntityId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TestEntityNavigation2", x => x.Id);
					table.ForeignKey(
						"FK_TestEntityNavigation2_TestEntity_EntityId",
						x => x.EntityId,
						"TestEntity",
						"Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				"TestEntityNavigation2",
				new[] {"Id", "EntityId", "Prop1"},
				new object[] {1, 1, "Navigation2Prop1"});

			migrationBuilder.CreateIndex(
				"IX_TestEntityNavigation2_EntityId",
				"TestEntityNavigation2",
				"EntityId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"TestEntityNavigation2");
		}
	}
}