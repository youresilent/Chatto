using System;

namespace Chatto.DAL.Interfaces
{
	public interface IDBManager : IDisposable
	{
		void Create<T>(T item) where T : class;

		void Remove<T>(T item) where T : class;
	}
}
