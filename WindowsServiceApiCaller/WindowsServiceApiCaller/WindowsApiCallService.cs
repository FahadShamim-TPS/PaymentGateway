using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace WindowsServiceApiCaller
{
    public partial class WindowsApiCallService : ServiceBase
    {
        Timer timer = new Timer();
        DateTime scheduleDateTime;
        public WindowsApiCallService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            this.timer.Interval = 30000;
            

            WriteLogFile("Service is started");
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);

            //timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            //scheduleDateTime = DateTime.Today.AddHours(16).AddMinutes(10);
            //var scheduleInterval = scheduleDateTime.Subtract(DateTime.Now).TotalSeconds * 1000;
            //if (scheduleInterval < 0)
            //{
            //    scheduleInterval += new TimeSpan(24, 0, 0).TotalSeconds * 1000;
            //}
            //timer.Interval = scheduleInterval;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteLogFile("Service is stopped");
        }


        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (timer.Interval != 30000)
            {
                timer.Interval = 30000; //Reset the timer    
            }
            string ApiData = new WebClient().DownloadString("http://localhost:4074/api/Authorization/Reghash");
            WriteLogFile($"Web Api called : Api Data {ApiData} ");
        }

        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }

    }
}
