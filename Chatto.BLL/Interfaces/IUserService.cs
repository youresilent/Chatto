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

		UserDTO GetUserData(string userName, bool isId = false);

		List<UserDTO> GetAllUsers();

		OperationDetails AddPendingFriend(string currentUser, string friendUserName);

		OperationDetails RemovePendingFriend(string currentUser, string friendUserName);

		List<string> GetPendingFriends(string userName, bool isIncoming = true);

		OperationDetails AddFriend(string currentUser, string friendUserName);

		OperationDetails RemoveFriend(string currentUser, string friendUserName);

		List<string> GetFriends(string userName);

		OperationDetails ChangeSecondaryInfo(UserDTO newUserInfo);

		OperationDetails ChangePassword(string oldPass, string newPass, string userName);

		OperationDetails DeleteAccount(string userName);
	}
}
