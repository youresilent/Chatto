﻿using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.BLL.Interfaces;
using Chatto.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Chatto.Controllers
{
    public class AccountController : Controller
    {
        #region init

        private IUserService UserService
		{
			get
			{
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
			}
		}

        private IAuthenticationManager AuthenticationManager
		{
            get
			{
                return HttpContext.GetOwinContext().Authentication;
			}
		}

		#endregion

		#region registration

        public ViewResult Register()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register (RegisterModel registerModel)
		{
            //await SetInitialDataAsync();
            if (ModelState.IsValid)
			{
                UserDTO userDTO = new UserDTO
                {
                    UserName = registerModel.UserName,
                    Password = registerModel.Password,
                    RealName = registerModel.RealName,
                    Email = registerModel.Email,
                    Adress = registerModel.Adress,
                    Gender = registerModel.Gender,
                    Age = registerModel.Age,
                    Role = "user"
                };

                OperationDetails operation = await UserService.Create(userDTO);

                if (operation.Succeeded)
                    return RedirectToAction("LogIn");
                else
                    ModelState.AddModelError(operation.Property, operation.Message);
			}

            return View(registerModel);
		}

		#endregion

		#region login/logout
		public RedirectToRouteResult LogOut()
		{
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
		}

        public ViewResult LogIn()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LogInModel loginModel)
        {
            if (ModelState.IsValid)
			{
                UserDTO userDTO = new UserDTO
                {
                    UserName = loginModel.UserName,
                    Password = loginModel.Password
                };

                ClaimsIdentity claims = await UserService.Authenticate(userDTO);

                if (claims == null)
                    ModelState.AddModelError("", "Неверная комбинация логин/пароль");
                else
				{
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claims);

                    return RedirectToAction("Home");
                }

			}

            return View(loginModel);
        }

		#endregion

		#region homepage

        [Authorize]
        public ViewResult Home()
		{
            var zxc = User.Identity.Name;
            UserDTO user = GetUserData(User.Identity.Name);
            var friends = GetUserFriends();
            ViewBag.Friends = friends;

            return View(user);
		}

		#endregion

		#region profile-methods

		[Authorize]
        [Route("Account/ProfileInfo/{userName}")]
        public ViewResult ProfileInfo(string userName)
		{
            UserDTO user = GetUserData(userName);
            return View(user);
		}

        [Authorize]
        public ViewResult ProfileEdit()
		{
            UserDTO user = GetUserData(User.Identity.Name);
            return View(user);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileEdit(UserDTO newUser)
        {
            if (string.IsNullOrWhiteSpace(newUser.Adress))
                ModelState.AddModelError("Adress", "Adress is required!");

            if (string.IsNullOrWhiteSpace(newUser.Email))
                ModelState.AddModelError("Email", "E-mail adress is required!");

            if (string.IsNullOrWhiteSpace(newUser.RealName))
                ModelState.AddModelError("RealName", "Real name is required!");

            if (string.IsNullOrWhiteSpace(newUser.Gender))
                ModelState.AddModelError("Gender", "Gender is required!");

            if (newUser.Age < 5 || newUser.Age > 120)
                ModelState.AddModelError("Age", "Age input is not correct! It must be between 5 and 120.");

            if (ModelState.IsValid)
			{
                UserService.ChangeSecondaryInfo(newUser);
                return RedirectToAction("Home");
            }
            else
			{
                return View(newUser);
            }
        }

        [Authorize]
        public ViewResult ChangePassword()
		{
            return View();
		}

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
		{
            if (ModelState.IsValid)
			{
                UserDTO user = GetUserData(User.Identity.Name);
                OperationDetails operation = UserService.ChangePassword(model.OldPassword, model.NewPassword, user.Id);

                if (operation.Succeeded)
                    return RedirectToAction("Home");
                else
                    ModelState.AddModelError(operation.Property, operation.Message);
			}

            return View(model);
		}

        [Authorize]
        public ViewResult PeopleList()
		{
            ViewBag.Users = UserService.GetAllUsers();
            return View(GetUserData(User.Identity.Name));
		}

        [Authorize]
        public ViewResult FriendsList()
		{
            ViewBag.Friends = GetUserFriends();

            return View();
		}

        [Authorize]
        public ActionResult AddFriend(string userName)
		{
            OperationDetails operation = UserService.AddFriend(User.Identity.Name, userName);

            if (operation.Succeeded)
                return RedirectToAction("Home");
            else
                return Redirect("/Shared/Error.cshtml");
		}

        [Authorize]
        public ActionResult RemoveFriend(string userName)
		{
            var operation = UserService.RemoveFriend(User.Identity.Name, userName);
            return RedirectToAction("Home");
		}

        [Authorize]
        public ViewResult DeleteAccount()
		{
            return View();
		}

		[HttpPost]
		[Authorize]
		public ActionResult DeleteAccount(string confirmation)
		{
			if (User.Identity.Name != confirmation)
                return View("Error");

            var operation = UserService.DeleteAccount(confirmation);

            LogOut();

            if (!operation.Succeeded)
                return View("Error");

            return RedirectToAction("Index", "Home");
        }

        #endregion

        //private async Task SetInitialDataAsync()
        //{
        //	await UserService.SetInitialData(new BLL.DTO.UserDTO
        //	{
        //		Adress = "ADMINADRESS",
        //		Age = 10,
        //		UserName = "adminadmin",
        //		Email = "ADMINEMAIL",
        //		RealName = "ADMINREALNAME",
        //		Gender = "ADMINGENDER",
        //		Password = "123123",
        //		Role = "admin"
        //	}, new List<string> { "user", "admin" });
        //}

        public ActionResult Index()
        {
            return View();
        }

        private UserDTO GetUserData(string userName)
		{
            return UserService.GetUserData(userName);
		}

        private List<UserDTO> GetUserFriends()
		{
            var user = GetUserData(User.Identity.Name);
            var friends = StringToList(user.Friends);
            List<UserDTO> friendsDTOs = new List<UserDTO>();

            foreach (var friend in friends)
                friendsDTOs.Add(GetUserData(friend.UserName));

            return friendsDTOs;
        }

        private List<UserDTO> StringToList(string friendsList)
		{
            List<string> stringList = UserService.StringToList(friendsList);
            List<UserDTO> users = new List<UserDTO>();

            foreach(var user in stringList)
			{
                users.Add(GetUserData(user));
			}

            return users;
		}
    }
}