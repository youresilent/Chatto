using Chatto.BLL.DTO;
using Chatto.BLL.Infrastructure;
using Chatto.BLL.Interfaces;
using Chatto.Hubs;
using Chatto.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Chatto.Controllers
{
	public class AccountController : Controller
	{
		#region init

		private string[] statusStrings = new string[] {
			"has added you to their friendslist! Refreshing page...",
			"has removed you from their friendslist! Refreshing page..."
		};

		private readonly IMessageService _messageService;
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

		public AccountController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		#endregion

		#region registration

		public ViewResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterModel registerModel)
		{
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
					ModelState.AddModelError("", "Wrong login/password input!");
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
			UserDTO user = GetUserData(User.Identity.Name);
			ViewBag.Friends = GetUserFriends();

			return View(user);
		}

		#endregion

		#region profile-methods

		[Authorize]
		[Route("Account/ProfileInfo/{userName}")]
		public ViewResult ProfileInfo(string userName)
		{
			return View(GetUserData(userName));
		}

		[Authorize]
		public ViewResult ProfileEdit()
		{
			return View(GetUserData(User.Identity.Name));
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
				return View(newUser);
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
		public ActionResult AddFriend(string friendUserName)
		{
			string currentUserName = User.Identity.Name;

			OperationDetails operation = UserService.AddFriend(currentUserName, friendUserName);

			if (operation.Succeeded)
			{
				SignalHub.Static_SendNotification(currentUserName, friendUserName, statusStrings[0]);
				return RedirectToAction("Home");
			}
			else
				return Redirect("/Shared/Error.cshtml");
		}

		[Authorize]
		public ActionResult RemoveFriend(string friendUserName)
		{
			string currentUserName = User.Identity.Name;

			OperationDetails operation = UserService.RemoveFriend(User.Identity.Name, friendUserName);

			if (operation.Succeeded)
			{
				SignalHub.Static_SendNotification(currentUserName, friendUserName, statusStrings[1]);
				return RedirectToAction("Home");
			}
			else
				return Redirect("/Shared/Error.cshtml");

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
				return Redirect("/Shared/Error.cshtml");

			_messageService.RemoveMessages(confirmation);

			var operation = UserService.DeleteAccount(confirmation);

			LogOut();

			if (!operation.Succeeded)
				return Redirect("/Shared/Error.cshtml");

			return RedirectToAction("Index", "Home");
		}

		#endregion

		#region non-action methods

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

			foreach (var user in stringList)
			{
				users.Add(GetUserData(user));
			}

			return users;
		}

		#endregion
	}
}