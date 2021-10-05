using System;

namespace Chatto.DAL.Interfaces
{
	public interface IDBManager : IDisposable
	{
		void Create<T>(T item);

		void Remove<T>(T item);
	}
}
