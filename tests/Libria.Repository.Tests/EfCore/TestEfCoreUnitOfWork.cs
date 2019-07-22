namespace Libria.Repository.Tests.EfCore
{
	using System;
	using System.Data;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using EFCore.Contracts;
	using Microsoft.EntityFrameworkCore;

	public class TestEfCoreUnitOfWork : EFCore.UnitOfWork.EfCoreUnitOfWork<TestDbContext>
	{
		public TestEfCoreUnitOfWork( 
			IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) 
			: base(new TestDbContext(), isolationLevel)
		{
		}
	}
}