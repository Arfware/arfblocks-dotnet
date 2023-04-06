using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Arfware.ArfBlocks.Core
{
	public class ArfBlocksDependencyProvider
	{
		private Dictionary<Type, Type> dependencies;
		private Dictionary<Type, object> dependencyInstances;

		public Action BuildDependencies;

		public ArfBlocksDependencyProvider()
		{
			dependencies = new Dictionary<Type, Type>();
			dependencyInstances = new Dictionary<Type, object>();
		}

		public void Add<T>(T instance)
		{
			if (dependencyInstances.Any(t => t.Key == typeof(T)))
				dependencyInstances[typeof(T)] = instance;
			else
				dependencyInstances.Add(typeof(T), instance);
		}

		public void Add<Ti, Tc>()
		{
			dependencies.Add(typeof(Ti), typeof(Tc));
		}

		public void Add<T>()
		{
			dependencies.Add(typeof(T), typeof(T));
		}

		public void ReBuildDependencies()
		{
			dependencies = new Dictionary<Type, Type>();
			dependencyInstances = new Dictionary<Type, object>();
			this.BuildDependencies();
		}

		public T GetInstance<T>()
		{
			// Check Built
			if (dependencies.Count == 0 && dependencyInstances.Count == 0)
			{
				dependencies = new Dictionary<Type, Type>();
				dependencyInstances = new Dictionary<Type, object>();
				this.BuildDependencies();
			}

			var type = typeof(T);

			object item;
			dependencyInstances.TryGetValue(type, out item);


			if (item != null)
			{
				return (T)item;
			}
			else
			{
				var creatingType = dependencies[type];
				var constructors = creatingType.GetConstructors();

				if (constructors.Length == 0)
				{
					return (T)Activator.CreateInstance(creatingType);
				}
				else
				{
					var constructorInfo = constructors[0];
					var constructorParameters = constructorInfo.GetParameters();
					var objects = constructorParameters.Select((p) => RecursiveGetInstance(p.ParameterType)).ToArray();

					return (T)Activator.CreateInstance(creatingType, objects);
				}
			}
		}

		private object RecursiveGetInstance(Type type)
		{
			object item;
			dependencyInstances.TryGetValue(type, out item);

			if (item != null)
			{
				return item;
			}
			else
			{
				var constructorInfo = type.GetConstructors()[0];
				var constructorParameters = constructorInfo.GetParameters();
				var objects = constructorParameters.Select((p) => RecursiveGetInstance(p.ParameterType)).ToArray();

				var creatingType = dependencies[type];
				return Activator.CreateInstance(creatingType, objects);
			}
		}
	}
}
