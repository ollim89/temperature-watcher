using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemperatureWatcher.Execution.Workers
{
    public class FileWorker : ExternalPathWorker
    {
        public FileWorker(string Path, string ContentMask, int hours, int minutes, int seconds)
            : base(Path, ContentMask, hours, minutes, seconds)
        {
            _timer.AutoReset = false;
        }

        public override void GetContent(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string fileContent = null;
                using (StreamReader sr = new StreamReader(_path))
                {
                    fileContent = sr.ReadToEnd();
                }

                this.Content = GetSearchedContent(fileContent);
            }
            catch (IOException)
            {
                throw;
            }

            _timer.Start();
        }
    }
}
