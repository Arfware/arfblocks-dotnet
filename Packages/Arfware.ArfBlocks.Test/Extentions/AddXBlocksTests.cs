using System;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Test.Registers;
using Arfware.ArfBlocks.Test.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arfware.ArfBlocks.Test.Extentions
{
	public static class AddArfBlocksTestsExtentions
	{
		public class AddArfBlocksTestOptions
		{
			public LogLevels LogLevel { get; set; }
			public string ApplicationProjectNamespace { get; set; }
			public IConfigurationSection ConfigurationSection { get; set; }
		}

		public static IServiceCollection AddArfBlocksTests(this IServiceCollection services, Action<AddArfBlocksTestOptions> options)
		{
			var opt = new AddArfBlocksTestOptions();
			options(opt);

			TestRegister.RegisterAssemblyWithName(opt.ApplicationProjectNamespace);

			var applicationData = TestRegister.GetApplicationTestData();

			GlobalSettings.LogLevel = opt.LogLevel;

			return services;
		}
	}
}