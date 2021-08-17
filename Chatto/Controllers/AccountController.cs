using Chatto.BLL.DTO;
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
            await SetInitialDataAsync();
            if (ModelState.IsValid)
			{
                UserDTO userDTO = new UserDTO
                {
                    UserName = registerModel.UserName,
                    Password = registerModel.Password,
                    RealName = registerModel.RealName,
                    Email = registerModel.Email,
                    Address = registerModel.Address,
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

		private async Task SetInitialDataAsync()
		{
			await UserService.SetInitialData(new BLL.DTO.UserDTO
			{
				Address = "ADMINADDRESS",
				Age = 10,
				UserName = "adminadmin",
				Email = "ADMINEMAIL",
				RealName = "ADMINREALNAME",
				Gender = "ADMINGENDER",
				Password = "123123",
				Role = "admin"
			}, new List<string> { "user", "admin" });
		}

		public ActionResult Index()
        {
            return View();
        }
    }
}