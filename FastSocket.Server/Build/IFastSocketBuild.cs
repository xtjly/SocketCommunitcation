using FastSocket.Server.Options;
using System;

namespace FastSocket.Server.Build
{
    public interface IFastSocketBuild
    {
        void ConfigureDefaultOptions(FastSocketBuildOption options);

        void ConfigFastSocketService(Action<IFastSocketService> p);

        IFastSocket Build();


    }
}
