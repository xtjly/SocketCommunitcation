using System;
using System.Text.RegularExpressions;

namespace FastSocket.Options
{
    public static class FastSocketBuildOptionExtention
    {
        public static (bool, Exception) IsConfigSuccess(this FastSocketBuildOption option)
        {
            if (option == null
                   || string.IsNullOrWhiteSpace(option.Ip)
                   || option.Port <= 0
                   || option.MaxConnections <= 0
                   || option.MaxTimeOutMillisecond <= 0
                   || !Regex.IsMatch(option.Ip, @"(^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$)|(^localhost$)")
                   || option.MaxTransPortBodyMB <= 0)
            {
                return (false, new Exception("fastsocket.json文件配置错误，或FastSocketBuildOption配置错误"));
            }
            return (true, null);
        }
    }
}
