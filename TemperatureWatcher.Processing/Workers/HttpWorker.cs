using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace TemperatureWatcher.Execution.Workers
{
    public class HttpWorker : ExternalPathWorker
    {
        public HttpWorker(string Path, string ContentMask, int hours, int minutes, int seconds)
            : base(Path, ContentMask, hours, minutes, seconds)
        {
            _timer.AutoReset = false;
        }

        public override void GetContent(object sender, ElapsedEventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                string content = client.DownloadString(_path);

                this.Content = GetSearchedContent(content);
            }
            catch (WebException)
            {
                throw;
            }

            _timer.Start();
        }
    }
}
