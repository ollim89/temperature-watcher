namespace TemperatureWatcher.Service.Settings
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.temperatureWatcherServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.temperatureWatcherServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // temperatureWatcherServiceProcessInstaller
            // 
            this.temperatureWatcherServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.temperatureWatcherServiceProcessInstaller.Password = null;
            this.temperatureWatcherServiceProcessInstaller.Username = null;
            // 
            // temperatureWatcherServiceInstaller
            // 
            this.temperatureWatcherServiceInstaller.DisplayName = "TemperatureWatcherService";
            this.temperatureWatcherServiceInstaller.ServiceName = "TemperatureWatcherService";
            this.temperatureWatcherServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.temperatureWatcherServiceProcessInstaller,
            this.temperatureWatcherServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller temperatureWatcherServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller temperatureWatcherServiceInstaller;
    }
}