using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemperatureWatcher.Execution.Workers
{
    public class FileWorker<T> : ExternalPathWorker<T>
    {
        public FileWorker(string Path, string ContentMask, int hours, int minutes, int seconds, Action<T, DateTime> onUpdateCallback)
            : base(Path, ContentMask, hours, minutes, seconds, onUpdateCallback)
        {
            _timer.AutoReset = false;
        }

        public override T GetContent(DateTime timeExecuted)
        {
            T content;
            try
            {
                string fileContent = null;
                using (StreamReader sr = new StreamReader(_path))
                {
                    fileContent = sr.ReadToEnd();
                }

                content = ConvertContent(GetSearchedContent(fileContent));
                CallOnUpdateCallbackIfValueChanged(content, timeExecuted);
            }
            catch (IOException)
            {
                throw;
            }

            _timer.Start();

            return content;
        }
    }
}
