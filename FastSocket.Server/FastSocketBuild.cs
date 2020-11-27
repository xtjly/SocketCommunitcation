using FastSocket.Server.Options;
using System;

namespace FastSocket.Server
{
    public class FastSocketBuild : IFastSocketBuild
    {
        public void ConfigureDefaultOptions(FastSocketBuildOption options)
        {
            fastSocketBuildOption = options;
        }

        public void ConfigFastSocketService()
        {
            Action_FastSocketService = new Action<IFastSocketService>(iFastSocketService =>
            {

            });
        }

        public void ConfigFastSocketService(Action<IFastSocketService> action)
        {
            Action_FastSocketService = action;
        }

        FastSocketBuildOption fastSocketBuildOption = null;

        Action<IFastSocketService> Action_FastSocketService = null;


        public IFastSocket Build()
        {
            IFastSocket fastSocket = new FastSocket();
            IFastSocketService fastSocketService = new FastSocketService();
            Action_FastSocketService(fastSocketService);
            fastSocket.ConfigOptions(fastSocketBuildOption);
            fastSocket.ConfigService(fastSocketService);
            return fastSocket;
        }
    }
}
