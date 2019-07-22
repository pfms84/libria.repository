namespace Libria.Repository.Tests.EfCore
{
	using System;
	using Microsoft.EntityFrameworkCore;
	using Models;

	public class TestDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(@"Server=.\SQLExpress2016;Database=LibriaRepositoryTestDb;Trusted_Connection=True;");
				optionsBuilder.UseLazyLoadingProxies();
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>(entity =>
			{
				entity.ToTable("Product");

				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired();
				entity.Property(e => e.ReferencePrice).IsRequired();
			});

			modelBuilder.Entity<Cart>(entity =>
			{
				entity.ToTable("Cart");

				entity.HasKey(e => e.Id);
				entity.Property(x => x.CreatedOn).IsRequired();
				
				entity.HasMany(e => e.Items)
					.WithOne(e => e.Cart)
					.HasForeignKey("CartId")
					.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
			});

			modelBuilder.Entity<Item>(entity =>
			{
				entity.ToTable("Item");

				entity.HasKey(e => e.Id);
				entity.Property(x => x.Quantity).IsRequired();

				entity.OwnsOne(e => e.ProductIdentifier, p =>
				{
					p.Property(e => e.Id).HasColumnName("ProductId").IsRequired();
				});

				entity.OwnsOne(e => e.Price, cfg =>
				{
					cfg.ToTable("ItemPrice");

					cfg.Property<Guid>("ItemId").IsRequired().HasColumnType("uniqueidentifier")
					.ValueGeneratedNever();

					cfg.Property<int>("Id").IsRequired();
					cfg.Property(e => e.Value).IsRequired();

					cfg.HasKey("Id");
				});
			});
		}
	}
}