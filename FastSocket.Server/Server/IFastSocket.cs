using FastSocket.Server.Connection;
using FastSocket.Server.Options;

namespace FastSocket.Server
{
    public interface IFastSocket
    {
        void ConfigOptions(FastSocketBuildOption fastSocketBuildOption);
        void ConfigService(IFastSocketService fastSocketService);

        void Run();
        void Close();
        void CloseOneConnection(FastSocketConnection fastSocketConnection);
        void CloseOneConnectionByConnectionID(int connectionID);
    }
}
