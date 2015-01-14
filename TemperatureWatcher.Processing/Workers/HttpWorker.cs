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
    public class HttpWorker<T> : ExternalPathWorker<T>
    {
        public HttpWorker(string Path, string ContentMask, int hours, int minutes, int seconds, Action<T, DateTime> onUpdateCallback)
            : base(Path, ContentMask, hours, minutes, seconds, onUpdateCallback)
        {
            _timer.AutoReset = false;
        }

        public override T GetContent(DateTime timeExecuted)
        {
            T content;

            try
            {
                WebClient client = new WebClient();
                content = ConvertContent(GetSearchedContent(client.DownloadString(_path)));

                CallOnUpdateCallbackIfValueChanged(content, timeExecuted);
            }
            catch (WebException)
            {
                throw;
            }

            _timer.Start();

            return content;
        }
    }
}
