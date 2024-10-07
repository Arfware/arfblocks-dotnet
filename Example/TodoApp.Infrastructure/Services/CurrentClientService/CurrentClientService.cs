using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using TodoApp.Infrastructure.RelationalDB;
using TodoApp.Domain.Entities;
using System.Net;
using System.Net.Sockets;

namespace TodoApp.Infrastructure.Services
{
	public class CurrentClientService
	{
		private IdentifiedClient _currentClient;
		private ApplicationDbContext _dbContext;
		private readonly EnvironmentService _environmentService;
		private readonly HttpContext _httpContext;

		public CurrentClientService(ClientProvider clientProvider, ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
		{
			_dbContext = dbContext;
			_currentClient = clientProvider._currentClient;
			_httpContext = httpContextAccessor.HttpContext;
		}

		public Guid GetCurrentUserId()
		{
			return this._currentClient.Id;
		}

		public string GetCurrentUserDisplayName()
		{
			return this._currentClient?.DisplayName;
		}

		public Guid GetCurrentUserDepartmentId()
		{
			if (this._currentClient.UserFromDB == null)
				this._currentClient.UserFromDB = _dbContext.Users.FirstOrDefault(p => p.Id == this._currentClient.Id);

			return this._currentClient?.UserFromDB.DepartmentId ?? Guid.Empty;
		}

		public User GetUser()
		{
			if (this._currentClient.UserFromDB == null)
				this._currentClient.UserFromDB = _dbContext.Users.FirstOrDefault(p => p.Id == this._currentClient.Id);

			return this._currentClient?.UserFromDB;
		}

		public IPAddress GetIpAdress()
		{
			IPAddress remoteIpAddress = null;
			var forwardedFor = _httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
			if (!string.IsNullOrEmpty(forwardedFor))
			{
				var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries)
									  .Select(s => s.Trim());
				foreach (var ip in ips)
				{
					if (IPAddress.TryParse(ip, out var address) &&
						(address.AddressFamily is AddressFamily.InterNetwork
						 or AddressFamily.InterNetworkV6))
					{
						remoteIpAddress = address;
						break;
					}
				}
			}
			else
			{
				remoteIpAddress = _httpContext.Connection.RemoteIpAddress;
			}

			return remoteIpAddress;
		}
	}
}
