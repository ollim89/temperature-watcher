using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TemperatureWatcher.Workers.ExternalPath
{
    public class FileWorker : ExternalPathWorker
    {
        public FileWorker(string Path, string ContentMask, int hours, int minutes, int seconds)
            : base(Path, ContentMask, hours, minutes, seconds)
        {

        }

        public override string RunProcessorToGetContent()
        {
            try
            {
                string fileContent = null;
                using (StreamReader sr = new StreamReader(_path))
                {
                    fileContent = sr.ReadToEnd();
                }

                return GetSearchedContent(fileContent);
            }
            catch (IOException)
            {
                throw;
            }
        }
    }
}
