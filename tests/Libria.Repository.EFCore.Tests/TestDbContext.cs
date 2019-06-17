namespace Libria.Repository.EFCore.Tests
{
	using Microsoft.EntityFrameworkCore;

	public class TestDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
				optionsBuilder.UseSqlServer(@"Server=.\SQLExpress2016;Database=TestDb;Trusted_Connection=True;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TestEntity>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasMany(e => e.EntityNavigations)
					.WithOne(e => e.Entity)
					.HasForeignKey(e => e.EntityId);
			});

			modelBuilder.Entity<TestEntityNavigation>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.Entity)
					.WithMany(e => e.EntityNavigations);
			});

			modelBuilder.Entity<TestEntityNavigation2>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.Entity)
					.WithMany(e => e.EntityNavigations2);
			});

			modelBuilder.Entity<TestEntity>()
				.HasData(new TestEntity
					{
						Id = 1,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 2,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 3,
						Prop1 = "Prop1_3"
					}, new TestEntity
					{
						Id = 4,
						Prop1 = "Prop1_4"
					},
					new TestEntity
					{
						Id = 5,
						Prop1 = "Prop1"
					},
					new TestEntity
					{
						Id = 6,
						Prop1 = "Prop1"
					},
					new TestEntity
					{
						Id = 7,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 8,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 9,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 10,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 11,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 12,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 13,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 14,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 15,
						Prop1 = "Prop1"
					}, new TestEntity
					{
						Id = 16,
						Prop1 = "Prop1"
					});

			modelBuilder.Entity<TestEntityNavigation>()
				.HasData(new TestEntityNavigation
				{
					Id = 1,
					Prop1 = "NavigationProp1",
					EntityId = 1
				});

			modelBuilder.Entity<TestEntityNavigation2>()
				.HasData(new TestEntityNavigation2
				{
					Id = 1,
					Prop1 = "Navigation2Prop1",
					EntityId = 1
				});
		}
	}
}