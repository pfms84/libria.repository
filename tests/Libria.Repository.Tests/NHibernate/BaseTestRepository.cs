namespace Libria.Repository.Tests.NHibernate
{
	using global::NHibernate;
	using Repository.NHibernate.Contracts;
	using Repository.NHibernate.Repository;

	public abstract class BaseTestRepository<TEntity, TKey> : NHibernateRepository<TEntity, TKey, ISession>
		where TEntity : class
	{
		protected BaseTestRepository(INHibernateUnitOfWork<ISession> unitOfWork) : base(unitOfWork)
		{
		}
	}
}