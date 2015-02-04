using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperatureWatcher.Configuration.Interfaces;

namespace TemperatureWatcher.Execution.Workers.Factories
{
    class ExternalPathWorkerFactory
    {
        public static ExternalPathWorker<T> CreateExternalPathWorker<T>(IExternalPathElementContainer externalPathElement, Action<T, DateTime> onUpdateCallback)
        {
            if (externalPathElement.FileSource != null)
            {
                return new FileWorker<T>(
                    externalPathElement.FileSource.Path,
                    externalPathElement.FileSource.ContentMask,
                    externalPathElement.FileSource.ReadInterval.Hours,
                    externalPathElement.FileSource.ReadInterval.Minutes,
                    externalPathElement.FileSource.ReadInterval.Seconds,
                    onUpdateCallback);
            }
            else if (externalPathElement.HttpSource != null)
            {
                return new HttpWorker<T>(
                    externalPathElement.HttpSource.Path,
                    externalPathElement.HttpSource.ContentMask,
                    externalPathElement.HttpSource.ReadInterval.Hours,
                    externalPathElement.HttpSource.ReadInterval.Minutes,
                    externalPathElement.HttpSource.ReadInterval.Seconds,
                    onUpdateCallback);
            }
            else
            {
                throw new ArgumentNullException("No source configured");
            }
        }
    }
}
