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
        public ActionResult ChatAction(string messageText, string currentUserName, string friendUserName)
		{
			var message = GetMessageDTO(currentUserName, friendUserName, messageText, DateTime.Now);

			var operation = _messageService.Create(message);

			SignalHub.Static_SendMessage(message, currentUserName);
			SignalHub.Static_SendMessage(message, friendUserName);

			return Content(operation.Result.Message);
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