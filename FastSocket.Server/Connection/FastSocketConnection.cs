using FastSocket.Server;
using System;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.Connection
{
    public class FastSocketConnection : IDisposable
    {
        private readonly Socket ConnectionSocket;
        public readonly int ConnectionID;
        private readonly FastSocket serverSocket;
        private readonly Encoding Encoding;
        private readonly IFastSocketService FastSocketService;
        internal volatile bool Enable;

        public FastSocketConnection(Socket newConnectionSocket, int connectionID, FastSocket serverSocket)
        {
            this.ConnectionSocket = newConnectionSocket;
            this.ConnectionSocket.SendTimeout = serverSocket.MaxTimeOutMillisecond;
            this.ConnectionSocket.ReceiveTimeout = serverSocket.MaxTimeOutMillisecond;
            this.ConnectionID = connectionID;
            this.serverSocket = serverSocket;
            this.Encoding = serverSocket.Encoding;
            this.FastSocketService = serverSocket.FastSocketService;
            this.Enable = true;
        }

        internal void Start()
        {
            this.serverSocket.Connections.AddLast(this);
            this.FastSocketService.OnConnectionConnected(this.serverSocket, this);
            HandleReceiveMsg();
            HandleConnectionClose();
        }

        private void HandleConnectionClose()
        {
            var timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Enabled = true;
            timer.Elapsed += (o, a) =>
            {
                Console.WriteLine(this.ConnectionID + "连接中");
                bool connected = true;
                try
                {
                    if (this.Enable && !IsConnected()) connected = false;
                }
                catch (Exception ex)
                {
                    this.FastSocketService.OnServiceException(this.serverSocket, ex);
                    connected = false;
                }
                if (!connected)
                {
                    timer.Stop();
                    timer.Enabled = false;
                    timer.Close();
                    if (this.Enable)
                    {
                        this.Enable = false;
                        this.serverSocket.Connections.Remove(this);
                        this.FastSocketService.OnConnectionClosed(this.serverSocket, this.ConnectionID);
                        this.Dispose();
                    }
                }
            };
            timer.Start();
        }

        private void HandleReceiveMsg()
        {
            byte[] buffer = new byte[1024 * 1024 * this.serverSocket.MaxTransPortBodyMB];
            this.ConnectionSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, asyncResult =>
            {
                try
                {
                    int length = this.ConnectionSocket?.EndReceive(asyncResult) ?? 0;

                    if (length > 0)
                    {
                        this.HandleReceiveMsg();
                        byte[] data = new byte[length];
                        Array.Copy(buffer, 0, data, 0, length);
                        this.FastSocketService.OnReceiveMsg(this.serverSocket, this, data);
                    }
                }
                catch (Exception ex)
                {
                    this.FastSocketService.OnServiceException(this.serverSocket, ex);
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
            this.ConnectionSocket.Close();
        }

        private bool Poll(int vs, SelectMode mode)
        {
            if (!(this?.ConnectionSocket?.Poll(vs, mode) ?? true))
            {
                return true;
            }
            return false;
        }

        private bool IsConnected()
        {
            if (this == null || !this.Enable) return false;
            return Poll(this?.serverSocket.MaxTimeOutMillisecond * 1000 ?? 2000 * 1000, SelectMode.SelectRead);
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
