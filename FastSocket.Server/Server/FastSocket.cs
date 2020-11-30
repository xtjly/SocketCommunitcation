using FastSocket.Connection;
using FastSocket.Options;
using FastSocket.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

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
        private int AutoGrowthConnectionId = 0;
        public readonly List<FastSocketConnection> FastSocketConnections = new List<FastSocketConnection>();

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
                    try
                    {
                        Socket newConnectionSocket = this.socket?.EndAccept(asyncResult);
                        if (this.IsListen) { this.HandleListenAsync(); }
                        else { return; }
                        FastSocketConnection fastSocketConnection = new FastSocketConnection(newConnectionSocket, (int)asyncResult.AsyncState, this);
                        this.FastSocketService.OnConnectionConnected(this, fastSocketConnection);
                        fastSocketConnection.Start();
                        this.FastSocketConnections.Add(fastSocketConnection);
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
                HandleConnectionClosedAsync();
                HandleListenAsync();
                while (!"exit".Equals(Console.ReadLine().Trim())) { }
                this.Stop();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                this.FastSocketService.OnServiceException(this, ex);
            }
        }

        private void HandleConnectionClosedAsync()
        {
            ThreadPool.QueueUserWorkItem(OnConnectionClose);
        }

        private void OnConnectionClose(object state)
        {
            while (true)
            {
                try
                {
                    if (this.FastSocketConnections.Count > 0 && this.IsListen)
                    {
                        for (int i = this.FastSocketConnections.Count - 1; i >= 0; i--)
                        {
                            if (this.IsListen)
                            {
                                if (this.FastSocketConnections[i].IsEnable)
                                {
                                    if (!this.FastSocketConnections[i].IsConnected())
                                    {
                                        this.FastSocketConnections[i].Close();
                                        this.FastSocketConnections[i].CloseConnectionSocketWhenNoEnable();
                                        FastSocketConnection theConnection = this.FastSocketConnections[i];
                                        this.FastSocketConnections.RemoveAt(i--);
                                        this.FastSocketService.OnConnectionCloseed(this, theConnection);
                                        continue;
                                    }
                                }
                                else
                                {
                                    this.FastSocketConnections[i].CloseConnectionSocketWhenNoEnable();
                                    FastSocketConnection theConnection = this.FastSocketConnections[i];
                                    this.FastSocketConnections.RemoveAt(i--);
                                    this.FastSocketService.OnConnectionCloseed(this, theConnection);
                                    continue;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.FastSocketService.OnServiceException(this, ex);
                }
                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            try
            {

                this.IsListen = false;
                this.socket?.Close();
                this.FastSocketConnections.Clear();
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
            theConnection.Close();
        }
    }
}
