using FastSocket.Options;
using FastSocket.Server;
using System;

namespace FastSocket.Build
{
    public interface IFastSocketBuild
    {
        void ConfigureDefaultOptions(FastSocketBuildOption options);

        void ConfigFastSocketService(Action<IFastSocketService> p);

        IFastSocket Build();


    }
}
