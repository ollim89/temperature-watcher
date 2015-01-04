using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.ConfigurationSection.Interfaces;

namespace TemperatureWatcher.Execution.Workers.Factories
{
    class ExternalPathWorkerFactory
    {
        public static ExternalPathWorker CreateExternalPathWorker(IExternalPathElementContainer externalPathElement)
        {
            if (externalPathElement.FileSource != null)
            {
                return new FileWorker(
                    externalPathElement.FileSource.Path,
                    externalPathElement.FileSource.ContentMask,
                    externalPathElement.FileSource.ReadInterval.Hours,
                    externalPathElement.FileSource.ReadInterval.Minutes,
                    externalPathElement.FileSource.ReadInterval.Seconds);
            }
            else if (externalPathElement.HttpSource != null)
            {
                return new HttpWorker(
                    externalPathElement.HttpSource.Path,
                    externalPathElement.HttpSource.ContentMask,
                    externalPathElement.HttpSource.ReadInterval.Hours,
                    externalPathElement.HttpSource.ReadInterval.Minutes,
                    externalPathElement.HttpSource.ReadInterval.Seconds);
            }
            else
            {
                throw new ArgumentNullException("No source configured");
            }
        }
    }
}
