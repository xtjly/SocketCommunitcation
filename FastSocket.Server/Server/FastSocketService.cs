using FastSocket.Connection;
using System;

namespace FastSocket.Server
{
    public class FastSocketService : IFastSocketService
    {
        public Action<FastSocket, FastSocketConnection> OnConnectionConnected { get; set; } = new Action<FastSocket, FastSocketConnection>((p1, p2) => { });
        public Action<FastSocket, FastSocketConnection, byte[]> OnReceiveMsg { get; set; } = new Action<FastSocket, FastSocketConnection, byte[]>((p1, p2, p3) => { });
        public Action<FastSocket, FastSocketConnection, byte[]> OnSendMsg { get; set; } = new Action<FastSocket, FastSocketConnection, byte[]>((p1, p2, p3) => { });
        public Action<FastSocket, FastSocketConnection> OnConnectionClosed { get; set; } = new Action<FastSocket, FastSocketConnection>((p1, p2) => { });
        public Action<FastSocket, Exception> OnServiceException { get; set; } = new Action<FastSocket, Exception>((p1, p2) => { });
        public Action<FastSocket> OnServiceStarted { get; set; } = new Action<FastSocket>(p => { });
        public Action<FastSocket> OnServiceStoped { get; set; } = new Action<FastSocket>(p => { });

        public IFastSocketService AddOnConnectionConnected(Action<FastSocket, FastSocketConnection> action)
        {
            this.OnConnectionConnected = action;
            return this;
        }

        public IFastSocketService AddOnReceiveMsg(Action<FastSocket, FastSocketConnection, byte[]> action)
        {
            this.OnReceiveMsg = action;
            return this;
        }

        public IFastSocketService AddOnSendMsg(Action<FastSocket, FastSocketConnection, byte[]> action)
        {
            this.OnSendMsg = action;
            return this;
        }

        public IFastSocketService AddOnConnectionClosed(Action<FastSocket, FastSocketConnection> action)
        {
            this.OnConnectionClosed = action;
            return this;
        }

        public IFastSocketService AddOnServiceException(Action<FastSocket, Exception> action)
        {
            this.OnServiceException = action;
            return this;
        }

        public IFastSocketService AddOnServiceStarted(Action<FastSocket> action)
        {
            this.OnServiceStarted = action;
            return this;
        }

        public IFastSocketService AddOnServiceStoped(Action<FastSocket> action)
        {
            this.OnServiceStoped = action;
            return this;
        }
    }
}
