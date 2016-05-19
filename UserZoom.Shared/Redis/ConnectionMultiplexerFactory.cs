using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserZoom.Shared.Redis
{
    public static class ConnectionMultiplexerFactory
    {
        static ConnectionMultiplexerFactory()
        {
            ConfigurationOptions options = new ConfigurationOptions();
            options.EndPoints.Add("locahost:6379");

            Current = ConnectionMultiplexer.Connect(options);
        }

        public static ConnectionMultiplexer Current { get; }
    }
}
