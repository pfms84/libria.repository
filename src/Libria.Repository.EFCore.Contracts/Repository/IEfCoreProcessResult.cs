namespace Libria.Repository.EFCore.Contracts.Repository
{
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;

	public interface IEfCoreProcessResult<T>
		where T : class
	{
		Task<T> LoadRelatedEntitiesAsync(DbContext context, T entity, CancellationToken cancellationToken);
	}
}