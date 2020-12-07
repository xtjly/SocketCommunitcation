using FastSocket.Connection;
using System;

namespace FastSocket.Server
{
    public interface IFastSocketService
    {
        Action<FastSocket, FastSocketConnection> OnConnectionConnected { get; set; }
        Action<FastSocket, FastSocketConnection, byte[]> OnReceiveMsg { get; set; }
        Action<FastSocket, FastSocketConnection, byte[]> OnSendMsg { get; set; }
        Action<FastSocket, FastSocketConnection> OnConnectionClosed { get; set; }
        Action<FastSocket, Exception> OnServiceException { get; set; }
        Action<FastSocket> OnServiceStarted { get; set; }
        Action<FastSocket> OnServiceStoped { get; set; }

        IFastSocketService AddOnConnectionConnected(Action<FastSocket, FastSocketConnection> action);
        IFastSocketService AddOnReceiveMsg(Action<FastSocket, FastSocketConnection, byte[]> action);
        IFastSocketService AddOnSendMsg(Action<FastSocket, FastSocketConnection, byte[]> action);
        IFastSocketService AddOnConnectionClosed(Action<FastSocket, FastSocketConnection> action);
        IFastSocketService AddOnServiceException(Action<FastSocket, Exception> action);
        IFastSocketService AddOnServiceStarted(Action<FastSocket> action);
        IFastSocketService AddOnServiceStoped(Action<FastSocket> action);
    }
}
