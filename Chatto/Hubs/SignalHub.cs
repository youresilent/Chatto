using Chatto.Models;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatto.Hubs
{
	public class SignalHub : Hub
	{
		public static List<HubUser> Users = new List<HubUser>();

		private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext("SignalHub");

		public static void Static_SendNotification(string friendUserName, string message)
		{
			HubUser friend = Users.FirstOrDefault(u => u.UserName == friendUserName);

			if (friend == null)
				return;

			hubContext.Clients.Client(friend.ConnectionId).showNotification(friendUserName, message);
		}

		#region connectivity
		public void Connect(string userName)
		{
			string id = Context.ConnectionId;

			if (!Users.Any(u => u.ConnectionId == id))
			{
				Users.Add(new HubUser { ConnectionId = id, UserName = userName });
				Clients.Caller.onConnected(id, userName, Users);
				Clients.AllExcept(id).onNewUserConnected(id, userName);
			}
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			var item = Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);

			if (item != null)
			{
				Users.Remove(item);
			}

			return base.OnDisconnected(stopCalled);
		}

		#endregion
	}
}