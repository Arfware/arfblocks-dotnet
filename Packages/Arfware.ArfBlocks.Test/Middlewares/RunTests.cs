using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Test.Abstractions;
using Arfware.ArfBlocks.Test.Registers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arfware.ArfBlocks.Test.Middlewares
{
	public static class Middlewares
	{
		public class UseScenarioTestOptions
		{
			public Type TestConfigurationsType { get; set; }
			public List<Type> SelectedTestList { get; set; }
			public List<Type> IgnoredTestList { get; set; }
		}

		public static async Task<IApplicationBuilder> RunTests(this IApplicationBuilder app, IConfiguration configuration,
																	   Action<UseScenarioTestOptions> options)
		{
			var opt = new UseScenarioTestOptions();
			options(opt);

			// DependencyProvider Operations
			var dependencyProvider = app.ApplicationServices.GetService<ArfBlocksDependencyProvider>();
			if (dependencyProvider == null)
				return app;

			// Test Class Operations
			var testList = TestRegister.GetAllTests();

			int succeddedTestCount = 0;
			int failedTestCount = 0;

			if (opt.SelectedTestList != null && opt.SelectedTestList.Count > 0)
			{
				testList = testList.Where(t => opt.SelectedTestList.Contains(t.TestType)).ToList();
				PrintWarningMessage($"WARNING: {opt.SelectedTestList.Count} Test(s) selected to run. Others will be ignored.");
			}

			if (opt.IgnoredTestList != null && opt.IgnoredTestList.Count > 0)
			{
				testList = testList.Where(t => !opt.IgnoredTestList.Contains(t.TestType)).ToList();
				PrintWarningMessage($"WARNING: {opt.SelectedTestList.Count} Test(s) ignored. Others will be run.");
			}

			ITestOperations testOperations = (ITestOperations)Activator.CreateInstance(opt.TestConfigurationsType, dependencyProvider);
			await testOperations.PreExecuting();

			System.Console.WriteLine($"\nStarting Executing {testList.Count} Tests.");

			foreach (var test in testList)
			{
				object obj = Activator.CreateInstance(test.TestType);
				if (obj == null)
					continue;

				var isSuccedded = await RunTest(dependencyProvider, (IArfBlocksTest)obj);
				if (isSuccedded)
					succeddedTestCount++;
				else failedTestCount++;
			}

			System.Console.WriteLine($"Finished Executing {testList.Count} Tests.\n");
			System.Console.WriteLine($"Total     Test Count: {succeddedTestCount + failedTestCount}");
			System.Console.WriteLine($"Succedded Test Count: {succeddedTestCount}");
			System.Console.WriteLine($"Failed    Test Count: {failedTestCount}");
			System.Console.WriteLine("");
			System.Console.WriteLine(failedTestCount == 0 ? "System is stable." : "System is not stable !");

			await testOperations.PostExecuting();

			return app;
		}

		private static List<Type> GetScenarioTestTypes()
		{
			var assembly = Assembly.GetExecutingAssembly();
			var typeList = new List<Type>();
			foreach (var type in assembly.GetExportedTypes())
			{
				if (type.IsAbstract)
					continue;

				foreach (var inter in type.GetInterfaces())
				{
					// System.Console.WriteLine(inter.FullName);
					if (inter == typeof(IArfBlocksTest))
					{
						typeList.Add(type);
					}
				}
			}

			return typeList;
		}

		private static async Task<bool> RunTest(ArfBlocksDependencyProvider dependencyProvider, IArfBlocksTest scenarioTest)
		{
			var testName = GetTestName(scenarioTest);

			try
			{
				scenarioTest.SetDependencies(dependencyProvider);
				await scenarioTest.PrepareTest();
				await scenarioTest.SetActor();
				await scenarioTest.RunTest();
				PrintSuccessResult(testName);
				return true;
			}
			catch (System.Exception ex)
			{
				PrintFailResult(testName);
				System.Console.WriteLine(ex.Message);
				if (ex.InnerException != null)
					System.Console.WriteLine(ex.InnerException.Message);
				return false;
			}

		}

		private static string GetTestName(IArfBlocksTest scenarioTest)
		{
			var fullName = scenarioTest.GetType().FullName;

			var parts = fullName.Split(".");
			if (parts.Length < 4)
				return "__ERROR_NOT_DETECTED__";

			return $"{parts[parts.Length - 5]} - {parts[parts.Length - 3]} - {parts[parts.Length - 1]}";
		}

		private static void PrintSuccessResult(string result)
		{
			var originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Green;

			char c = '√';
			Console.WriteLine($"{c} {result}");

			Console.ForegroundColor = originalColor;
		}

		private static void PrintFailResult(string result)
		{
			var originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			char c = 'X';
			Console.Write($"{c} {result} ::: ");

			Console.ForegroundColor = originalColor;
		}

		private static void PrintWarningMessage(string message)
		{
			var originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine($"{message}");

			Console.ForegroundColor = originalColor;
		}
	}
}