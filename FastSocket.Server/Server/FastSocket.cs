using FastSocket.Server.Connection;
using FastSocket.Server.Options;
using FastSocket.Server.Build;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.Server
{
    public class FastSocket : IFastSocket
    {
        private string Ip = string.Empty;
        private int Port;
        private int MaxConnections;
        private int MaxTimeOutMillisecond;
        private int MaxTransPortBodyMB;
        private IFastSocketService FastSocketService;
        //
        private EnumSocketProtocolType SocketProtocolType = EnumSocketProtocolType.tcp;
        private Socket socket;
        private bool IsListen = false;
        private int AutoGrowthConnectionId = 0;
        private LinkedList<FastSocketConnection> FastSocketConnections = new LinkedList<FastSocketConnection>();
        //
        private void PrintConfigInfo() => Console.WriteLine($"FastSocket：Ip({this.Ip})，Port({this.Port})，MaxConnections({this.MaxConnections})，MaxTimeOutMillisecond({this.MaxTimeOutMillisecond})，SocketProtocolType({this.SocketProtocolType})，MaxTransPortBodyMB({this.MaxTransPortBodyMB})");

        public void ConfigOptions(FastSocketBuildOption option)
        {
            this.Ip = option.Ip;
            this.Port = option.Port;
            this.MaxConnections = option.MaxConnections;
            this.MaxTimeOutMillisecond = option.MaxTimeOutMillisecond;
            this.MaxTransPortBodyMB = option.MaxTransPortBodyMB;
            //
            this.SocketProtocolType = EnumSocketProtocolType.tcp;
        }

        public void ConfigService(IFastSocketService fastSocketService)
        {
            this.FastSocketService = fastSocketService;
        }

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
            HandleListenAsync();
        }

        public void Close()
        {
            this.IsListen = false;
            this.socket?.Close();
            this.socket = null;
            this.FastSocketConnections = new LinkedList<FastSocketConnection>();
        }

        public void CloseOneConnection(FastSocketConnection fastSocketConnection)
        {
            FastSocketConnection connection = this.FastSocketConnections?.Find(fastSocketConnection)?.Value;
            this.FastSocketConnections?.Remove(connection);

        }

        public void CloseOneConnectionByConnectionID(int connectionID)
        {
            throw new NotImplementedException();
        }
    }
}
