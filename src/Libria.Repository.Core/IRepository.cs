namespace Libria.Repository.Core
{
	using System.Collections.Generic;
	using Specification;

	public interface IRepository<TEntity, in TKey> where TEntity : class
	{
		TEntity GetById(TKey id);

		TEntity Add(TEntity entity);

		TEntity Update(TEntity entity);

		void AddRange(IEnumerable<TEntity> entities);

		TEntity Remove(TEntity entity);

		void RemoveRange(IEnumerable<TEntity> entities);

		int Count(ISpecification<TEntity> specification);

		TEntity Find(ISpecification<TEntity> specification);

		IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification);
	}
}