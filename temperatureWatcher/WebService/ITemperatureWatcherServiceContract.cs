using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace temperatureWatcher
{
    [ServiceContract]
    interface ITemperatureWatcherServiceContract
    {
        [OperationContract]
        void RunExecutable(int nrOfMinutes);

        [OperationContract]
        DateTime NextStartTimeWithCurrentTemperature();

        [OperationContract]
        void StopExecution();
    }
}
