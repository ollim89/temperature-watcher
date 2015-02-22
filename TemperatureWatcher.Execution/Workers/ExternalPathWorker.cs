using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace TemperatureWatcher.Execution.Workers
{
    public abstract class ExternalPathWorker<T>
    {
        protected string _path;
        private TimeSpan _interval;
        private Regex _contentMask;
        protected Timer _timer;
        protected Action<T, DateTime> _onUpdateCallback;
        protected T _content;
        private bool _firstCallbackCalled;

        public ExternalPathWorker(string Path, string ContentMask, int hours, int minutes, int seconds, Action<T, DateTime> onUpdateCallback)
        {
            _path = Path;
            _contentMask = new Regex(ContentMask);
            _interval = new TimeSpan(hours, minutes, seconds);
            _onUpdateCallback = onUpdateCallback;
            _firstCallbackCalled = false;

            //Create and set timer
            _timer = new Timer(_interval.TotalMilliseconds);
            _timer.Elapsed += GetContentAndSetContentProperty;

            //Get content first so that content propery has a value
            this.GetContentAndSetContentProperty(this, null);
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

        protected void CallOnUpdateCallbackIfValueChanged(T content, DateTime contentUpdated)
        {
            if(content != null && (!_firstCallbackCalled || (_firstCallbackCalled && !_content.Equals(content)) ))
            {
                _onUpdateCallback(content, contentUpdated);
            }
        }

        protected T ConvertContent(string content)
        {
            return (T)Convert.ChangeType(content, typeof(T), CultureInfo.InvariantCulture);
        }

        private void GetContentAndSetContentProperty(object sender, ElapsedEventArgs e)
        {
            DateTime timeExecuted;
            
            if(e != null)
            {
                timeExecuted = e.SignalTime;
            }
            else
            {
                timeExecuted = DateTime.Now;
            }

            T content = this.GetContent(timeExecuted);
            _content = content;
        }

        public abstract T GetContent(DateTime timeExecuted);
    }
}
