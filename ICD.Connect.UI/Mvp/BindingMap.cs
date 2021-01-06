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
		private readonly BiDictionary<Type, Type> m_InterfaceToConcrete;
		private readonly IcdHashSet<Assembly> m_CachedAssemblies;
		private readonly SafeCriticalSection m_CacheSection;

		/// <summary>
		/// Constructor.
		/// </summary>
		public BindingMap()
		{
			m_InterfaceToConcrete = new BiDictionary<Type, Type>();
			m_CachedAssemblies = new IcdHashSet<Assembly>();
			m_CacheSection = new SafeCriticalSection();
		}

		#region Methods

		/// <summary>
		/// Gets the interface type for the given concrete type.
		/// </summary>
		/// <returns></returns>
		public Type GetInterfaceType(Type concrete)
		{
			if (concrete == null)
				throw new ArgumentNullException("concrete");

			if (concrete.IsInterface)
				throw new ArgumentException("Type must be concrete");

			m_CacheSection.Enter();

			try
			{
				Assembly assembly = concrete.GetAssembly();
				if (!m_CachedAssemblies.Contains(assembly))
					CacheAssembly(assembly);

				Type boundInterface;
				if (m_InterfaceToConcrete.TryGetKey(concrete, out boundInterface))
					return boundInterface;
			}
			finally
			{
				m_CacheSection.Leave();
			}

			string message = string.Format("No interface found for {0}", concrete.Name);
			throw new KeyNotFoundException(message);
		}

		/// <summary>
		/// Gets the concrete type for the given bound interface type.
		/// </summary>
		/// <returns></returns>
		public Type GetConcreteType(Type boundInterface)
		{
			if (boundInterface == null)
				throw new ArgumentNullException("boundInterface");

			if (!boundInterface.IsInterface)
				throw new ArgumentException("Type must be an interface");

			m_CacheSection.Enter();

			try
			{
				Assembly assembly = boundInterface.GetAssembly();
				if (!m_CachedAssemblies.Contains(assembly))
					CacheAssembly(assembly);

				Type concrete;
				if (m_InterfaceToConcrete.TryGetValue(boundInterface, out concrete))
					return concrete;
			}
			finally
			{
				m_CacheSection.Leave();
			}

			string message = string.Format("No binding found for {0}", boundInterface.Name);
			throw new KeyNotFoundException(message);
		}

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

			if (!boundInterface.IsInterface)
				throw new ArgumentException("Type must be an interface");

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

			m_CacheSection.Enter();

			try
			{
				Assembly assembly = type.GetAssembly();
				CacheAssembly(assembly);

				return m_InterfaceToConcrete.Keys.Where(k => k.IsAssignableTo(type));
			}
			finally
			{
				m_CacheSection.Leave();
			}
		}

		#endregion

		#region Private Methods

		private void CacheAssembly(Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			m_CacheSection.Enter();

			try
			{
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
			finally
			{
				m_CacheSection.Leave();
			}
		}

		private void CacheType(Type interfaceType, Type type)
		{
			if (interfaceType == null)
				throw new ArgumentNullException("interfaceType");

			if (!interfaceType.IsInterface)
				throw new ArgumentException("Interface Type must be an interface");

			if (type == null)
				throw new ArgumentNullException("type");

			if (!type.IsAssignableTo(interfaceType))
				throw new ArgumentException(string.Format("{0} is not of type {1}", type, interfaceType));

			m_CacheSection.Execute(() => m_InterfaceToConcrete.Add(interfaceType, type));
		}

		#endregion
	}
}
