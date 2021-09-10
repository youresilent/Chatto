using Chatto.BLL.DTO;
using Chatto.BLL.Interfaces;
using Chatto.Hubs;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public ActionResult ChatRoom(string friendUserName)
		{
			List<MessageDTO> messages = _messageService.GetMessages(User.Identity.Name, friendUserName);
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

				return Content("Message sent successfully!");
			}
			else
			{
				return Content("Message was not sent!");
			}

		}

		private MessageDTO GetMessageDTO(string sender, string recipient, string message, DateTime sendDateTime)
		{
			MessageDTO messageDTO = new MessageDTO
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