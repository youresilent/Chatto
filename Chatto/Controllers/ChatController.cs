using Chatto.BLL.DTO;
using Chatto.BLL.Interfaces;
using Chatto.Hubs;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Chatto.Controllers
{
	public class ChatController : Controller
	{
		private readonly IMessageService _messageService;

		public ChatController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		private IUserService UserService
		{
			get
			{
				return HttpContext.GetOwinContext().GetUserManager<IUserService>();
			}
		}

		public ActionResult ChatRoom(string friendUserName)
		{
			var currentUserFriends = GetUserFriends();

			if (currentUserFriends.Find(u => u.UserName == friendUserName) == null)
			{
				return RedirectToAction("Error");
			}

			var messages = _messageService.GetMessages(User.Identity.Name, friendUserName);
			ViewBag.FriendUserName = friendUserName;

			return View(messages);
		}

		[HttpPost]
		public ActionResult SendMessage(string messageText, string friendUserName)
		{
			var currentUserName = User.Identity.Name;
			var message = GetMessageDTO(currentUserName, friendUserName, messageText, DateTime.Now);

			var operation = _messageService.Create(message);

			if (operation.Result.Succeeded)
			{
				SignalHub.Static_SendMessage(message, currentUserName, friendUserName);
				SignalHub.Static_SendMessage(message, friendUserName, currentUserName);

				SignalHub.Static_SendMessageNotification(friendUserName, currentUserName);

				return Content(StringsResource.Message_SentOK);
			}
			else
			{
				return Content(StringsResource.Message_SendError);
			}

		}

		public ViewResult Error()
		{
			return View();
		}

		private List<UserDTO> GetUserFriends()
		{
			var user = UserService.GetUserData(User.Identity.Name);
			var friends = StringToList(user.Friends);

			var friendsDTOs = new List<UserDTO>();

			foreach (var friend in friends)
			{
				friendsDTOs.Add(UserService.GetUserData(friend.UserName));
			}

			return friendsDTOs;
		}

		private List<UserDTO> StringToList(string friendsList)
		{
			var stringList = UserService.StringToList(friendsList);
			var users = new List<UserDTO>();

			foreach (var user in stringList)
			{
				users.Add(UserService.GetUserData(user));
			}

			return users;
		}

		private MessageDTO GetMessageDTO(string sender, string recipient, string message, DateTime sendDateTime)
		{
			var messageDTO = new MessageDTO
			{
				Sender = sender,
				Recipient = recipient,
				Message = message,
				SendDateTime = sendDateTime
			};

			return messageDTO;
		}
	}
}