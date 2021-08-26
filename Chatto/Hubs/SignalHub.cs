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
			HubUser user = Users.Find(u => u.UserName == friendUserName);

			if (user != null)
				hubContext.Clients.Client(user.ConnectionId).showNotification(currentUser, message);
		}

		public void Connect(string userName)
		{
			string id = Context.ConnectionId;
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