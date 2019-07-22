namespace Libria.Repository.NHibernate.Contracts
{
	using Core;
	using global::NHibernate;

	public interface INHibernateUnitOfWork<out TSession>: IUnitOfWork
		where TSession: ISession
	{
		TSession Session { get; }
	}
}