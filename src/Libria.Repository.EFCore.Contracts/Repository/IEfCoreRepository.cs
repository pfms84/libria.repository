namespace Libria.Repository.EFCore.Contracts.Repository
{
	using System.Linq;
	using Core;

	public interface IEfCoreRepository<T> : IRepository<T>, IAsyncRepository<T> where T : class
	{
	}

	public interface IEfCoreRepository<T, in TKey1> : IRepository<T,TKey1>, IAsyncRepository<T, TKey1> where T : class
	{
	}

	public interface IEfCoreRepository<T, in TKey1, in TKey2> : IRepository<T,TKey1,TKey2>, IAsyncRepository<T, TKey1, TKey2> where T : class
	{
	}

	public interface IEfCoreRepository<T, in TKey1, in TKey2, in TKey3> : IRepository<T,TKey1,TKey2,TKey3>, IAsyncRepository<T, TKey1, TKey2, TKey3> where T : class
	{
	}
}