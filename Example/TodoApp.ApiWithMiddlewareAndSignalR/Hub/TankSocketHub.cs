using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace TodoApp.WebApi;

public class ChatHub : Hub
{
	public ChatHub()
	{
	}

	public override async Task OnConnectedAsync()
	{
		// System.Console.WriteLine("User Connected via WebSocket");
		await Clients.All.SendAsync("UserConnected", "newUser");

		await base.OnConnectedAsync();
	}

	public async Task ServiceInfo(string user, string message)
	{
		// System.Console.WriteLine("ServiceInfo:::" + user + "::" + message);
		await Clients.All.SendAsync("ServiceInfo", user, message);
	}
}