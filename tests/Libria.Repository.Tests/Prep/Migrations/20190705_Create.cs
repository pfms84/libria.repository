namespace Libria.Repository.Tests.Prep.Migrations
{
	using System;
	using Microsoft.EntityFrameworkCore.Infrastructure;
	using Microsoft.EntityFrameworkCore.Metadata;
	using Microsoft.EntityFrameworkCore.Migrations;

	[DbContext(typeof(PrepDbContext))]
	[Migration("20190705_Create")]
	public class Create : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"Product",
				table => new
				{
					Id = table.Column<int>(nullable: false),
					Name = table.Column<string>(nullable: false),
					ReferencePrice = table.Column<decimal>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Product", x => x.Id);
				});

			migrationBuilder.CreateTable(
				"Cart",
				table => new
				{
					Id = table.Column<Guid>(nullable: false),
					CreatedOn = table.Column<DateTimeOffset>(nullable: false),
				},
				constraints: table => { table.PrimaryKey("PK_Cart", x => x.Id); });

			migrationBuilder.CreateTable(
				"Item",
				table => new
				{
					Id = table.Column<Guid>(nullable: false),
					ProductId = table.Column<int>(nullable: false),
					Quantity = table.Column<uint>(nullable: false),
					CartId = table.Column<Guid>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Item", x => x.Id);
					table.ForeignKey("FK_Item_Product_Id", x => x.ProductId, "Product", "Id");
					table.ForeignKey("FK_Item_Cart_Id", x => x.CartId, "Cart", "Id");
				});


			migrationBuilder.CreateTable(
				"ItemPrice",
				table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Value = table.Column<decimal>(nullable: false),
					ItemId = table.Column<Guid>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ItemPrice", x => x.Id);
					table.ForeignKey("FK_ItemPrice_Item", x => x.ItemId, "Item", "Id");
				});

			migrationBuilder.InsertData(
				"Product",
				new[] {"Id", "Name", "ReferencePrice"},
				new object[,]
				{
					{1, "Product1", 1},
					{2, "Product2", 2},
					{3, "Product3", 3},
					{4, "Product4", 4}
				});

			migrationBuilder.InsertData(
				"Cart",
				new[] { "Id", "CreatedOn" },
				new object[,]
				{
					{Guid.Empty, DateTimeOffset.Now}
				});

			migrationBuilder.InsertData(
				"Item",
				new[] { "Id", "Quantity", "CartId", "ProductId" },
				new object[,]
				{
					{Guid.Empty, 1, Guid.Empty, 1}
				});

			migrationBuilder.InsertData(
				"ItemPrice",
				new[] { "ItemId", "Value" },
				new object[,]
				{
					{Guid.Empty, 1.0}
				});
		}
	}
}