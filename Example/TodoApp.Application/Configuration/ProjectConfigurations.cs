using System;
using BusinessModules.Management.Infrastructure.Services;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.RelationalDB;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Application.Configuration
{

	public class ProjectNameConfigurations : IConfigurationClass
	{
		public EnvironmentConfiguration EnvironmentConfiguration { get; set; }
		public RelationalDbConfiguration RelationalDbConfiguration { get; set; }
		public DocumentDbConfiguration DocumentDbConfiguration { get; set; }
	}
}
