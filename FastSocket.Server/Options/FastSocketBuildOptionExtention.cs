using System;
using System.Collections.Generic;
using System.Text;

namespace FastSocket.Server.Options
{
    public static class FastSocketBuildOptionExtention
    {
        public (bool, Exception) IsOptionOk(this FastSocketBuildOption option)
        {
            if (option == null
                   || string.IsNullOrWhiteSpace(option.Ip)
                   || option.Port <= 0
                   || option.MaxConnections <= 0)
            {

            }

        }
    }
}
