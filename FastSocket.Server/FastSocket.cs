using FastSocket.Server.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.Server
{
    public class FastSocket : IFastSocket
    {
        private string Ip { get; set; }
        private int Port { get; set; }
        private int MaxConnections { get; set; }
        private int MaxTimeOutMillisecond { get; set; }
        private EnumSocketProtocolType SocketProtocolType { get; set; }

        public void ConfigOptions(FastSocketBuildOption option)
        {
            this.Ip = option.Ip;
            this.Port = option.Port;
            this.MaxConnections = option.MaxConnections;
            this.MaxTimeOutMillisecond = option.MaxTimeOutMillisecond;

            //
            this.SocketProtocolType = EnumSocketProtocolType.tcp;
        }

        public void ConfigService(IFastSocketService fastSocketService)
        {
            //throw new NotImplementedException();
        }

        private void PrintConfigInfo()
        {
            Console.WriteLine($"FastSocket：Ip({this.Ip})，Port({this.Port})，MaxConnections({this.MaxConnections})，MaxTimeOutMillisecond({this.MaxTimeOutMillisecond})，SocketProtocolType({this.SocketProtocolType})");
        }

        public void Run()
        {
            this.PrintConfigInfo();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(this.Ip), this.Port);
            Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, (ProtocolType)((int)SocketProtocolType));

        }
    }
}
