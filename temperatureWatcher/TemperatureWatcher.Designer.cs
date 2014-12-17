namespace TemperatureWatcher.Service
{
    partial class TemperatureWatcher
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
            this.TimePollTimer = new System.Timers.Timer();
            this.ExecutionTimer = new System.Timers.Timer();
            this.TemperaturePollTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.TimePollTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExecutionTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemperaturePollTimer)).BeginInit();
            // 
            // TimePollTimer
            // 
            this.TimePollTimer.Interval = 300000D;
            this.TimePollTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.TimePollTimer_Elapsed);
            // 
            // ExecutionTimer
            // 
            this.ExecutionTimer.Interval = 10000D;
            this.ExecutionTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.ExecutionTimer_Elapsed);
            // 
            // TemperaturePollTimer
            // 
            this.TemperaturePollTimer.Interval = 300000D;
            this.TemperaturePollTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.TemperaturePollTimer_Elapsed);
            // 
            // Service1
            // 
            this.ServiceName = "Temperature Watcher";
            ((System.ComponentModel.ISupportInitialize)(this.TimePollTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExecutionTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TemperaturePollTimer)).EndInit();

        }

        #endregion

        private System.Timers.Timer TimePollTimer;
        private System.Timers.Timer ExecutionTimer;
        private System.Timers.Timer TemperaturePollTimer;
    }
}
