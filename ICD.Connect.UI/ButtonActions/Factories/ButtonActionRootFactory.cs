using System;
using System.Linq;
using Crestron.SimplSharp.Reflection;
using ICD.Common.Properties;
using ICD.Common.Utils;
using ICD.Common.Utils.Collections;
using ICD.Common.Utils.Extensions;
using ICD.Common.Utils.Services.Logging;
using ICD.Connect.UI.ButtonActions.Actions;
using ICD.Common.Logging.LoggingContexts;
using ICD.Connect.UI.ButtonActions.Attributes;

namespace ICD.Connect.UI.ButtonActions.Factories
{
	public static class ButtonActionRootFactory
	{

		private static readonly ServiceLoggingContext s_Logger;

		private static readonly BiDictionary<string, Type> s_FactoryNameTypeMap;
		private static readonly SafeCriticalSection s_MapSection;

		static ButtonActionRootFactory()
		{
			s_Logger = new ServiceLoggingContext(typeof(ButtonActionRootFactory));
			s_FactoryNameTypeMap = new BiDictionary<string, Type>();
			s_MapSection = new SafeCriticalSection();


#if SIMPLSHARP
			CacheAssembly(typeof(ButtonActionRootFactory).GetCType().Assembly);
#else
			CacheAssembly(typeof(ButtonActionRootFactory).Assembly);
#endif
		}


		public static Type GetSettingsTypeForFactoryName(string factoryName)
		{
			s_MapSection.Enter();
			try
			{
				Type returnType;
				if (s_FactoryNameTypeMap.TryGetValue(factoryName, out returnType))
					return returnType;
			}
			finally
			{
				s_MapSection.Leave();
			}

			return null;
		}


		public static void CacheAssembly([NotNull] Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			Type[] types;

			try
			{
				types = assembly.GetTypes()
#if SIMPLSHARP
				                .Select(t => (Type)t)
#endif
				                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo<IButtonActionSettings>())
				                .ToArray();
			}
#if STANDARD
			catch (ReflectionTypeLoadException e)
			{
				foreach (Exception inner in e.LoaderExceptions)
				{
					if (inner is System.IO.FileNotFoundException)
					{
						s_Logger.Log(eSeverity.Error,
						             "Failed to cache assembly {0} - Could not find one or more dependencies by path",
						             assembly.GetName().Name);
						continue;
					}

					s_Logger.Log(eSeverity.Error, inner, "Failed to cache assembly {0}",
					             assembly.GetName().Name);
				}

				return;
			}
#endif
			catch (TypeLoadException e)
			{
#if SIMPLSHARP
				s_Logger.Log(eSeverity.Error, "Failed to cache assembly {0} - {1}", assembly.GetName().Name, e.Message);
#else
				s_Logger.Log(eSeverity.Error, "Failed to cache assembly {0} - could not load type {1}",
							 assembly.GetName().Name, e.TypeName);
#endif
				return;
			}

			foreach (Type type in types)
				CacheType(type);


		}

		public static void CacheType([NotNull]Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			try
			{
				ButtonActionSettingsAttribute attribute = AttributeUtils.GetClassAttribute<ButtonActionSettingsAttribute>(type, false);
				if (attribute == null)
					return;

				s_MapSection.Enter();
				try
				{


					if (s_FactoryNameTypeMap.ContainsKey(attribute.FactoryName))
					{
						s_Logger.Log(eSeverity.Error, "Failed to cache {0} - Duplicate factory name {1}", type.Name, attribute.FactoryName);
						return;
					}

					s_FactoryNameTypeMap.Add(attribute.FactoryName, type);
				}
				finally
				{
					s_MapSection.Leave();
				}
			}
			// GetMethods for Open Generic Types is not supported.
			catch (NotSupportedException)
			{
			}
			// Not sure why this happens :/
			catch (InvalidProgramException)
			{
			}
		}
	}
}