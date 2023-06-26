using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectangles.Infrastructure
{
    internal interface IConfiguration
    {
        int Seed { get; set; }
        int RemovalCycles { get; set; }
        int TimerInterval { get; set; }

        bool InitializeVariables();
    }
}
