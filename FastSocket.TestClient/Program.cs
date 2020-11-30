using System;
using System.Net;
using System.Net.Sockets;

namespace FastSocket.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6188);
            Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            StartConnect(socket, iPEndPoint);
            while (true) { };
        }

        private static void StartConnect(Socket socket, IPEndPoint iPEndPoint)
        {
            socket.BeginConnect(iPEndPoint, astncResult =>
            {
                socket.EndConnect(astncResult);
                StartConnect(socket, iPEndPoint);//

                socket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
            }, null);

        }

    }
}
