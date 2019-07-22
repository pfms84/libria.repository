namespace Libria.Repository.Tests
{
	using System;
	using FluentNHibernate.Cfg;
	using global::NHibernate;
	using Microsoft.EntityFrameworkCore;
	using Prep;

	public class PrepFixture: IDisposable
	{
		public PrepFixture()
		{
			PrepDbContext = new PrepDbContext();
			PrepDbContext.Database.EnsureDeleted();
			PrepDbContext.Database.EnsureCreated();
			PrepDbContext.Database.Migrate();
		}

		public PrepDbContext PrepDbContext { get; }
		public void Dispose()
		{
			PrepDbContext?.Dispose();
		}
	}
}