using FastSocket.Server.Comman;
using FastSocket.Server.Options;
using System;

namespace FastSocket.Server.Factory
{
    public static class SocketFactory
    {
        public static IFastSocketBuild CreateSocketBuild()
        {
            string jsonFilePath = GetJsonFilePath();
            FastSocketBuildOption option = JsonFileObj.GetJsonObjFromJsonFile<FastSocketBuildOption>(jsonFilePath);
            var isOk = option.IsOptionOk();
            if (isOk.Item1)
            {
                return CreateSocketBuild(option);
            }
            else
            {
                if (isOk.Item2 == null)
                {
                    return CreateSocketBuild(new FastSocketBuildOption
                    {
                        Ip = "127.0.0.1",
                        Port = 6188,
                        MaxConnections = 10,
                        MaxTimeOutMillisecond = 5000
                    });
                }
                else
                {
                    Console.WriteLine($"配置错误({isOk.Item2.Message})({isOk.Item2.StackTrace})");
                    throw isOk.Item2;
                }
            }
        }

        public static IFastSocketBuild CreateSocketBuild(FastSocketBuildOption options)
        {
            IFastSocketBuild fastSocketBuild = new FastSocketBuild();
            fastSocketBuild.ConfigureDefaultOptions(options);
            return fastSocketBuild;
        }

        private static string GetJsonFilePath()
        {

        }
    }
}
