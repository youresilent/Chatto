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
			ApplicationUser user = await DataBase.UserManager.FindByEmailAsync(userDTO.Email);

			if (user == null)
			{
				user = new ApplicationUser { Email = userDTO.Email, UserName = userDTO.UserName };
				var result = await DataBase.UserManager.CreateAsync(user, userDTO.Password);

				if (result.Errors.Count() > 0)
					return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

				await DataBase.UserManager.AddToRoleAsync(user.Id, userDTO.Role);

				ClientProfile clientProfile = new ClientProfile { Adress = userDTO.Adress, Age = userDTO.Age, Gender = userDTO.Gender, Id = user.Id, RealName = userDTO.RealName };
				DataBase.ClientManager.Create(clientProfile);

				await DataBase.SaveAsync();

				return new OperationDetails(true, "Регистрация прошла успешно!", "");
			}
			else
			{
				return new OperationDetails(false, "Пользователь с таким именем пользователя уже существует!", "UserName");
			}
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
			};

			return user;
		}

		public async Task ChangeSecondaryInfo(UserDTO newUserInfo)
		{
			ApplicationUser currentUser = DataBase.UserManager.FindByName(newUserInfo.UserName);
			
			currentUser.ClientProfile.Adress = newUserInfo.Adress;
			currentUser.ClientProfile.Age = newUserInfo.Age;
			currentUser.Email = newUserInfo.Email;
			currentUser.ClientProfile.Gender = newUserInfo.Gender;
			currentUser.ClientProfile.RealName = newUserInfo.RealName;

			DataBase.UserManager.Update(currentUser);
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
	}
}
