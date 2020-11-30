using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.Server.Connection
{
    public class FastSocketConnection
    {
        public readonly Socket ConnectionSocket;
        public readonly int ConnectionID;
        private Socket serverSocket;

        public FastSocketConnection(Socket newConnectionSocket, int connectionID, Socket serverSocket)
        {
            this.ConnectionSocket = newConnectionSocket;
            this.ConnectionID = connectionID;
            this.serverSocket = serverSocket;
        }

        internal void Start()
        {
            //throw new NotImplementedException();
        }
    }
}
