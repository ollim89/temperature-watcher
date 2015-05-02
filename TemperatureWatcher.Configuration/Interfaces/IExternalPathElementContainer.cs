using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Configuration.General;

namespace TemperatureWatcher.Configuration.Interfaces
{
    public interface IExternalPathElementContainer
    {
        ExternalPath FileSource { get; set; }
        ExternalPath HttpSource { get; set; }
    }
}
