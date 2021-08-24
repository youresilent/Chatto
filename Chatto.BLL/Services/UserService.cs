using Chatto.BLL.DTO;
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
			ApplicationUser user = await DataBase.UserManager.FindByEmailAsync(userDTO.Email);

			if (user == null)
			{
				user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };
				var result = await DataBase.UserManager.CreateAsync(user, userDTO.Password);

				if (result.Errors.Count() > 0)
					return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

				await DataBase.UserManager.AddToRoleAsync(user.Id, userDTO.Role);

				ClientProfile clientProfile = new ClientProfile { Adress = userDTO.Adress, Age = userDTO.Age, Gender = userDTO.Gender, Id = user.Id, RealName = userDTO.RealName, Friends = "" };
				DataBase.ClientManager.Create(clientProfile);

				await DataBase.SaveAsync();

				return new OperationDetails(true, "Successful registration!", "");
			}
			else
				return new OperationDetails(false, "This User name already exists!", "UserName");
		}

		public OperationDetails AddFriend(string currentUser, string friendUserName)
		{
			ApplicationUser user = DataBase.UserManager.FindByName(currentUser);
			ApplicationUser friend = DataBase.UserManager.FindByName(friendUserName);

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
			ApplicationUser user = DataBase.UserManager.FindByName(currentUser);
			ApplicationUser friend = DataBase.UserManager.FindByName(friendUserName);

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
				Friends = tempUser.ClientProfile.Friends
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

			if (DataBase.UserManager.CheckPassword(user, oldPass))
			{
				DataBase.UserManager.ChangePassword(id, oldPass, newPass);
				return new OperationDetails(true, "Changed pass successfully!", "");
			}
			else
				return new OperationDetails(false, "Incorrect old password", oldPass);
		}

		public void Dispose()
		{
			DataBase.Dispose();
		}

		public async Task SetInitialData(UserDTO adminDTO, List<string> roles)
		{
			foreach (var roleName in roles)
			{
				var role = await DataBase.RoleManager.FindByNameAsync(roleName);

				if (role == null)
				{
					role = new ApplicationRole { Name = roleName };
					await DataBase.RoleManager.CreateAsync(role);
				}

				await Create(adminDTO);
			}
		}

		public List<string> StringToList(string str)
		{
			List<string> outList = str.Split(',').ToList();

			if (outList[outList.Count - 1] == "")
				outList.RemoveAt(outList.Count - 1);

			return outList;
		}

		private string ListToString(List<string> userDTOs)
		{
			if (userDTOs.Count == 0)
				return "";

			var output = "";

			foreach (var item in userDTOs)
				output += item + ",";

			//for (int i = 0; i < userDTOs.Count - 1; i++)
			//{
			//	output += userDTOs[i].ToString() + ",";
			//}

			//output += userDTOs[userDTOs.Count - 1];

			return output;
		}
	}
}
