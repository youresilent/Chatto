using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chatto.BLL.Interfaces
{
	public interface IUserService : IDisposable
	{
		Task<OperationDetails> Create(UserDTO userDTO);
		Task<ClaimsIdentity> Authenticate(UserDTO userDTO);
		Task SetInitialData(UserDTO adminDTO, List<string> roles);
	}
}
