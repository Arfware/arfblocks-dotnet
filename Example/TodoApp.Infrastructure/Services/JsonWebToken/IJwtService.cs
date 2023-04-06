using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Services
{
	public interface IJwtService
	{
		string GenerateJwt(User user);
		int GetExpirationDayCount();
	}
}
