using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TemperatureWatcher.Workers
{
    public class Worker
    {
        public abstract void RunWorker();
        public abstract void StopWorker();
    }
}
