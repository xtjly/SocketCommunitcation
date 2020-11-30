using FastSocket.Server;
using System;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.Connection
{
    public class FastSocketConnection
    {
        private readonly Socket ConnectionSocket;
        public readonly int ConnectionID;
        private readonly FastSocket serverSocket;
        private readonly Encoding Encoding;
        private readonly IFastSocketService FastSocketService;

        public FastSocketConnection(Socket newConnectionSocket, int connectionID, FastSocket serverSocket)
        {
            this.ConnectionSocket = newConnectionSocket;
            this.ConnectionID = connectionID;
            this.serverSocket = serverSocket;
            this.Encoding = serverSocket.Encoding;
            this.FastSocketService = serverSocket.FastSocketService;
        }

        internal void Start()
        {
            this.LoadOnReceiveMsgEventAsync();
        }

        private void LoadOnReceiveMsgEventAsync()
        {
            byte[] buffer = new byte[1024 * 1024 * this.serverSocket.MaxTransPortBodyMB];
            this.ConnectionSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, asyncResult =>
            {
                int length = this.ConnectionSocket.EndReceive(asyncResult);
                this.LoadOnReceiveMsgEventAsync();
                if (length > 0)
                {
                    byte[] data = new byte[length];
                    Array.Copy(buffer, 0, data, 0, length);
                    this.FastSocketService.OnReceiveMsg(this.serverSocket, this, data);
                }
            }, this.ConnectionID);
        }

        public bool Send(byte[] sendBytes)
        {
            int? length = this.ConnectionSocket?.Send(sendBytes);
            if (length == sendBytes?.Length)
            {
                this.FastSocketService.OnSendMsg(serverSocket, this, sendBytes);
                return true;
            }
            return false;
        }

        public bool Send(string sendString)
        {
            byte[] sendBytes = this.Encoding.GetBytes(sendString);
            int? length = this.ConnectionSocket?.Send(sendBytes);
            if (length == sendBytes?.Length)
            {
                this.FastSocketService.OnSendMsg(serverSocket, this, sendBytes);
                return true;
            }
            return false;
        }

        public void Close()
        {
            if (this?.ConnectionSocket != null)
            {
                this.ConnectionSocket.Close();
            }
        }

        public bool Poll(int ms, SelectMode mode)
        {
            if (this.ConnectionSocket.Poll(ms * 1000, mode))
            {
                return true;
            }
            return false;
        }
    }
}
