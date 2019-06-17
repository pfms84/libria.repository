namespace Libria.Repository.Core
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Specification;

	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity GetById(params object[] keyValues);

		TEntity Add(TEntity entity);

		TEntity Update(TEntity entity);

		void AddRange(IEnumerable<TEntity> entities);

		TEntity Remove(TEntity entity);

		void RemoveRange(IEnumerable<TEntity> entities);

		int Count(ISpecification<TEntity> specification);

		TEntity Find(ISpecification<TEntity> specification);

		IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification);
	}

	public interface IRepository<TEntity, in TKey1>
		: IRepository<TEntity> where TEntity : class
	{
		TEntity GetById(TKey1 keyValue1);
	}

	public interface IRepository<TEntity, in TKey1, in TKey2>
		: IRepository<TEntity> where TEntity : class
	{
		TEntity GetById(TKey1 keyValue1, TKey2 keyValue2);
	}

	public interface IRepository<TEntity, in TKey1, in TKey2, in TKey3>
		: IRepository<TEntity> where TEntity : class
	{
		TEntity GetById(TKey1 keyValue1, TKey2 keyValue2, TKey3 keyValue3);
	}
}