using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chatto.BLL.Interfaces
{
	public interface IUserService : IDisposable
	{
		Task<OperationDetails> Create(UserDTO userDTO);
		Task<ClaimsIdentity> Authenticate(UserDTO userDTO);
		Task SetInitialData(UserDTO adminDTO, List<string> roles);

		UserDTO GetUserData(string userName);

		void ChangeSecondaryInfo(UserDTO newUserInfo);

		OperationDetails ChangePassword(string oldPass, string newPass, string userName);
	}
}
