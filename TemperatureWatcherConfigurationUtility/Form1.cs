using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using temperatureWatcher;
using Microsoft.Win32;

namespace TemperatureWatcherConfigurationUtility
{
    public partial class Form1 : Form
    {
        public TemperatureService Service;
        private Settings Settings;
        private string ServiceName = "temperatureWatcher";
        private string SettingsFilePath = "settings.xml";

        private ServiceStatusForm StatusForm;
        
        /// <summary>
        /// Constructor, tries to get service and settings
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            //Try to get service and settings
            try
            {
                //Service to create object of
                Service = new TemperatureService(ServiceName);
                Service.PropertyChanged += new PropertyChangedEventHandler(ServiceStatusChanged);

                //Registry key pointing to service
                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\" + ServiceName);
                string strRegKey = regKey.GetValue("ImagePath").ToString().TrimStart('"').TrimEnd('"');
                strRegKey = strRegKey.Substring(0, strRegKey.Length - strRegKey.Split('\\').Last().Length);

                //Update settingsfilepath with full path of service installfolder
                SettingsFilePath = strRegKey + SettingsFilePath;

                if (Service.Status == ServiceControllerStatus.Stopped)
                {
                    //The settings object
                    Settings = new Settings(SettingsFilePath);
                }
                ServiceStatusChanged(new object(), new PropertyChangedEventArgs("Status"));

                ProgramMenuStrip.Enabled = true;
            }
            //Catch IO-errors when opening settingsfile
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Settingsfile could not be found", "Error");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("En error occured while trying to read settingsfile, mayby it is in use by another process", "Error");
            }
            //Catch exceptions when trying to locate service
            catch (InvalidOperationException)
            {
                MessageBox.Show("Can't find service", "Error");
            }
            //Catch exceptions when trying to find registrykey
            catch (ArgumentNullException)
            {
                MessageBox.Show("Could'nt get registry information about service, be sure to run as administrator");
            }
        }

        public void ServiceStatusChanged(object sender, PropertyChangedEventArgs e)
        {
            //Set control enablestate depending on service runstatus
            if (Service.Status == ServiceControllerStatus.Running)
            {
                ChangeControlStatus(false);
                ServiceOffButton.Enabled = true;
                
                if (StatusForm != null && StatusForm.Visible)
                {
                    StatusForm.Close();
                }
            }
            else if (Service.Status == ServiceControllerStatus.Stopped || Service.Status == ServiceControllerStatus.Paused)
            {
                ChangeControlStatus(true);
                ServiceOffButton.Enabled = false;

                if (StatusForm != null && StatusForm.Visible)
                {
                    StatusForm.Close();
                }
            }
            else
            {
                //StatusForm = new ServiceStatusForm(Service.Status);
                //StatusForm.FormClosed += new FormClosedEventHandler(ServiceStatusFormClosed);
                //StatusForm.ShowDialog();
            }
        }

        public void ChangeControlStatus(bool enabled)
        {
            foreach (Control c in this.Controls)
            {
                if (enabled)
                {
                    c.Enabled = true;
                }
                else
                {
                    c.Enabled = false;
                }
            }
        }

        private void ServiceOnButton_Click(object sender, EventArgs e)
        {
            try
            {
                Service.Start();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("An error occured when trying to start service, be sure to run as administrator", "Error");
            }
        }

        private void ServiceStatusFormClosed(object sender, FormClosedEventArgs e)
        {
            StatusForm.Dispose();
        }

        private void ServiceOffButton_Click(object sender, EventArgs e)
        {
            try
            {
                Service.Stop();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("An error occured when trying to stop the service, be sure to run as administrator", "Error");
            }
        }
    }
}
