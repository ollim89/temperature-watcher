using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemperatureWatcher.Workers.ExternalPath
{
    public class HttpWorker : ExternalPathWorker
    {
        public HttpWorker(string Path, string ContentMask, int hours, int minutes, int seconds)
            : base(Path, ContentMask, hours, minutes, seconds)
        {

        }

        private string GetContent()
        {
            try
            {
                WebClient client = new WebClient();
                string content = client.DownloadString(_path);

                return GetSearchedContent(content);
            }
            catch (WebException)
            {
                throw;
            }
        }

        public override void RunWorker()
        {
            throw new NotImplementedException();
        }

        public override void StopWorker()
        {
            throw new NotImplementedException();
        }
    }
}
