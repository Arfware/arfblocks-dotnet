using System;
using System.Text;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocks.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arfware.ArfBlocks.Core.Extentions
{
	public static class AddArfBlocksExtentions
	{
		public class AddArfBlocksOptions
		{
			public LogLevels LogLevel { get; set; }
			public string ApplicationProjectNamespace { get; set; }
			public IConfigurationSection ConfigurationSection { get; set; }
			public Type PreOperateHandler;
			public Type PostOperateHandler;
		}

		public static IServiceCollection AddArfBlocks(this IServiceCollection services, Action<AddArfBlocksOptions> options)
		{
			var opt = new AddArfBlocksOptions();
			options(opt);

			CommandQueryRegister.RegisterAssemblyWithName(opt.ApplicationProjectNamespace);
			CommandQueryRegister.SetOperateHandler(opt.PreOperateHandler, opt.PostOperateHandler);

			var applicationData = CommandQueryRegister.GetApplicationData();
			var moduleArfBlocksSettings = opt.ConfigurationSection.Get(applicationData.ConfigurationClassType);

			services.AddSingleton(applicationData.ConfigurationClassType, moduleArfBlocksSettings);
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient(typeof(ArfBlocksDependencyProvider), applicationData.DependencyProviderType);

			GlobalSettings.LogLevel = opt.LogLevel;

			return services;
		}
	}
}