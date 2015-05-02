using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureWatcher.WebApi.Models
{
    public class ExecutableControl
    {
        public bool SendOnFlags { get; set; }
        public int MinutesToKeepRunning { get; set; }
    }
}
