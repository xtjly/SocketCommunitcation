using FastSocket.Server.Connection;
using System;

namespace FastSocket.Server
{
    public class FastSocketService : IFastSocketService
    {
        public Action<FastSocket, FastSocketConnection> OnConnectionConnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<FastSocket, FastSocketConnection, byte[]> OnReceiveMsg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<FastSocket, FastSocketConnection, byte[]> OnSendMsg { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<FastSocket, FastSocketConnection> OnConnectionCloseed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<FastSocket, Exception> OnServiceException { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<FastSocket> OnServiceStarted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<FastSocket> OnServiceStoped { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        public IFastSocketService AddOnConnectionCloseed(Action<FastSocket, FastSocketConnection> action)
        {
            this.OnConnectionCloseed = action;
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
