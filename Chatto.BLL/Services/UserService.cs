using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.BLL.Interfaces;
using Chatto.DAL.Entities;
using Chatto.DAL.Identity;
using Chatto.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Chatto.BLL.Services
{
	public class UserService : IUserService
	{
		IUnitOfWork DataBase { get; set; }

		public UserService(IUnitOfWork uow)
		{
			DataBase = uow;
		}

		public async Task<ClaimsIdentity> Authenticate(UserDTO userDTO)
		{
			ClaimsIdentity claims = null;
			var user = await DataBase.UserManager.FindAsync(userDTO.UserName, userDTO.Password);

			if (user != null)
			{
				claims = await DataBase.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			}

			return claims;
		}

		public async Task<OperationDetails> Create(UserDTO userDTO)
		{
			CheckRoles();

			var user = await DataBase.UserManager.FindByEmailAsync(userDTO.Email);
			if (user != null)
			{
				return new OperationDetails(false, StatusStringsResource.Registration_Failure, "UserName");
			}

			user = new ApplicationUser { Id = Guid.NewGuid(), Email = userDTO.Email, UserName = userDTO.UserName };

			var result = await DataBase.UserManager.CreateAsync(user, userDTO.Password);

			if (result.Errors.Count() > 0)
			{
				return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
			}

			var clientProfile = new ClientProfile
			{
				Adress = userDTO.Adress,
				Age = userDTO.Age,
				Gender = userDTO.Gender,
				Id = user.Id,
				RealName = userDTO.RealName,
			};

			try
			{
				await DataBase.UserManager.AddToRoleAsync(user.Id, userDTO.Role);
				DataBase.DBManager.Create(clientProfile);

				await DataBase.SaveAsync();
			}
			catch
            {
				DataBase.UserManager.Delete(user);
				DataBase.UserManager.RemoveFromRole(user.Id, userDTO.Role);

				return new OperationDetails(false, StatusStringsResource.Registration_CreatingUserFailure, "");
            }

			return new OperationDetails(true, StatusStringsResource.Registration_OK, "");
		}

		public OperationDetails DeleteAccount(string userName)
		{
			var user = DataBase.UserManager.FindByName(userName);

			if (user == null)
            {
				return new OperationDetails(false, StatusStringsResource.AccountRemoval_Failure, userName);
			}

			var userFriends = user.ClientProfile.ClientFriends.ToList();
			if (userFriends.Count != 0)
            {
				foreach (var friend in userFriends)
				{
					var userData = GetUserData(Convert.ToString(friend.Friend_Id2), true);
					RemoveFriend(userData.UserName, userName);
				}
			}

			DataBase.DBManager.Remove(user.ClientProfile);

			var operation = DataBase.UserManager.Delete(user);

			if (!operation.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.AccountRemoval_Failure, userName);
			}

			return new OperationDetails(true, StatusStringsResource.AccountRemoval_OK, "");
		}

		public OperationDetails AddPendingFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			if (user == null)
			{
				return new OperationDetails(false, StatusStringsResource.UserNotFound, currentUser);
			}
			
			if (friend == null)
			{
				return new OperationDetails(false, StatusStringsResource.FriendNotFound, friendUserName);
			}

			var clientPendingFriend = new ClientPendingFriend
			{
				Id_Sender = user.Id,
				Id_Receiver = friend.Id,
			};

			var userCurrentSenderPendingFriends = user.ClientProfile.ClientSenderPendingFriends;
			var friendCurrentReceiverPendingFriends = friend.ClientProfile.ClientReceiverPendingFriends;

			try
			{
				user.ClientProfile.ClientSenderPendingFriends.Add(clientPendingFriend);
				friend.ClientProfile.ClientReceiverPendingFriends.Add(clientPendingFriend);
			}
			catch
            {
				user.ClientProfile.ClientSenderPendingFriends = userCurrentSenderPendingFriends;
				friend.ClientProfile.ClientReceiverPendingFriends = friendCurrentReceiverPendingFriends;

				return new OperationDetails(false, StatusStringsResource.AddPendingFriend_AddFailure, "");
			}

			var operation1 = DataBase.UserManager.Update(friend);
			var operation2 = DataBase.UserManager.Update(user);

			if (!operation1.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.AddPendingFriend_FailureOP1, friendUserName);
			}

			if (!operation2.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.AddPendingFriend_FailureOP2, currentUser);
			}

			return new OperationDetails(true, StatusStringsResource.AddPendingFriend_OK, "");
		}

		public OperationDetails RemovePendingFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			if (user == null)
			{
				return new OperationDetails(false, StatusStringsResource.UserNotFound, currentUser);
			}

			if (friend == null)
			{
				return new OperationDetails(false, StatusStringsResource.FriendNotFound, friendUserName);
			}

			try
			{
				var tableRecord = user.ClientProfile.ClientReceiverPendingFriends
				.Where(w => w.Id_Sender == friend.Id && w.Id_Receiver == user.Id)
				.SingleOrDefault();

				DataBase.ClientManager.RemovePendingFriend(tableRecord);
			}
			catch
            {
				return new OperationDetails(false, StatusStringsResource.RemovePendingFriend_Failure, "");
            }

			return new OperationDetails(true, StatusStringsResource.RemovePendingFriend_OK, "");
		}

		public List<Guid> GetPendingFriends(string userName, bool isIncoming = true)
		{
			var pendingFriendsList = new List<Guid>();
			
			var user = DataBase.UserManager.FindByName(userName);

			if (user == null)
            {
				return pendingFriendsList;
            }

			if (isIncoming)
			{
				var userIncomingPendingFriends = user.ClientProfile.ClientReceiverPendingFriends.ToList();
				if (userIncomingPendingFriends.Count != 0)
                {
					foreach (var item in userIncomingPendingFriends)
					{
						pendingFriendsList.Add(item.Id_Sender);
					}
				}
			}
			else
			{
				var userOutgoingPendingFriends = user.ClientProfile.ClientSenderPendingFriends.ToList();
				if (userOutgoingPendingFriends.Count != 0)
                {
					foreach (var item in userOutgoingPendingFriends)
					{
						pendingFriendsList.Add(item.Id_Receiver);
					}
                }
			}
			
			return pendingFriendsList;
		}

		public OperationDetails AddFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			if (user == null)
			{
				return new OperationDetails(false, StatusStringsResource.UserNotFound, currentUser);
			}

			if (friend == null)
			{
				return new OperationDetails(false, StatusStringsResource.FriendNotFound, friendUserName);
			}

			var userCurrentFriends = user.ClientProfile.ClientFriends;
			var friendCurrentFriends = friend.ClientProfile.ClientFriends;

			try
            {
				user.ClientProfile.ClientFriends.Add(new ClientFriend { Friend_Id1 = user.Id, Friend_Id2 = friend.Id });
				friend.ClientProfile.ClientFriends.Add(new ClientFriend { Friend_Id1 = friend.Id, Friend_Id2 = user.Id });
            }
			catch
            {
				user.ClientProfile.ClientFriends = userCurrentFriends;
				friend.ClientProfile.ClientFriends = friendCurrentFriends;

				return new OperationDetails(false, StatusStringsResource.AddFriend_AddFailure, "");
            }

			var removeOperation = RemovePendingFriend(currentUser, friendUserName);

			if (!removeOperation.Succeeded)
			{
				return new OperationDetails(false, removeOperation.Message, removeOperation.Property);
			}

			var operation1 = DataBase.UserManager.Update(user);
			var operation2 = DataBase.UserManager.Update(friend);

			if (!operation1.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.AddFriend_FailureOP1, currentUser);
			}

			if (!operation2.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.AddFriend_FailureOP2, friendUserName);
			}

			return new OperationDetails(true, StatusStringsResource.AddFriend_OK, "");
		}

		public OperationDetails RemoveFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			if (user == null)
			{
				return new OperationDetails(false, StatusStringsResource.UserNotFound, currentUser);
			}

			if (friend == null)
			{
				return new OperationDetails(false, StatusStringsResource.FriendNotFound, friendUserName);
			}

			ClientFriend friendEntry1;
			ClientFriend friendEntry2;

			try
			{
				friendEntry1 = DataBase.DBManager.FindClientFriend(user.Id, friend.Id);
				friendEntry2 = DataBase.DBManager.FindClientFriend(friend.Id, user.Id);
			}
			catch
            {
				return new OperationDetails(false, StatusStringsResource.RemoveFriend_DBEntryNotFound, "");
            }

			try
			{
				DataBase.DBManager.Remove(friendEntry2, friendEntry2.Id);
				DataBase.DBManager.Remove(friendEntry1, friendEntry1.Id);
			}
			catch
            {
				return new OperationDetails(false, StatusStringsResource.RemoveFriend_RemoveFailure, "");
			}

			var operation1 = DataBase.UserManager.Update(user);
			var operation2 = DataBase.UserManager.Update(friend);

			if (!operation1.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.RemoveFriend_FailureOP1, currentUser);
			}

			if (!operation2.Succeeded)
			{
				return new OperationDetails(false, StatusStringsResource.RemoveFriend_FailureOP2, friendUserName);
			}

			return new OperationDetails(true, StatusStringsResource.RemoveFriend_OK, "");
		}

		public List<Guid> GetFriends(string userName)
		{
			var friendsList = new List<Guid>();

			var user = DataBase.UserManager.FindByName(userName);
			if (user == null)
            {
				return friendsList;
            }

			var userFriends = user.ClientProfile.ClientFriends.ToList();
			if (userFriends.Count != 0)
            {
				foreach (var item in user.ClientProfile.ClientFriends.ToList())
				{
					friendsList.Add(item.Friend_Id2);
				}
            }

			return friendsList;
		}

		public UserDTO GetUserData(string userName, bool isId = false)
		{
			ApplicationUser tempUser;
			UserDTO user = new UserDTO();

			if (isId)
			{
				tempUser = DataBase.UserManager.FindById(Guid.Parse(userName));
			}
			else
			{
				tempUser = DataBase.UserManager.FindByName(userName);
			}

			if (tempUser != null)
            {
				user = new UserDTO
				{
					Id = tempUser.Id,
					UserName = tempUser.UserName,
					Email = tempUser.Email,
					Adress = tempUser.ClientProfile.Adress,
					Gender = tempUser.ClientProfile.Gender,
					Age = tempUser.ClientProfile.Age,
					RealName = tempUser.ClientProfile.RealName,
				};
            }

			return user;
		}

		public List<UserDTO> GetAllUsers()
		{
			var list = new List<UserDTO>();
			var users = DataBase.UserManager.Users.ToList();

			if (users.Count != 0)
            {
				foreach (var item in users)
				{
					list.Add(GetUserData(item.UserName));
				}
			}

			return list;
		}

		public OperationDetails ChangeSecondaryInfo(UserDTO newUserInfo)
		{
			var currentUser = DataBase.UserManager.FindByName(newUserInfo.UserName);
			if (currentUser == null)
            {
				return new OperationDetails(false, StatusStringsResource.UserNotFound, "");
            }

			currentUser.ClientProfile.Adress = newUserInfo.Adress;
			currentUser.ClientProfile.Age = newUserInfo.Age;
			currentUser.Email = newUserInfo.Email;
			currentUser.ClientProfile.Gender = newUserInfo.Gender;
			currentUser.ClientProfile.RealName = newUserInfo.RealName;

			var operation = DataBase.UserManager.Update(currentUser);

			if (operation.Succeeded)
			{
				return new OperationDetails(true, StatusStringsResource.ChangeSecondaryInfo_OK, "");
			}
			else
			{
				return new OperationDetails(false, StatusStringsResource.ChangeSecondaryInfo_Failure, "");
			}
		}

		public OperationDetails ChangePassword(string oldPass, string newPass, Guid id)
		{
			var user = DataBase.UserManager.FindById(id);
			if (user == null)
            {
				return new OperationDetails(false, StatusStringsResource.UserNotFound, "");
            }

			if (oldPass == newPass)
			{
				return new OperationDetails(false, StatusStringsResource.ChangePassword_SameOldNewPasswordError, oldPass);
			}

			if (!DataBase.UserManager.CheckPassword(user, oldPass))
			{
				return new OperationDetails(false, StatusStringsResource.ChangePassword_IncorrectOldPasswordError, oldPass);
			}

			try
			{
				DataBase.UserManager.ChangePassword(id, oldPass, newPass);
			}
			catch
            {
				return new OperationDetails(false, StatusStringsResource.ChangePassword_UpdateFailed, "");
            }

			return new OperationDetails(true, StatusStringsResource.ChangePassword_OK, "");
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}

		public async Task SetInitialData(UserDTO adminDTO, List<string> roles)
		{
			foreach (var roleName in roles)
			{
				var role = DataBase.RoleManager.FindByName(roleName);

				if (role == null)
				{
					role = new CustomRole { Name = roleName };
					DataBase.RoleManager.Create(role);
				}

				await Create(adminDTO);
			}
		}

		private void CheckRoles()
		{
			var userRole = DataBase.RoleManager.FindByName("user");
			var adminRole = DataBase.RoleManager.FindByName("admin");

			if (userRole == null)
			{
				userRole = new CustomRole { Id = Guid.NewGuid(), Name = "user" };
				DataBase.RoleManager.Create(userRole);
			}

			if (adminRole == null)
			{
				adminRole = new CustomRole { Id = Guid.NewGuid(), Name = "admin" };
				DataBase.RoleManager.Create(adminRole);
			}
		}
	}
}
