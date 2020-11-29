using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.Server.Connection
{
    public class FastSocketConnection
    {
        public Socket ConnectionSocket { get; set; }
        private int connectionID;
        public int GetConnectionID() => this.connectionID;
        private Socket serverSocket;

        public FastSocketConnection(Socket newConnectionSocket, int connectionID, Socket serverSocket)
        {
            this.ConnectionSocket = newConnectionSocket;
            this.connectionID = connectionID;
            this.serverSocket = serverSocket;
        }

        internal void Start()
        {
            //throw new NotImplementedException();
        }
    }
}
