using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.BLL.Infrastructure
{
	class OperationDetails
	{
		public bool Succeeded { get; private set; }
		public string Message { get; private set; }
		public string Property { get; private set; }

		public OperationDetails (bool succeeded, string message, string property)
		{
			Succeeded = succeeded;
			Message = message;
			Property = property;
		}
	}
}
