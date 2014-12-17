using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Timers;

namespace TemperatureWatcherConfigurationUtility
{
    public class TemperatureService : ServiceController, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BackgroundWorker StopPendingChecker { get; set; }
        private BackgroundWorker StartPendingChecker { get; set; }
        private BackgroundWorker ContinuePendingChecker { get; set; }
        private BackgroundWorker PausePendingChecker { get; set; }
        
        public TemperatureService(string serviceName) : base(serviceName)
        {
            StopPendingChecker = new BackgroundWorker();
            StopPendingChecker.WorkerReportsProgress = true;
            StopPendingChecker.WorkerSupportsCancellation = true;
            StopPendingChecker.DoWork += new DoWorkEventHandler(StopPendingWorkerJob);

            StartPendingChecker = new BackgroundWorker();
            StartPendingChecker.WorkerReportsProgress = true;
            StartPendingChecker.WorkerSupportsCancellation = true;
            StartPendingChecker.DoWork += new DoWorkEventHandler(StartPendingWorkerJob);

            PausePendingChecker = new BackgroundWorker();
            PausePendingChecker.WorkerReportsProgress = true;
            PausePendingChecker.WorkerSupportsCancellation = true;
            PausePendingChecker.DoWork += new DoWorkEventHandler(PausePendingWorkerJob);

            ContinuePendingChecker = new BackgroundWorker();
            ContinuePendingChecker.WorkerReportsProgress = true;
            ContinuePendingChecker.WorkerSupportsCancellation = true;
            ContinuePendingChecker.DoWork += new DoWorkEventHandler(ContinuePendingWorkerJob);

            StopPendingChecker.RunWorkerAsync();
            StartPendingChecker.RunWorkerAsync();
            ContinuePendingChecker.RunWorkerAsync();
            PausePendingChecker.RunWorkerAsync();
        }

        private void StopPendingWorkerJob(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.WaitForStatus(ServiceControllerStatus.StopPending);
                NotifyServiceStatusChanged();
                if (StopPendingChecker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void PausePendingWorkerJob(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.WaitForStatus(ServiceControllerStatus.PausePending);
                NotifyServiceStatusChanged();
                if (PausePendingChecker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ContinuePendingWorkerJob(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.WaitForStatus(ServiceControllerStatus.ContinuePending);
                NotifyServiceStatusChanged();
                if (ContinuePendingChecker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void StartPendingWorkerJob(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                this.WaitForStatus(ServiceControllerStatus.StartPending);
                NotifyServiceStatusChanged();
                if (StartPendingChecker.CancellationPending)
                {
                    e.Cancel = true;
                }
            }
        }

        private void NotifyServiceStatusChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Status"));
            }
        }

        ~TemperatureService()
        {
            StopPendingChecker.CancelAsync();
            StartPendingChecker.CancelAsync();
            PausePendingChecker.CancelAsync();
            ContinuePendingChecker.CancelAsync();
        }
    }
}
