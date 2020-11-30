using FastSocket.Connection;

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
