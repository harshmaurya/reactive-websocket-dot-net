using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReactiveWebsocket.PlatformAbstraction
{
    internal static class PlatformHelper
    {
        private static readonly List<string> KnownAssemblies = new List<string>
        {
            "ReactiveWebsocket.Desktop",
            "ReactiveWebsocket.Android",
        };

        private static Assembly _assembly;

        static PlatformHelper()
        {
            LoadPlatformSpecificAssembly();
        }

        public static T Resolve<T>()
        {
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
