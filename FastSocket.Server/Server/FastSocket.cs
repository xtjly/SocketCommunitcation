using FastSocket.Connection;
using FastSocket.Options;
using FastSocket.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FastSocket
{
    public class FastSocket : IFastSocket
    {
        public readonly string Ip = string.Empty;
        public readonly int Port;
        public readonly int MaxConnections;
        public readonly int MaxTimeOutMillisecond;
        public readonly int MaxTransPortBodyMB;
        public readonly IFastSocketService FastSocketService;
        //
        public readonly Encoding Encoding;
        private readonly EnumSocketProtocolType SocketProtocolType = EnumSocketProtocolType.tcp;
        private Socket socket;
        private bool IsListen = false;
        private volatile int AutoGrowthConnectionId = 0;
        private readonly LinkedList<FastSocketConnection> connections;
        private bool ClearTag = true;

        public FastSocket(FastSocketBuildOption option, IFastSocketService fastSocketService)
        {
            this.Ip = option.Ip;
            this.Port = option.Port;
            this.MaxConnections = option.MaxConnections;
            this.MaxTimeOutMillisecond = option.MaxTimeOutMillisecond;
            this.MaxTransPortBodyMB = option.MaxTransPortBodyMB;
            //
            this.Encoding = Encoding.UTF8;
            this.SocketProtocolType = EnumSocketProtocolType.tcp;
            this.FastSocketService = fastSocketService;
            connections = new LinkedList<FastSocketConnection>();
        }

        //
        private void PrintConfigInfo() => Console.WriteLine($"FastSocket：Ip({this.Ip})，Port({this.Port})，MaxConnections({this.MaxConnections})，MaxTimeOutMillisecond({this.MaxTimeOutMillisecond})，SocketProtocolType({this.SocketProtocolType})，MaxTransPortBodyMB({this.MaxTransPortBodyMB})");

        private void HandleListenAsync()
        {
            if (this.IsListen)
            {
                this.socket.BeginAccept(asyncResult =>
                {
                    try
                    {
                        Socket newConnectionSocket = this.socket?.EndAccept(asyncResult);
                        if (this.IsListen) { this.HandleListenAsync(); }
                        else
                        {
                            newConnectionSocket.Close();
                            return;
                        }
                        if (this.connections.Count(p => p.Enable) >= this.MaxConnections)
                        {
                            newConnectionSocket.Close();
                            return;
                        }
                        else
                        {
                            FastSocketConnection fastSocketConnection = new FastSocketConnection(newConnectionSocket, (int)asyncResult.AsyncState, this);
                            fastSocketConnection.Start();
                            this.connections.AddLast(fastSocketConnection);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.FastSocketService.OnServiceException(this, ex);
                    }
                }, this.AutoGrowthConnectionId++);
            }
        }

        public void Run()
        {
            try
            {
                this.PrintConfigInfo();
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(this.Ip), this.Port);
                this.socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, (ProtocolType)((int)this.SocketProtocolType));
                this.socket.SendTimeout = this.MaxTimeOutMillisecond;
                this.socket.ReceiveTimeout = this.MaxTimeOutMillisecond;
                this.socket.Bind(iPEndPoint);
                this.socket.Listen(this.MaxConnections);
                this.IsListen = true;
                this.FastSocketService.OnServiceStarted(this);
                HandleListenAsync();
                HandleObjectClear();
                while (!"exit".Equals(Console.ReadLine().Trim())) { }
                this.Stop();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                this.FastSocketService.OnServiceException(this, ex);
            }
        }

        private void HandleObjectClear()
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 5000;
            timer.Enabled = true;
            timer.Elapsed += (o, a) =>
            {
                if (GetClearTag())
                {
                    SetClearTag(false);
                    foreach (var item in connections)
                    {
                        if (item.Enable == false)
                        {
                            connections.Remove(item);
                            item.Close();
                        }
                    }
                    SetClearTag(true);
                }
            };
            timer.Start();
        }

        private bool GetClearTag()
        {
            return this.ClearTag;
        }

        private void SetClearTag(bool tag)
        {
            this.ClearTag = tag;
        }

        public void Stop()
        {
            try
            {
                this.IsListen = false;
                this.socket?.Close();
                this.FastSocketService.OnServiceStoped(this);
            }
            catch (Exception ex)
            {
                this.FastSocketService.OnServiceException(this, ex);
            }
        }

        public void CloseOneConnection(FastSocketConnection fastSocketConnection)
        {
            if (fastSocketConnection != null)
            {
                fastSocketConnection.Close();
            }
        }
    }
}
