# FastSocket  
NuGet包下载使用地址：https://www.nuget.org/packages/FastSocket.LiuYan/  
  
# 快速开始  
服务端  
Socket服务端启动参数有两种配置方式，一是通过在项目中添加配置文件`fastsocket.json`，二是使用代码配置。  
两种配置方式如下：  
1)代码配置  
```
  IFastSocketBuild fastSocketBuild = SocketFactory.CreateSocketBuild(new FastSocketBuildOption  
  {  
      Ip = "127.0.0.1",               //ip  
      Port = 6188,                    //port  
      MaxConnections = 2,             //最大连接数  
      MaxTimeOutMillisecond = 3000,   //最大超时时间毫秒ms  
      MaxTransPortBodyMB = 2          //最大传输内容大小MB  
  });  
```
2)配置文件`fastsocket.json`  
```
  IFastSocketBuild fastSocketBuild = SocketFactory.CreateSocketBuild();  
  
  //fastsocket.json 配置
  {
    "Ip": "127.0.0.1",
    "Port": 6188,
    "MaxConnections": 2,
    "MaxTimeOutMillisecond": 3000,
    "MaxTransPortBodyMB": 2
  }
```
配置完之后，可以为Socket服务端根据实际业务添加一些事件的监听及处理，如下：  
```
 fastSocketBuild.ConfigFastSocketService(fastSocketService =>
    {
        fastSocketService
        .AddOnConnectionConnected((serverSocket, connection) => //客户端接入监听事件
        {
            Console.WriteLine($"Socket服务，ConnectionId({connection.ConnectionID})，接入链接");
        }).AddOnReceiveMsg((serverSocket, connection, receiveBytes) => //客户端发来信息监听事件
        {
            Console.WriteLine($"Socket服务，ConnectionId({connection?.ConnectionID})，从客户端接收到消息：({serverSocket.Encoding.GetString(receiveBytes)})");
            connection.Send($"ConnectionId({connection?.ConnectionID})服务端返回的数据");
        }).AddOnSendMsg((serverSocket, connection, sendBytes) => //向客户端发出信息监听事件
        {
            Console.WriteLine($"Socket服务，ConnectionId({connection?.ConnectionID})，发送消息至客户端：({serverSocket.Encoding.GetString(sendBytes)})");
        }).AddOnConnectionClosed((serverSocket, connection) => //有客户端已关闭监听事件
        {
            Console.WriteLine($"Socket服务，ConnectionId({connection?.ConnectionID})，关闭链接");
        }).AddOnServiceException((serverSocket, exception) => //Socket服务异常监听事件
        {
            Console.WriteLine($"Socket服务异常，ExceptionMsg({exception.GetType().Name}:{exception.Message})，ExceptionStackTrace({exception.StackTrace})");
        }).AddOnServiceStarted((serverSocket) => //Socket服务已启动监听事件
        {
            Console.WriteLine("Socket服务启动");
        }).AddOnServiceStoped((serverSocket) => //Socket服务已停止监听事件
        {
            Console.WriteLine("Socket服务关闭");
        });
    }
);
```
设置完配置，添加完监听事件，即可启动运行 `FastSocket`  ，默认以`6188`端口运行
```
IFastSocket fastSocket = fastSocketBuild.Build();
fastSocket.Run();
```
  
客户端  
待完善！  
  
