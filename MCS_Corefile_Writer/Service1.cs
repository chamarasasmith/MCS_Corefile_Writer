using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MCS_Corefile_Writer
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        string CoreLocation1 = ConfigurationManager.AppSettings["CoreLocation1"];
        string CoreLocation2 = ConfigurationManager.AppSettings["CoreLocation2"];
        string CSVLocation1 = ConfigurationManager.AppSettings["CSVLocation1"];
        string CSVLocation2 = ConfigurationManager.AppSettings["CSVLocation2"];
        string ImageLocation1 = ConfigurationManager.AppSettings["ImageLocation1"];
        string ImageLocation2 = ConfigurationManager.AppSettings["ImageLocation2"];
        string TInterval = ConfigurationManager.AppSettings["TInterval"];
        string CoreFilePattern1 = ConfigurationManager.AppSettings["CoreFilePattern1"];
        

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") +" : Service is Started" );
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = int.Parse(TInterval);  
            timer.Enabled = true;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                DirectoryInfo d1 = new DirectoryInfo(CoreLocation1);
                FileInfo[] Files1 = d1.GetFiles(CoreFilePattern1);

                DirectoryInfo d2 = new DirectoryInfo(ImageLocation1);
                FileInfo[] Files2 = d2.GetFiles("*.tif");

                DirectoryInfo d3 = new DirectoryInfo(ImageLocation1);
                FileInfo[] Files3 = d3.GetFiles("*.jpg");


                DirectoryInfo d4 = new DirectoryInfo(CSVLocation1);
                FileInfo[] Files4 = d4.GetFiles("*.CSV");

                DirectoryInfo d5 = new DirectoryInfo(CoreLocation2);
                FileInfo[] Files5 = d5.GetFiles("*.TMP");


                foreach (FileInfo file1 in Files1)
                {
                    File.Copy(file1.FullName, CoreLocation2 + "\\" + file1.Name, true);
                    File.Delete(file1.FullName);
                    WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Copied - "+file1.Name);
                }

                foreach (FileInfo file2 in Files2)
                {
                    File.Copy(file2.FullName, ImageLocation2 + "\\" + file2.Name, true);
                    File.Delete(file2.FullName);
                    WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Copied - " + file2.Name);
                }

                foreach (FileInfo file3 in Files3)
                {
                    File.Copy(file3.FullName, ImageLocation2 + "\\" + file3.Name, true);
                    File.Delete(file3.FullName);
                    WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Copied - " + file3.Name);
                }

                foreach (FileInfo file4 in Files4)
                {
                    File.Copy(file4.FullName, CSVLocation2 + "\\" + file4.Name, true);
                    File.Delete(file4.FullName);
                    WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Copied - " + file4.Name);
                }

                foreach (FileInfo file5 in Files5)
                {
                    //File.Copy(file5.FullName, CoreLocation2 + "\\" + file5.Name.Replace(".TMP",".DTL"), true);
                    //File.Delete(file5.FullName);
                    string fname5 = file5.FullName;
                    File.Move(fname5, Path.ChangeExtension(fname5, ".DTL"));
                    WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Rename - " + file5.Name);
                }

            }
            catch (Exception ex)
            {
                WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Error - " + ex);
            }
            
            

        }

        protected override void OnStop()
        {
            WriteToFile(DateTime.Now.ToString("yyyyMMddHHmmss") + " : Service is Stopped");
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog - " + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

    }
}
