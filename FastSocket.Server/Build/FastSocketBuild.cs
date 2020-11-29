using FastSocket.Server.Options;
using System;

namespace FastSocket.Server.Build
{
    public class FastSocketBuild : IFastSocketBuild
    {
        public void ConfigureDefaultOptions(FastSocketBuildOption options)
        {
            fastSocketBuildOption = options;
        }

        public void ConfigFastSocketService(Action<IFastSocketService> action)
        {
            Action_FastSocketService = action;
        }

        FastSocketBuildOption fastSocketBuildOption = null;
        Action<IFastSocketService> Action_FastSocketService = null;

        public IFastSocket Build()
        {
            var result = fastSocketBuildOption.IsConfigSuccess();
            var configResult = new
            {
                IsSuccess = result.Item1,
                ErrorException = result.Item2
            };
            if (!configResult.IsSuccess)
            {
                Console.WriteLine($"配置错误({configResult.ErrorException.Message})({configResult.ErrorException.StackTrace})");
                throw configResult.ErrorException;
            }

            IFastSocket fastSocket = new FastSocket();
            IFastSocketService fastSocketService = new FastSocketService();
            Action_FastSocketService(fastSocketService);
            fastSocket.ConfigOptions(fastSocketBuildOption);
            fastSocket.ConfigService(fastSocketService);
            return fastSocket;
        }
    }
}
