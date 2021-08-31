using Chatto.BLL.DTO;
using Chatto.BLL.Interfaces;
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

        // GET: Chat
        public ActionResult ChatRoom()
        {
            _messageService.Create(new MessageDTO { Id = "0", Message = "zxc", Recipient = "recipient", Sender = "sender", SendDateTime = DateTime.Now });
            return Content(_messageService.ToString());
        }
    }
}