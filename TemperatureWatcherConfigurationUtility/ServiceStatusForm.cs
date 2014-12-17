using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;

namespace TemperatureWatcherConfigurationUtility
{
    public partial class ServiceStatusForm : Form
    {
        public ServiceStatusForm(ServiceControllerStatus status)
        {
            InitializeComponent();

            if (status == ServiceControllerStatus.StartPending)
            {
                StatusLabel.Text += "starting";
            }
            else if (status == ServiceControllerStatus.PausePending)
            {
                StatusLabel.Text += "pausing";
            }
            else if (status == ServiceControllerStatus.StopPending)
            {
                StatusLabel.Text += "turning off";
            }
        }
    }
}
