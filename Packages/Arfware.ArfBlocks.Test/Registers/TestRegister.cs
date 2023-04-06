using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Arfware.ArfBlocks.Core.Attributes;
using Arfware.ArfBlocks.Test.Models;
using Arfware.ArfBlocks.Test.Settings;
using Arfware.ArfBlocks.Test.Abstractions;
using static Arfware.ArfBlocks.Core.Models.EndpointModel;

namespace Arfware.ArfBlocks.Test.Registers
{
	public class TestRegister
	{
		private static ApplicationTestDataModel applicationModel;
		private static List<TestModel> testList;

		public static void RegisterAssemblyWithName(string assemblyName)
		{
			var assembly = Assembly.Load(assemblyName);
			TestRegister.RegisterAssembly(assembly);
		}

		public static void RegisterAssembly(Assembly assembly)
		{
			TestRegister.applicationModel = TestRegister.BuildApplicationModel(assembly);
			TestRegister.testList = TestRegister.GetAllEndpointsFromAssembly(assembly);

			if (TestRegister.applicationModel.TestConfigurationsType == null)
				throw new Exception($"ArfBlocks could not find TestOperations Class in Application Project: {assembly.FullName}");

			if (GlobalSettings.CanLogInformation())
				System.Console.WriteLine($"## ArfBlocks.Test  - TestRegister: Registered {TestRegister.testList.Count} Test(s)");
		}

		public static ApplicationTestDataModel GetApplicationTestData()
		{
			return TestRegister.applicationModel;
		}

		public static List<TestModel> GetAllTests()
		{
			return TestRegister.testList;
		}

		private static List<Type> GetTypeByRefencedType<Treference, Tsearch>()
		{
			string nameSpace = typeof(Treference).Namespace;
			Assembly assembly = typeof(Treference).Assembly;

			List<Type> typelist = assembly.GetTypes()
				.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
				.ToList();

			var list = new List<Type>();
			foreach (Type type in typelist)
			{
				if (type.GetInterfaces().Contains(typeof(Tsearch)))
					list.Add(type);
			}

			return list;
		}

		private static ApplicationTestDataModel BuildApplicationModel(Assembly assembly)
		{
			var applicationData = new ApplicationTestDataModel();

			foreach (var type in assembly.GetExportedTypes())
			{
				if (type.BaseType != null && type.BaseType == typeof(ITestOperations))
					applicationData.TestConfigurationsType = type;

				foreach (var inter in type.GetInterfaces())
				{
					if (inter == typeof(ITestOperations))
					{
						applicationData.TestConfigurationsType = type;
						break;
					}
				}
			}

			return applicationData;
		}

		private static List<TestModel> GetAllEndpointsFromAssembly(Assembly assembly)
		{
			var typeList = new List<Type>();
			foreach (var type in assembly.GetExportedTypes())
			{
				typeList.Add(type);
			}

			var list = new List<TestModel>();
			foreach (var type in typeList)
			{
				foreach (var inter in type.GetInterfaces())
				{
					// System.Console.WriteLine(inter.FullName);
					if (inter == typeof(IArfBlocksTest))
					{
						string nameSpace = type.Namespace;

						List<Type> typesInAssembly = typeList
												.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
												.ToList();

						var names = ParseNamespace(nameSpace);
						// var attributes = GetAttributes(type);

						var endpoint = new TestModel()
						{
							ObjectName = names.objectName,
							ActionName = names.actionName,
							EndpointType = GetEndpointType(nameSpace),
							TestType = type,//typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IArfBlocksTest))),
						};

						list.Add(endpoint);
					}
				}
			}

			return list;
		}

		private static (string objectName, string actionName) ParseNamespace(string typeNamespace)
		{
			var parts = typeNamespace.Split('.');

			if (parts.Length == 0 || parts.Length < 3)
				return ("__ERROR__", "__ERROR__");

			var length = parts.Length;
			return (parts[length - 3], parts[length - 1]);
		}

		private static EndpointTypes GetEndpointType(string nameSpace)
		{
			if (nameSpace.Contains(".Commands."))
				return EndpointTypes.Command;
			else if (nameSpace.Contains(".Queries."))
				return EndpointTypes.Query;

			throw new Exception("Command Query Register Error: Endpoint Type Could Not Determined !");
		}

		private static (bool IsInternal, bool IsAuthorize, bool IsAllowAnonymous) GetAttributes(Type type)
		{
			bool isInternal = false;
			bool isAuthorize = false;
			bool isAllowAnonymous = false;

			var attrs = System.Attribute.GetCustomAttributes(type).ToList();  // Reflection.  
			attrs.ForEach((attr) =>
			{
				if (attr is InternalHandlerAttribute)
					isInternal = true;
				else if (attr is AuthorizedHandlerAttribute)
					isAuthorize = true;
				else if (attr is AllowAnonymousHandlerAttribute)
					isAllowAnonymous = true;
			});

			// Lock Control
			if (isAuthorize && isAllowAnonymous)
				throw new Exception($"'{type.FullName}' marked as Authorize and AllowAnonymous Attributes. You can't use them together!");

			return (isInternal, isAuthorize, isAllowAnonymous);
		}
	}

	public class DescendingComparer : System.Collections.IComparer
	{
		public int Compare(Object a, Object b)
		{
			return (new System.Collections.CaseInsensitiveComparer()).Compare(b, a);
		}
	}
}