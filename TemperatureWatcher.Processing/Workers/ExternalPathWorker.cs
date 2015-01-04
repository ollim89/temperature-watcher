using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace TemperatureWatcher.Execution.Workers
{
    public abstract class ExternalPathWorker
    {
        protected string _path;
        private TimeSpan _interval;
        private Regex _contentMask;
        protected Timer _timer;

        public string Content { get; set; }

        public ExternalPathWorker(string Path, string ContentMask, int hours, int minutes, int seconds)
        {
            _path = Path;
            _contentMask = new Regex(ContentMask);
            _interval = new TimeSpan(hours, minutes, seconds);

            //Get content first so that content propery has a value
            this.GetContent(this, null);

            //Create and set timer
            _timer = new Timer(_interval.TotalMilliseconds);
            _timer.Elapsed += GetContent;
        }

        public void StartWorker()
        {
            _timer.Start();
        }

        public void StopWorker()
        {
            _timer.Stop();
        }

        protected string GetSearchedContent(string fileContent)
        {
            return _contentMask.Match(fileContent).Value;
        }

        public abstract void GetContent(object sender, ElapsedEventArgs e);
    }
}
