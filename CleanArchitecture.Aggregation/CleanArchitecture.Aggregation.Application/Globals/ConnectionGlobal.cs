using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Application.Globals
{
    public static class ConnectionGlobal
    {
        public static bool IsRabbitMqConnection { get; set; } = true;
    }
}
