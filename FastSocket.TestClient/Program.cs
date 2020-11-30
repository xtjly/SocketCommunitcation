using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FastSocket.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6188);
            Socket clientSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(iPEndPoint);

            //send message
            string sendStr = $"111send to server : hello,ni hao";
            byte[] sendBytes = Encoding.UTF8.GetBytes(sendStr);
            clientSocket.Send(sendBytes);

            //receive message
            string recStr = "";
            byte[] recBytes = new byte[4096];
            int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
            recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
            Console.WriteLine(recStr);

            clientSocket.Close();
        }
    }
}
