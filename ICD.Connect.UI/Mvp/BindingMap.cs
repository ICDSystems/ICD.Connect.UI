using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Utils;
using ICD.Common.Utils.Extensions;
using ICD.Connect.UI.Attributes;
#if SIMPLSHARP
using Crestron.SimplSharp.Reflection;
#else
using System.Reflection;
#endif
using ICD.Common.Utils.Collections;

namespace ICD.Connect.UI.Mvp
{
	/// <summary>
	/// Maps interfaces to their concrete types.
	/// </summary>
	public sealed class BindingMap<TAttribute>
		where TAttribute : Attribute, IUiBindingAttribute
	{
		private readonly Dictionary<Type, Type> m_InterfaceToConcrete;
		private readonly IcdHashSet<Assembly> m_CachedAssemblies;

		/// <summary>
		/// Constructor.
		/// </summary>
		public BindingMap()
		{
			m_InterfaceToConcrete = new Dictionary<Type, Type>();
			m_CachedAssemblies = new IcdHashSet<Assembly>();
		}

		#region Methods

		/// <summary>
		/// Creates the concrete instance for the given bound interface type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public T CreateInstance<T>(object[] parameters)
		{
			return (T)CreateInstance(typeof(T), parameters);
		}

		/// <summary>
		/// Creates the concrete instance for the given bound interface type.
		/// </summary>
		/// <param name="boundInterface"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public object CreateInstance(Type boundInterface, object[] parameters)
		{
			if (boundInterface == null)
				throw new ArgumentNullException("boundInterface");

			Type concrete = GetConcreteType(boundInterface);
			return ReflectionUtils.CreateInstance(concrete, parameters);
		}

		/// <summary>
		/// Gets all of the concrete types that are assignable to the given type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IEnumerable<Type> GetAssignableInterfaces(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			Assembly assembly = type.GetAssembly();
			CacheAssembly(assembly);

			return m_InterfaceToConcrete.Keys.Where(k => k.IsAssignableTo(type));
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the concrete type for the given bound interface type.
		/// </summary>=
		/// <returns></returns>
		private Type GetConcreteType(Type boundInterface)
		{
			if (boundInterface == null)
				throw new ArgumentNullException("boundInterface");

			Assembly assembly = boundInterface.GetAssembly();
			if (!m_CachedAssemblies.Contains(assembly))
				CacheAssembly(assembly);

			Type concrete;
			if (m_InterfaceToConcrete.TryGetValue(boundInterface, out concrete))
				return concrete;

			string message = string.Format("No binding found for {1}", boundInterface.Name);
			throw new KeyNotFoundException(message);
		}

		private void CacheAssembly(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			if (!m_CachedAssemblies.Add(assembly))
				return;

			foreach (var type in assembly.GetTypes())
			{
				if (!type.IsClass || type.IsAbstract || type.IsInterface)
					continue;

				IEnumerable<TAttribute> attributes = type.GetCustomAttributes<TAttribute>(true);
				foreach (TAttribute attribute in attributes)
					CacheType(attribute.InterfaceBinding, type);
			}
		}

		private void CacheType(Type interfaceType, Type type)
		{
			if (interfaceType == null)
				throw new ArgumentNullException("interfaceType");

			if (type == null)
				throw new ArgumentNullException("type");

			if (!type.IsAssignableTo(interfaceType))
				throw new ArgumentException(string.Format("{0} is not of type {1}", type, interfaceType));

			m_InterfaceToConcrete.Add(interfaceType, type);
		}

		#endregion
	}
}
