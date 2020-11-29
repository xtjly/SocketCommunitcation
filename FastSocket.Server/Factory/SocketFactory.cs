using FastSocket.Server.Build;
using FastSocket.Server.Comman;
using FastSocket.Server.Options;
using System;
using System.IO;

namespace FastSocket.Server.Factory
{
    public static class SocketFactory
    {
        public static IFastSocketBuild CreateSocketBuild()
        {
            string jsonConfigFilePath = GetJsonConfigFilePath();
            FastSocketBuildOption option = JsonFileObj.GetJsonObjFromJsonFile<FastSocketBuildOption>(jsonConfigFilePath);
            if (string.IsNullOrWhiteSpace(jsonConfigFilePath))
            {
                return CreateSocketBuild(new FastSocketBuildOption
                {
                    Ip = "127.0.0.1",
                    Port = 6188,
                    MaxConnections = 10,
                    MaxTimeOutMillisecond = 5000,
                    MaxTransPortBodyMB = 2
                });
            }
            return CreateSocketBuild(option);
        }

        public static IFastSocketBuild CreateSocketBuild(FastSocketBuildOption options)
        {
            IFastSocketBuild fastSocketBuild = new FastSocketBuild();
            fastSocketBuild.ConfigureDefaultOptions(options);
            return fastSocketBuild;
        }

        private static string GetJsonConfigFilePath()
        {
            string jsonConfigFilePath = Path.Combine(Environment.CurrentDirectory, "fastsocket.json");
            if (File.Exists(jsonConfigFilePath))
            {
                return jsonConfigFilePath;
            }
            else
            {
                return null;
            }
        }
    }
}
