using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemperatureWatcher.Workers.ExternalPath
{
    public abstract class ExternalPathWorker : Worker
    {
        protected string _path;
        private TimeSpan _interval;
        private Regex _contentMask;

        public ExternalPathWorker(string Path, string ContentMask, int hours, int minutes, int seconds)
        {
            _path = Path;
            _contentMask = new Regex(ContentMask);
            _interval = new TimeSpan(hours, minutes, seconds);
        }

        public string GetSearchedContent(string fileContent)
        {
            return _contentMask.Match(fileContent).Value;
        }
    }
}
