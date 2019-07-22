namespace Libria.Repository.Tests
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using FluentNHibernate.Cfg;
	using global::NHibernate;
	using Microsoft.EntityFrameworkCore;
	using Prep;
	using Xunit;

	public class PrepFixture: IAsyncLifetime
	{
		public static Mutex Lock { get; } = new Mutex();

		public PrepFixture()
		{
		}

		public PrepDbContext PrepDbContext { get; private set; }
		
		public async Task InitializeAsync()
		{
			Lock.WaitOne();

			PrepDbContext = new PrepDbContext();
			PrepDbContext.Database.EnsureDeleted();
			PrepDbContext.Database.EnsureCreated();
			PrepDbContext.Database.Migrate();
		}

		public async Task DisposeAsync()
		{
			PrepDbContext?.Dispose();
			Lock.ReleaseMutex();
		}
	}
}