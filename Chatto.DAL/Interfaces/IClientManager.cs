using Chatto.DAL.Entities;
using System;

namespace Chatto.DAL.Interfaces
{
	public interface IClientManager : IDisposable
	{
		void Create(ClientProfile item);

		void Remove(ClientProfile item);
	}
}
