using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Grade_B_Service
{
    public partial class CopyService : ServiceBase
    {
        private int eventID;
        private string fileName;
       
        private string destinationFile;
        //private string sourcePath = @"C:\Users\willi\RootFolder\SubFolder2";
        private string sourcePath = @"C:\Users\willi\Documents";
        //private string destinationPath = @"C:\Users\willi\AnotherRootFolder";
        private string destinationPath = @"D:\CopiedDocuments";
        private string filePath = @"D:\CopiedDocuments\loggFile.txt";
        //private string filePath = @"C:\Users\willi\AnotherRootFolder\loggFile.txt";
       
        public CopyService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("SourceToCopyService"))
            {
                System.Diagnostics.EventLog.CreateEventSource("SourceToCopyService","newEventLog");
            }
            eventLog1.Source = "SourceToCopyService";
            eventLog1.Log = "newEventLog";

            //Check if the file in the folder exists, if not create a new on
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
           
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Service started");
            Timer timer = new Timer();
            //Creating timespan objects. Day take 24 hours, now take the time right now on my computer.
            //Activation is the object who takes the time when the files should be copied. 
            //trackActivation takes the day minus the time now and the adds the activation time, if trackActivation is larger than 24 as it always is, it take 24h minus on the trackActivation so it gets the right value.
            TimeSpan day = new TimeSpan(24, 0, 0);
            TimeSpan now = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
            TimeSpan activation = new TimeSpan(18,00, 0);
            TimeSpan trackActivation = ((day - now) + activation);

            if (trackActivation.TotalHours > 24)
            {
                trackActivation -= new TimeSpan(24, 0, 0);
            }

            timer.Interval = trackActivation.TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(this.onTimer);
            timer.Start();

            string TextToLogg = "Service started";
            File.AppendAllText(filePath, "\n" + TextToLogg + DateTime.Now.ToString(" dd MMM yyyy HH:mm:ss"));

         

        }

        private void onTimer(object sender, ElapsedEventArgs e)
        {
            eventLog1.WriteEntry("System system system", EventLogEntryType.Information, eventID++);
            string TimerText;
            //Creating a array of strings to get all the files in subfolder3
            string[] filesToBeCopied = Directory.GetFiles(sourcePath);

            List<string> fileNames = new List<string>();
            //Looping through the files and add each file that being found to fileNames list. 
            foreach (var file in filesToBeCopied)
            {

                fileName = Path.GetFileName(file);
                fileNames.Add(fileName);
            }
            
            //Loping through files again to check each file if they are a docx file. And then writing out a substring to the filename
            //Then copying the file to the destinationpath and writing message to text log wich is stored in the timertext variable
            for (int i = 0; i < filesToBeCopied.Length; i++)
            {
                if (filesToBeCopied[i].Contains(".docx"))
                {
                    destinationFile = Path.Combine(destinationPath, "CopiedFile" + i.ToString() + " " + fileNames[i]);
                    File.Copy(filesToBeCopied[i], destinationFile, true);

                    TimerText = fileNames[i] + " " + " copied to " + destinationPath;
                    File.AppendAllText(filePath, "\n" + TimerText + DateTime.Now.ToString(" dd MMM yyyy HH:mm:ss"));

                }

            }
           
           
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Service stopped");
            string TextToLogg = "Service stopped";
            File.AppendAllText(filePath, "\n" + TextToLogg + DateTime.Now.ToString(" dd MMM yyy HH:mm:ss"));
        }

        //Test method for debugging in visual studio.
        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
