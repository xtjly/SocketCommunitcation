using FastSocket.Server.Connection;
using FastSocket.Server.Options;

namespace FastSocket.Server
{
    public interface IFastSocket
    {
        void Run();
        void Stop();
        void CloseOneConnection(FastSocketConnection fastSocketConnection);
        void CloseOneConnectionByConnectionID(int connectionID);
    }
}
