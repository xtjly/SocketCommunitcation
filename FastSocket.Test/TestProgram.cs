using FastSocket.Build;
using FastSocket.Factory;
using FastSocket.Options;
using FastSocket.Server;
using System;

namespace FastSocket.Test
{
    class TestProgram
    {
        static void Main(string[] args)
        {
            //1)可自定义参数
            IFastSocketBuild fastSocketBuild1 = SocketFactory.CreateSocketBuild(new FastSocketBuildOption
            {
                Ip = "127.0.0.1",               //ip
                Port = 6188,                    //port
                MaxConnections = 3,             //最大连接数
                MaxTimeOutMillisecond = 5000,   //最大超时时间毫秒ms
                MaxTransPortBodyMB = 2          //最大传输内容大小MB
            });
            //1)也可默认使用配置文件 fastsocket.json
            IFastSocketBuild fastSocketBuild = SocketFactory.CreateSocketBuild();
            fastSocketBuild.ConfigFastSocketService(fastSocketService =>
            {
                fastSocketService
                .AddOnConnectionConnected((serverSocket, connection) =>
                {
                    Console.WriteLine($"Socket服务，ConnectionId({connection.ConnectionID})，接入链接");
                }).AddOnReceiveMsg((serverSocket, connection, receiveBytes) =>
                {
                    Console.WriteLine($"Socket服务，ConnectionId({connection?.ConnectionID})，从客户端接收到消息：({serverSocket.Encoding.GetString(receiveBytes)})");
                    connection.Send($"ConnectionId({connection?.ConnectionID})服务端返回的数据");
                }).AddOnSendMsg((serverSocket, connection, sendBytes) =>
                {
                    Console.WriteLine($"Socket服务，ConnectionId({connection?.ConnectionID})，发送消息至客户端：({serverSocket.Encoding.GetString(sendBytes)})");
                }).AddOnConnectionClosed((serverSocket, connection) =>
                {
                    Console.WriteLine($"Socket服务，ConnectionId({connection?.ConnectionID})，关闭链接");
                }).AddOnServiceException((serverSocket, exception) =>
                {
                    Console.WriteLine($"Socket服务异常，ExceptionMsg({exception.GetType().Name}:{exception.Message})，ExceptionStackTrace({exception.StackTrace})");
                }).AddOnServiceStarted((serverSocket) =>
                {
                    Console.WriteLine("Socket服务启动");
                }).AddOnServiceStoped((serverSocket) =>
                {
                    Console.WriteLine("Socket服务关闭");
                });
            });
            IFastSocket fastSocket = fastSocketBuild.Build();
            fastSocket.Run();                                                   //Socket服务启动
            //fastSocket.Stop();                                               //Socket服务关闭
            //fastSocket.CloseOneConnection(new FastSocketConnection());        //关闭某个客户端连接
            //fastSocket.CloseOneConnectionByConnectionID((int)connectionID);   //根据连接ID关闭某个客户端连接
        }
    }
}
