using System;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.PlatformAbstraction;

namespace ReactiveWebsocket.Public
{
    public static class WebsocketInitializer
    {
        public static void SetPlatform(PlatformName platform)
        {
            string assemblyName = string.Empty;
            switch (platform)
            {
                case PlatformName.Desktop:
                    {
                        assemblyName = PlatformHelper.DesktopAssembly;
                        break;
                    }

                case PlatformName.Android:
                    {
                        assemblyName = PlatformHelper.AndroidAssembly;
                        break;
                    }

                case PlatformName.Ios:
                    {
                        assemblyName = PlatformHelper.IosAssembly;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
            if (string.IsNullOrEmpty(assemblyName))
                throw new InvalidOperationException("Unknown Platform");
            PlatformHelper.LoadSpecificAssembly(assemblyName);
        }
    }
}
