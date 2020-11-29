using FastSocket.Server.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastSocket.Server
{
    public class FastSocket : IFastSocket
    {
        private string Ip { get; set; }
        private int Port { get; set; }
        private int MaxConnections { get; set; }
        private int MaxTimeOutMillisecond { get; set; }

        public void ConfigOptions(FastSocketBuildOption option)
        {
            this.Ip = option.Ip;
            this.Port = option.Port;
            this.MaxConnections = option.MaxConnections;
            this.MaxTimeOutMillisecond = option.MaxTimeOutMillisecond;
        }

        public void ConfigService(IFastSocketService fastSocketService)
        {
            //throw new NotImplementedException();
        }

        public void PrintConfigInfo()
        {
            Console.WriteLine($"FastSocket：Ip({this.Ip})，Port({this.Port})，MaxConnections({this.MaxConnections})，MaxTimeOutMillisecond({this.MaxTimeOutMillisecond})");
        }

        public void Run()
        {
            this.PrintConfigInfo();

        }
    }
}
