using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace temperatureWatcher
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class TemperatureWatcherService : ITemperatureWatcherServiceContract
    {
        private static TemperatureWatcherService instance;
        public TemperatureWatcher TemperatureWatcher { get; set; }

        private TemperatureWatcherService() { }

        public static TemperatureWatcherService Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new TemperatureWatcherService();
                }
                return instance;
            }
        }

        public void RunExecutable(int nrOfMinutes)
        {
            EventLog.WriteEntry(TemperatureWatcher.EventLogSource, "Webservice called at " + DateTime.Now.ToString() + ", with parameter = " + nrOfMinutes.ToString(), EventLogEntryType.Information);

            if (TemperatureWatcher == null)
            {
                throw new FaultException("No reference to windows service");
            }

            if (nrOfMinutes <= 0)
            {
                throw new FaultException("Number of minutes must be more than zero");
            }

            DateTime stopTime = DateTime.Now.AddMinutes(nrOfMinutes);
            TemperatureWatcher.IsInstantStart = true;
            TemperatureWatcher.InstantStartStopTime = stopTime;

            try
            {
                TemperatureWatcher.StartExecutable(stopTime);
            }
            catch (Exception e)
            {
                throw new FaultException("Could not start process, message: " + e.ToString());
            }
        }


        public DateTime NextStartTimeWithCurrentTemperature()
        {
            return TemperatureWatcher.nextLeaveTime;
        }

        public void StopExecution()
        {
            try
            {
                TemperatureWatcher.StopExecutable();
            }
            catch (Exception e)
            {
                throw new FaultException("Could not stop process, message: " + e.ToString());
            }
        }
    }
}
