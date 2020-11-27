using FastSocket.Server.Options;
using System;

namespace FastSocket.Server
{
    public interface IFastSocketBuild
    {
        void ConfigureDefaultOptions(FastSocketBuildOption options);

        void ConfigFastSocketService();
        void ConfigFastSocketService(Action<IFastSocketService> p);

        IFastSocket Build();


    }
}
