using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TemperatureWatcher.Service.Settings
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new TemperatureWatcherService() 
			};

            //Show forms button to close service if run in interactive mode (for debugging)
            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
            }
            //Run as windows service
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }

        static void RunInteractive(ServiceBase[] servicesToRun)
        {
            //Start service
            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
            BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }

            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Width = 220;
            form.Height = 140;

            System.Windows.Forms.Button stopButton = new System.Windows.Forms.Button();
            stopButton.Width = 180;
            stopButton.Height = 80;
            stopButton.Text = "Stoppa tjänst";
            stopButton.Top = 10;
            stopButton.Left = 10;
            stopButton.Click += delegate
            {
                //Stop service and exit application
                MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
                BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (ServiceBase service in servicesToRun)
                {
                    onStopMethod.Invoke(service, null);
                }
                Application.Exit();
            };

            form.Controls.Add(stopButton);
            Application.Run(form);
        }
    }
}
