using FastSocket.Server.Connection;
using FastSocket.Server.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace FastSocket.Server
{
    public class FastSocket : IFastSocket
    {
        public readonly string Ip = string.Empty;
        public readonly int Port;
        public readonly int MaxConnections;
        public readonly int MaxTimeOutMillisecond;
        public readonly int MaxTransPortBodyMB;
        private readonly IFastSocketService FastSocketService;
        //
        private readonly EnumSocketProtocolType SocketProtocolType = EnumSocketProtocolType.tcp;
        private Socket socket;
        private bool IsListen = false;
        private int AutoGrowthConnectionId = 0;
        public readonly LinkedList<FastSocketConnection> FastSocketConnections = new LinkedList<FastSocketConnection>();

        public FastSocket(FastSocketBuildOption option, IFastSocketService fastSocketService)
        {
            this.Ip = option.Ip;
            this.Port = option.Port;
            this.MaxConnections = option.MaxConnections;
            this.MaxTimeOutMillisecond = option.MaxTimeOutMillisecond;
            this.MaxTransPortBodyMB = option.MaxTransPortBodyMB;
            //
            this.SocketProtocolType = EnumSocketProtocolType.tcp;
            this.FastSocketService = fastSocketService;
        }

        //
        private void PrintConfigInfo() => Console.WriteLine($"FastSocket：Ip({this.Ip})，Port({this.Port})，MaxConnections({this.MaxConnections})，MaxTimeOutMillisecond({this.MaxTimeOutMillisecond})，SocketProtocolType({this.SocketProtocolType})，MaxTransPortBodyMB({this.MaxTransPortBodyMB})");

        private void HandleListenAsync()
        {
            //测试MaxConnections是否生效
            if (this.IsListen)
            {
                this.socket.BeginAccept(asyncResult =>
                {
                    Socket newConnectionSocket = this.socket.EndAccept(asyncResult);
                    if (this.IsListen) { HandleListenAsync(); }
                    else { return; }
                    FastSocketConnection fastSocketConnection = new FastSocketConnection(newConnectionSocket, (int)asyncResult.AsyncState, this.socket);
                    this.FastSocketService.OnConnectionConnected(this, fastSocketConnection);
                    fastSocketConnection.Start();
                    this.FastSocketConnections.AddLast(fastSocketConnection);

                }, this.AutoGrowthConnectionId++);
            }
        }

        public void Run()
        {
            this.PrintConfigInfo();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(this.Ip), this.Port);
            this.socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, (ProtocolType)((int)this.SocketProtocolType));
            this.socket.Listen(this.MaxConnections);
            this.IsListen = true;
            this.FastSocketService.OnServiceStarted(this);
            HandleListenAsync();
        }

        public void Stop()
        {
            this.IsListen = false;
            this.socket?.Close();
            this.socket = null;
            this.FastSocketConnections.Clear();
            this.FastSocketService.OnServiceStoped(this);
        }

        public void CloseOneConnection(FastSocketConnection fastSocketConnection)
        {
            if (fastSocketConnection != null && fastSocketConnection?.ConnectionSocket != null)
            {
                this.FastSocketConnections.Remove(fastSocketConnection);
                fastSocketConnection.ConnectionSocket.Close();
            }
        }

        public void CloseOneConnectionByConnectionID(int connectionID)
        {
            FastSocketConnection theConnection = null;
            foreach (FastSocketConnection connection in this.FastSocketConnections)
            {
                if (connection.ConnectionID == connectionID)
                {
                    theConnection = connection;
                    break;
                }
            }
            this.FastSocketConnections.Remove(theConnection);
            theConnection.ConnectionSocket.Close();
        }
    }
}
