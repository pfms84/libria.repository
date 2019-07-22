namespace Libria.Repository.NHibernate.Contracts
{
	using Core;
	using global::NHibernate;

	public interface INHibernateUnitOfWork: IUnitOfWork
	{
		ISession Session { get; }
	}
}