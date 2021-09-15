﻿using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.BLL.Interfaces;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using Microsoft.AspNet.Identity;
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
				claims = await DataBase.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

			return claims;
		}

		public async Task<OperationDetails> Create(UserDTO userDTO)
		{
			CheckRoles();

			ApplicationUser user = await DataBase.UserManager.FindByEmailAsync(userDTO.Email);

			if (user == null)
			{
				user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };
				var result = await DataBase.UserManager.CreateAsync(user, userDTO.Password);

				if (result.Errors.Count() > 0)
					return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

				await DataBase.UserManager.AddToRoleAsync(user.Id, userDTO.Role);

				ClientProfile clientProfile = new ClientProfile 
				{ 
					Adress = userDTO.Adress, 
					Age = userDTO.Age, 
					Gender = userDTO.Gender, 
					Id = user.Id, 
					RealName = userDTO.RealName, 
					Friends = "", 
					PendingFriendsSent = "", 
					PendingFriendsReceived = ""
				};

				DataBase.ClientManager.Create(clientProfile);

				await DataBase.SaveAsync();

				return new OperationDetails(true, "Successful registration!", "");
			}
			else
				return new OperationDetails(false, "This User name already exists!", "UserName");
		}

		public OperationDetails DeleteAccount(string userName)
		{
			ApplicationUser user = DataBase.UserManager.FindByName(userName);

			var userFriendList = StringToList(user.ClientProfile.Friends);

			foreach (var friend in userFriendList)
				RemoveFriend(friend, userName);

			DataBase.ClientManager.Remove(user.ClientProfile);

			var operation = DataBase.UserManager.Delete(user);

			if (!operation.Succeeded)
				return new OperationDetails(false, "Guru meditation. Account removal failure.", userName);

			return new OperationDetails(true, "Account deleted successfully!", "");
		}

		public OperationDetails AddPendingFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			user.ClientProfile.PendingFriendsSent += friendUserName + ",";
			friend.ClientProfile.PendingFriendsReceived += currentUser + ",";

			var operation1 = DataBase.UserManager.Update(user);
			var operation2 = DataBase.UserManager.Update(friend);

			if (!operation1.Succeeded)
				return new OperationDetails(false, "Error occured while adding pending friend! (operation1)", currentUser);

			if (!operation2.Succeeded)
				return new OperationDetails(false, "Error occured while adding pending friend! (operation2)", friendUserName);

			return new OperationDetails(true, "Added friend successfully!", "");
		}

		public OperationDetails RemovePendingFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			var userPendingFriendList = StringToList(user.ClientProfile.PendingFriendsReceived);
			userPendingFriendList.Remove(friendUserName);

			var otherFriendPendingList = StringToList(friend.ClientProfile.PendingFriendsSent);
			otherFriendPendingList.Remove(currentUser);

			user.ClientProfile.PendingFriendsReceived = ListToString(userPendingFriendList);
			friend.ClientProfile.PendingFriendsSent = ListToString(otherFriendPendingList);

			var operation1 = DataBase.UserManager.Update(user);
			var operation2 = DataBase.UserManager.Update(friend);

			if (!operation1.Succeeded)
				return new OperationDetails(false, "Error occured while removing pending friend! (operation1)", currentUser);

			if (!operation2.Succeeded)
				return new OperationDetails(false, "Error occured while removing pending friend! (operation2)", friendUserName);

			return new OperationDetails(true, "Removed pending friend successfully!", "");
		}

		public OperationDetails AddFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			var removeOperation = RemovePendingFriend(currentUser, friendUserName);

			if (!removeOperation.Succeeded)
			{
				return new OperationDetails(false, removeOperation.Message, removeOperation.Property);
			}

			user.ClientProfile.Friends += friendUserName + ",";
			friend.ClientProfile.Friends += currentUser + ",";

			var operation1 = DataBase.UserManager.Update(user);
			var operation2 = DataBase.UserManager.Update(friend);

			if (!operation1.Succeeded)
				return new OperationDetails(false, "Error occured while adding friend! (operation1)", currentUser);

			if (!operation2.Succeeded)
				return new OperationDetails(false, "Error occured while adding friend! (operation2)", friendUserName);

			return new OperationDetails(true, "Added friend successfully!", "");
		}

		public OperationDetails RemoveFriend(string currentUser, string friendUserName)
		{
			var user = DataBase.UserManager.FindByName(currentUser);
			var friend = DataBase.UserManager.FindByName(friendUserName);

			var userFriendList = StringToList(user.ClientProfile.Friends);
			userFriendList.Remove(friendUserName);

			var otherFriendList = StringToList(friend.ClientProfile.Friends);
			otherFriendList.Remove(currentUser);

			user.ClientProfile.Friends = ListToString(userFriendList);
			friend.ClientProfile.Friends = ListToString(otherFriendList);

			var operation1 = DataBase.UserManager.Update(user);
			var operation2 = DataBase.UserManager.Update(friend);

			if (!operation1.Succeeded)
				return new OperationDetails(false, "Error occured while removing friend! (operation1)", currentUser);

			if (!operation2.Succeeded)
				return new OperationDetails(false, "Error occured while removing friend! (operation2)", friendUserName);

			return new OperationDetails(true, "Removed friend successfully!", "");
		}

		public UserDTO GetUserData(string userName)
		{
			ApplicationUser tempUser = DataBase.UserManager.FindByName(userName);

			UserDTO user = new UserDTO
			{
				Id = tempUser.Id,
				UserName = tempUser.UserName,
				Email = tempUser.Email,
				Adress = tempUser.ClientProfile.Adress,
				Gender = tempUser.ClientProfile.Gender,
				Age = tempUser.ClientProfile.Age,
				RealName = tempUser.ClientProfile.RealName,
				Friends = tempUser.ClientProfile.Friends,
				PendingFriendsSent = tempUser.ClientProfile.PendingFriendsSent,
				PendingFriendsReceived = tempUser.ClientProfile.PendingFriendsReceived
			};

			return user;
		}

		public List<UserDTO> GetAllUsers()
		{
			List<UserDTO> list = new List<UserDTO>();
			var users = DataBase.UserManager.Users.ToList();

			foreach (var item in users)
			{
				list.Add(GetUserData(item.UserName));
			}

			return list;
		}

		public OperationDetails ChangeSecondaryInfo(UserDTO newUserInfo)
		{
			ApplicationUser currentUser = DataBase.UserManager.FindByName(newUserInfo.UserName);

			currentUser.ClientProfile.Adress = newUserInfo.Adress;
			currentUser.ClientProfile.Age = newUserInfo.Age;
			currentUser.Email = newUserInfo.Email;
			currentUser.ClientProfile.Gender = newUserInfo.Gender;
			currentUser.ClientProfile.RealName = newUserInfo.RealName;

			var operation = DataBase.UserManager.Update(currentUser);

			if (operation.Succeeded)
				return new OperationDetails(true, "Updated successfully!", "");
			else
				return new OperationDetails(false, "Update failed!", "");
		}

		public OperationDetails ChangePassword(string oldPass, string newPass, string id)
		{
			ApplicationUser user = DataBase.UserManager.FindById(id);

			if (oldPass == newPass)
			{
				return new OperationDetails(false, "Old and new passwords are same!", oldPass);
			}

			if (!DataBase.UserManager.CheckPassword(user, oldPass))
			{
				return new OperationDetails(false, "Incorrect old password", oldPass);
			}

			DataBase.UserManager.ChangePassword(id, oldPass, newPass);
			return new OperationDetails(true, "Changed pass successfully!", "");
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
					role = new ApplicationRole { Name = roleName };
					DataBase.RoleManager.Create(role);
				}

				await Create(adminDTO);
			}
		}

		public List<string> StringToList(string str)
		{
			var outList = new List<string>();
			if (str == null)
			{
				return outList;
			}

			outList = str.Split(',').ToList();

			if (outList[outList.Count - 1] == "")
				outList.RemoveAt(outList.Count - 1);

			return outList;
		}

		private void CheckRoles()
		{
			ApplicationRole userRole = DataBase.RoleManager.FindByName("user");
			ApplicationRole adminRole = DataBase.RoleManager.FindByName("admin");

			if (userRole == null)
			{
				userRole = new ApplicationRole { Name = "user" };
				DataBase.RoleManager.Create(userRole);
			}

			if (adminRole == null)
			{
				adminRole = new ApplicationRole { Name = "admin" };
				DataBase.RoleManager.Create(adminRole);
			}
		}

		private string ListToString(List<string> userDTOs)
		{
			if (userDTOs.Count == 0)
				return "";

			var output = "";

			foreach (var item in userDTOs)
				output += item + ",";

			return output;
		}
	}
}
