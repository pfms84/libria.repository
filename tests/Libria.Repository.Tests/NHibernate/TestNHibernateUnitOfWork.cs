namespace Libria.Repository.Tests.NHibernate
{
	using System.Data;
	using System.Data.Common;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using FluentNHibernate.Mapping;
	using global::NHibernate;
	using global::NHibernate.Dialect;
	using global::NHibernate.Driver;
	using global::NHibernate.SqlTypes;
	using Models;
	using Repository.NHibernate.UnitOfWork;

	public class ProductMap : ClassMap<Product>
	{
		public ProductMap()
		{
			Not.LazyLoad();

			Table("Product");
			Id(p => p.Id);
			Map(p => p.Name).Not.Nullable();
			Map(p => p.ReferencePrice);
		}	
	}

	public class CartMap : ClassMap<Cart>
	{
		public CartMap()
		{
			Not.LazyLoad();

			Table("Cart");
			Id(p => p.Id);
			Map(p => p.CreatedOn);
			HasMany(p => p.Items)
				.Access.LowerCaseField(Prefix.Underscore)
				.KeyColumn("CartId")
				.Cascade.AllDeleteOrphan();
		}
	}

	public class ItemMap : ClassMap<Item>
	{
		public ItemMap()
		{
			Not.LazyLoad();

			Table("Item");
			Id(x => x.Id);
			Map(x => x.Quantity);
			References(x => x.Cart)
				.Column("CartId");

			Component(c => c.ProductIdentifier, cfg =>
			{
				cfg.Map(x => x.Id, "ProductId");
			});

			Join("ItemPrice", x =>
			{
				x.KeyColumn("ItemId");

				x.Component(c => c.Price, cfg =>
				{
					cfg.Map(m => m.Value);
				});
			});
		}	
	}

	public class TestNHibernateUnitOfWork : NHibernateUnitOfWork
	{
		class ExtendedMsSql2012Dialect : MsSql2012Dialect
		{
			public ExtendedMsSql2012Dialect()
			{
				RegisterColumnType(DbType.UInt32, "BIGINT");
			}
		}

		class ExtendedMsSql2012ClientDriver : Sql2008ClientDriver
		{
			protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType)
			{
				if (sqlType == SqlTypeFactory.UInt32) sqlType = SqlTypeFactory.Int64;

				base.InitializeParameter(dbParam, name, sqlType);
			}
		}	

		public TestNHibernateUnitOfWork(
			IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) 
			: base(SessionFactory.OpenSession(), isolationLevel)
		{
		}

		private static ISessionFactory SessionFactory { get; }

		static TestNHibernateUnitOfWork()
		{
			//@"Server=.\SQLExpress2016;Database=LibriaRepositoryTestDb;Trusted_Connection=True;"
			SessionFactory = Fluently
				.Configure()
				.Database(MsSqlConfiguration.MsSql2012
					.ConnectionString(cs =>
					{
						cs.Database("LibriaRepositoryTestDb");
						cs.TrustedConnection();
						cs.Server(@".\SQLExpress2016");
					})
					.Dialect<ExtendedMsSql2012Dialect>()
					.Driver<ExtendedMsSql2012ClientDriver>())
				.Mappings(m =>
				{
					m.FluentMappings.AddFromAssemblyOf<TestNHibernateUnitOfWork>();
				})
				.BuildSessionFactory();
		}
	}
}