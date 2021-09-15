using Chatto.BLL.DTO;
using Chatto.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Chatto.Hubs
{
	[HubName("signalHub")]
	public class SignalHub : Hub
	{
		private static List<HubUser> Users = new List<HubUser>();

		private static readonly IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalHub>();

		public static void Static_SendNotification(string currentUser, string friendUserName, string message)
		{
			var user = Users.Find(u => u.UserName == friendUserName);

			if (user != null)
				hubContext.Clients.Client(user.ConnectionId).showFriendNotification(currentUser, message);
		}

		public static void Static_LoadMessages(List<MessageDTO> messages, string currentUserName)
		{
			var user = Users.Find(u => u.UserName == currentUserName);

			foreach (var msg in messages)
			{
				hubContext.Clients.Client(user.ConnectionId).addMessage(msg.Sender, msg.Recipient, msg.Message, msg.SendDateTime);
			}
		}

		public static void Static_SendMessage(MessageDTO message, string userName, string friendName)
		{
			var user = Users.Find(u => u.UserName == userName);

			if (user != null)
			{
				hubContext.Clients.Client(user.ConnectionId).addMessage(message.Sender, message.Recipient, message.Message, message.SendDateTime, friendName);
			}

		}

		public static void Static_SendMessageNotification(string friendUserName, string currentUserName)
		{
			var user = Users.Find(u => u.UserName == friendUserName);

			if (user != null)
			{
				hubContext.Clients.Client(user.ConnectionId).showMessageNotification(currentUserName, StringsResource.Message_ReceivedNotification);
			}

		}

		public void Connect(string userName)
		{
			var id = Context.ConnectionId;
			userName = Context.User.Identity.Name;

			if (!Users.Any(u => u.ConnectionId == id))
			{
				Users.Add(new HubUser { ConnectionId = id, UserName = userName });
				Clients.Caller.onConnected(id, userName, Users);
				Clients.AllExcept(id).onNewUserConnected(id, userName);
			}
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			HubUser item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

			if (item != null)
				Users.Remove(item);

			return base.OnDisconnected(stopCalled);
		}
	}
}