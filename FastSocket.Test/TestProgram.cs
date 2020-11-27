using FastSocket.Server;
using FastSocket.Server.Factory;
using FastSocket.Server.Options;
using System;

namespace FastSocket.Test
{
    class TestProgram
    {
        static void Main(string[] args)
        {
            IFastSocketBuild fastSocketBuild = SocketFactory.CreateSocketBuild();
            fastSocketBuild.ConfigFastSocketService();
            IFastSocket fastSocket = fastSocketBuild.Build();
            fastSocket.Run();
        }
    }
}
