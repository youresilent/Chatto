using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.DAL.Interfaces
{
	public interface IDBManager : IDisposable
	{
		void Create<T>(T item);

		void Remove<T>(T item);

	}
}
