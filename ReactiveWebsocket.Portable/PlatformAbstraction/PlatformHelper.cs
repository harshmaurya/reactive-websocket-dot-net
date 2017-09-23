using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReactiveWebsocket.PlatformAbstraction
{
    internal static class PlatformHelper
    {
        public const string DesktopAssembly = "ReactiveWebsocket.Desktop";
        public const string AndroidAssembly = "ReactiveWebsocket.Android";
        public const string IosAssembly = "ReactiveWebsocket.Ios";
        private static bool _isInitialized = false;
        private static readonly List<string> KnownAssemblies = new List<string>
        {
            DesktopAssembly,
            AndroidAssembly,
            IosAssembly
        };

        private static Assembly _assembly;

        public static void LoadSpecificAssembly(string assemblyName)
        {
            var assembly = LoadAssembly(assemblyName);
            _assembly = assembly ?? throw new InvalidOperationException($"Assembly {assemblyName} not found");
            _isInitialized = true;
        }

        private static void Initialize()
        {
            if (_isInitialized) return;
            LoadPlatformSpecificAssembly();
        }

        public static T Resolve<T>()
        {
            Initialize();
            var type = typeof(T);
            var typeToCreate = _assembly.DefinedTypes.First(info => info.ImplementedInterfaces.Contains(type));
            var instance = Activator.CreateInstance(typeToCreate.AsType());
            return (T)instance;
        }

        private static void LoadPlatformSpecificAssembly()
        {
            Assembly platformSpecific = null;
            foreach (var knownAssembly in KnownAssemblies)
            {
                platformSpecific = LoadAssembly(knownAssembly);
                if (platformSpecific != null) break;
            }
            _assembly = platformSpecific ?? throw new Exception("Could not find platform specific assembly");
        }


        private static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(new AssemblyName(assemblyName));
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }
    }
}
