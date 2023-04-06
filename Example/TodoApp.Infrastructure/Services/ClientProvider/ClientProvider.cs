using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using TodoApp.Infrastructure.RelationalDB;
using TodoApp.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace TodoApp.Infrastructure.Services
{
	public class ClientProvider
	{
		public IdentifiedClient _currentClient;
		private ApplicationDbContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly EnvironmentService _environmentService;

		public ClientProvider(IHttpContextAccessor httpContextAccessor, EnvironmentService environmentService, IdentifiedClient identifiedClient)
		{
			_httpContextAccessor = httpContextAccessor;

			if (environmentService.Environment == CustomEnvironments.Test)
			{
				this._currentClient = identifiedClient;
			}
			else
			{
				this._currentClient = this.ParseJwt();
			}
		}

		private IdentifiedClient ParseJwt()
		{
			try
			{
				var auhtorizatioValue = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
				if (auhtorizatioValue != null)
				{
					// format: [Authorization : Baerer eykjdsfjajfkl230423ujre2r23rnnj]
					string jwtInput = auhtorizatioValue.Split(' ')[1];

					var jwtHandler = new JwtSecurityTokenHandler();

					// Check Token Format
					if (!jwtHandler.CanReadToken(jwtInput)) throw new Exception("Invalid JWT format.");

					var token = jwtHandler.ReadJwtToken(jwtInput);

					// Get Claims
					string username = token.Claims.FirstOrDefault(c => c.Type == "given_name").Value;
					string displayName = token.Claims.FirstOrDefault(c => c.Type == "unique_name").Value;
					Guid userId = Guid.Parse(token.Claims.FirstOrDefault(c => c.Type == "nameid").Value);

					var currentClient = new IdentifiedClient()
					{
						Id = userId,
						DisplayName = displayName,
						Username = username
					};

					return currentClient;
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine($"ClientProvider Error: {exception.Message}");
			}

			return null;
		}

	}

	public class IdentifiedClient
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string DisplayName { get; set; }
		public User UserFromDB { get; set; }
	}
}
