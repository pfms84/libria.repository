namespace Libria.Repository.Tests.Prep
{
	using Microsoft.EntityFrameworkCore;

	public class PrepDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
				optionsBuilder.UseSqlServer(@"Server=.\SQLExpress2016;Database=LibriaRepositoryTestDb;Trusted_Connection=True;");
		}
	}
}