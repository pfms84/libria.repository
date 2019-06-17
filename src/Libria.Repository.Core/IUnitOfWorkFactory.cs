namespace Libria.Repository.Core
{
	public interface IUnitOfWorkFactory<out T> where T : IUnitOfWork
	{
		T Create();
	}
}