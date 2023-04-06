using System;
using BusinessModules.Management.Infrastructure.Services;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.RelationalDB;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Application.Configuration
{
	public class ApplicationDependencyProvider : ArfBlocksDependencyProvider
	{
		public ApplicationDependencyProvider(IHttpContextAccessor httpContextAccessor, ProjectNameConfigurations projectConfigurations)
		{
			// Instances
			base.Add<RelationalDbConfiguration>(projectConfigurations.RelationalDbConfiguration);
			base.Add<EnvironmentConfiguration>(projectConfigurations.EnvironmentConfiguration);
			base.Add<IHttpContextAccessor>(httpContextAccessor);
			base.Add<IdentifiedClient>(new IdentifiedClient());

			// Types
			base.Add<ApplicationDbContext, ApplicationDbContext>();
			base.Add<DbValidationService, DbValidationService>();
			base.Add<EnvironmentService, EnvironmentService>();
			base.Add<ClientProvider, ClientProvider>();
			base.Add<CurrentClientService, CurrentClientService>();
			base.Add<IJwtService, JwtService>();
			base.Add<ActivityLogService, ActivityLogService>();
		}
	}
}
