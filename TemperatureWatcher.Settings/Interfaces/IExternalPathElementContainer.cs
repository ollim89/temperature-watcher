using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.General;

namespace TemperatureWatcher.ConfigurationSection.Interfaces
{
    public interface IExternalPathElementContainer
    {
        ExternalPath FileSource { get; set; }
        ExternalPath HttpSource { get; set; }
    }
}
