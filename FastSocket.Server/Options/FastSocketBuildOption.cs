namespace FastSocket.Server.Options
{
    public class FastSocketBuildOption
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public int MaxConnections { get; set; }
        public int MaxTimeOutMillisecond { get; set; }
        public int MaxTransPortBodyMB { get; set; }
    }
}
