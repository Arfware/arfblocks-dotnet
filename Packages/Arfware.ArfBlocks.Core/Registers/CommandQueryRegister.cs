using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Arfware.ArfBlocks.Core.Abstractions;
using Arfware.ArfBlocks.Core.Attributes;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocks.Core.Settings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Arfware.ArfBlocks.Core.Contexts;

namespace Arfware.ArfBlocks.Core
{
	public class CommandQueryRegister
	{
		private static ApplicationDataModel applicationModel;
		private static List<EndpointModel> commandQueryList;
		public static EndpointModel PreOperateEndpoint;
		public static EndpointModel PostOperateEndpoint;

		public static void RegisterAssemblyWithName(string assemblyName)
		{
			var assembly = Assembly.Load(assemblyName);
			CommandQueryRegister.RegisterAssembly(assembly);
		}

		public static void SetOperateHandler(Type preOperateHandler, Type postOperateHandler)
		{
			CommandQueryRegister.PreOperateEndpoint = CommandQueryRegister.commandQueryList.FirstOrDefault(o => o.Handler == preOperateHandler);
			CommandQueryRegister.PostOperateEndpoint = CommandQueryRegister.commandQueryList.FirstOrDefault(o => o.Handler == postOperateHandler); ;
		}

		public static void RegisterAssembly(Assembly assembly)
		{
			CommandQueryRegister.applicationModel = CommandQueryRegister.BuildApplicationModel(assembly);
			CommandQueryRegister.commandQueryList = CommandQueryRegister.GetAllEndpointsFromAssembly(assembly);

			if (CommandQueryRegister.applicationModel.ConfigurationClassType == null)
				throw new Exception($"ArfBlocks could not find Configuration Class in Application Project: {assembly.FullName}");

			if (CommandQueryRegister.applicationModel.DependencyProviderType == null)
				throw new Exception($"ArfBlocks could not find DependencyProvider Class in Application Project: {assembly.FullName}");

			if (GlobalSettings.CanLogInformation())
				System.Console.WriteLine($"## ArfBlocks CommandQueryRegister: Registered {CommandQueryRegister.commandQueryList.Count} Handler(s)");
		}

		public static EndpointModel GetEndpointByObjectAndAction(string objectName, string actionName)
		{
			return CommandQueryRegister.commandQueryList
											.FirstOrDefault(c => c.ObjectName.ToLower() == objectName.ToLower() && c.ActionName.ToLower() == actionName.ToLower());
		}

		public static ApplicationDataModel GetApplicationData()
		{
			return CommandQueryRegister.applicationModel;
		}

		public static List<EndpointModel> GetAllEndpoints()
		{
			return CommandQueryRegister.commandQueryList;
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

		private static ApplicationDataModel BuildApplicationModel(Assembly assembly)
		{
			var applicationData = new ApplicationDataModel();

			foreach (var type in assembly.GetExportedTypes())
			{
				if (type.BaseType != null && type.BaseType == typeof(ArfBlocksDependencyProvider))
					applicationData.DependencyProviderType = type;

				foreach (var inter in type.GetInterfaces())
				{
					if (inter == typeof(IConfigurationClass))
						applicationData.ConfigurationClassType = type;

					if (applicationData.ConfigurationClassType != null && applicationData.DependencyProviderType != null)
						break;
				}
			}

			return applicationData;
		}

		private static List<EndpointModel> GetAllEndpointsFromAssembly(Assembly assembly)
		{
			var typeList = new List<Type>();
			foreach (var type in assembly.GetExportedTypes())
			{
				typeList.Add(type);
			}

			var list = new List<EndpointModel>();
			foreach (var type in typeList)
			{
				foreach (var inter in type.GetInterfaces())
				{
					// System.Console.WriteLine(inter.FullName);
					if (inter == typeof(IRequestHandler))
					{
						string nameSpace = type.Namespace;

						List<Type> typesInAssembly = typeList
												.Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
												.ToList();

						var names = ParseNamespace(nameSpace);
						var attributes = GetAttributes(type);

						var endpoint = new EndpointModel()
						{
							ObjectName = names.objectName,
							ActionName = names.actionName,
							EndpointType = GetEndpointType(nameSpace),
							Handler = type,
							PreHandler = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IPreRequestHandler))),
							PostHandler = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IPostRequestHandler))),
							RequestModel = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestModel))),
							ResponseModel = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IResponseModel) || i == typeof(IResponseModel<Array>))),
							DataAccess = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IDataAccess))),
							Validator = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestValidator))),
							Verificator = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IRequestVerificator))),
							Context = typesInAssembly.FirstOrDefault(t => t.GetInterfaces().Any(i => i == typeof(IEndpointContext))),
							IsInternal = attributes.IsInternal,
							IsAuthorize = attributes.IsAuthorize,
							IsAllowAnonymous = attributes.IsAllowAnonymous,
							IsEventHandler = attributes.IsEventHandler,
							IsAuditable = attributes.isAuditableHandler,
							IsTraceable = attributes.isTraceableHandler,
						};

						if (endpoint.ResponseModel != null)
							endpoint.IsResponseModelArray = endpoint.ResponseModel.GetInterfaces().Any(i => i == typeof(IResponseModel<Array>));

						list.Add(endpoint);
					}
				}
			}

			return list;
		}

		public static (string objectName, string actionName) ParseNamespace(string typeNamespace)
		{
			var parts = typeNamespace.Split('.');

			if (parts.Length == 0 || parts.Length < 3)
				return ("__ERROR__", "__ERROR__");

			var length = parts.Length;
			return (parts[length - 3], parts[length - 1]);
		}

		public static EndpointModel.EndpointTypes GetEndpointType(string nameSpace)
		{
			if (nameSpace.Contains(".Commands."))
				return EndpointModel.EndpointTypes.Command;
			else if (nameSpace.Contains(".Queries."))
				return EndpointModel.EndpointTypes.Query;

			throw new Exception("Command Query Register Error: Endpoint Type Could Not Determined !");
		}

		public static (bool IsInternal, bool IsAuthorize, bool IsAllowAnonymous, bool IsEventHandler, bool isAuditableHandler, bool isTraceableHandler,string sourceRef) GetAttributes(Type type)
		{
			bool isInternal = false;
			bool isAuthorize = false;
			bool isAllowAnonymous = false;
			bool isEventHandler = false;
			bool isAuditableHandler = false;
			bool isTraceableHandler = false;
			var sourceRef = string.Empty;

			var attrs = System.Attribute.GetCustomAttributes(type).ToList();  // Reflection.  
			attrs.ForEach((attr) =>
			{
				
				if (attr is InternalHandlerAttribute)
					isInternal = true;
				else if (attr is AuthorizedHandlerAttribute)
					isAuthorize = true;
				else if (attr is AllowAnonymousHandlerAttribute)
					isAllowAnonymous = true;
				else if (attr is EventHandlerAttribute)
					isEventHandler = true;


				if (attr is AuditableHandlerAttribute auditAttr)
				{
					sourceRef = auditAttr.SourceRef;
					isAuditableHandler = true;
				}
				
				if (attr is TraceableHandlerAttribute)
					isTraceableHandler = true;

			});

			// Lock Control
			if (isAuthorize && isAllowAnonymous)
				throw new Exception($"'{type.FullName}' marked as Authorize and AllowAnonymous Attributes. You can't use them together!");

			return (isInternal, isAuthorize, isAllowAnonymous, isEventHandler, isAuditableHandler, isTraceableHandler, sourceRef);
		}
	}
}