namespace Libria.Repository.Tests.EfCore
{
	using System;
	using System.Data;
	using System.Linq;
	using System.Linq.Expressions;

	public class TestEfCoreUnitOfWork : EFCore.UnitOfWork.EfCoreUnitOfWork
	{
		public TestEfCoreUnitOfWork( 
			IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) 
			: base(new TestDbContext(), isolationLevel)
		{
		}
	}
}