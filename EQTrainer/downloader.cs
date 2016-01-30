using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EQTrainer
{
    public partial class downloader : Form
    {
        public downloader()
        {
            InitializeComponent();
        }

        private void downloader_Load(object sender, EventArgs e)
        {

        }

        private void formShown(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
                backgroundWorker1.RunWorkerAsync();
        }

        public string whatsGoinOn = "Downloading files... ";
        bool restart = false;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var url = "http://newagesoldier.com/myfiles/eqtrainer.php";
            var client = new WebClient();
            Dictionary<string, string> updateTimeStamp = new Dictionary<string, string>();
            int total = 0;

            using (var stream = client.OpenRead(url))
            {
                using (var reader = new StreamReader(stream))
                {
                    string[] words = reader.ReadLine().Split(';');

                    foreach (string word in words)
                    {
                        total++;
                    }
                    int t = 1;

                    //step 1 : download files. This takes milliseconds
                    foreach (string word in words)
                    {
                        if (!word.Contains(','))
                            continue;
                        t++;
                        int percentage = (t * 100 / total);
                        backgroundWorker1.ReportProgress(percentage);

                        string[] s = word.Split(',');
                        Uri uri = new Uri("http://newagesoldier.com/myfiles/eqtrainer/" + s[0].Replace('\\', '/'));

                        if (!Directory.Exists(System.IO.Path.GetDirectoryName(Application.StartupPath + Path.DirectorySeparatorChar + s[0])))
                            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Application.StartupPath + Path.DirectorySeparatorChar + s[0]));

                        //this.Invoke(new Action(() => { MessageBox.Show(this, s[0] + " | " + Convert.ToDateTime(s[1]).ToString() + " | " + File.GetLastWriteTime(Application.StartupPath + Path.DirectorySeparatorChar + s[0]).ToString() + " | " + DateTime.Compare(File.GetLastWriteTime(Application.StartupPath + Path.DirectorySeparatorChar + s[0]), Convert.ToDateTime(s[1])).ToString()); }));

                        if (!File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + s[0]) 
                            || DateTime.Compare(File.GetLastWriteTime(Application.StartupPath + Path.DirectorySeparatorChar + s[0]), Convert.ToDateTime(s[1])) < 0)
                        {
                            using (WebClient webC = new WebClient())
                            {
                                try
                                {
                                    if (s[0] != null && s[0] != "")
                                        webC.DownloadFile(uri, Application.StartupPath + Path.DirectorySeparatorChar + s[0]);
                                }
                                catch
                                {
                                    MessageBox.Show("crashed at " + '"' + s[0] + '"');
                                }
                            }
                            if (word.Contains(','))
                                updateTimeStamp.Add(s[0], s[1]);
                        }
                    }
                }
            }

            int i = 1;
            //step 2 : modify datetimes to match server
            foreach (KeyValuePair<string, string> entry in updateTimeStamp) //key = path, value = date
            {
                whatsGoinOn = "Changing file datetimes... ";
                restart = true;
                i++;
                int percentage2 = (i * 100 / total);
                backgroundWorker1.ReportProgress(percentage2);
                FileAttributes attr = File.GetAttributes(Application.StartupPath + Path.DirectorySeparatorChar + entry.Key);
                if (!attr.HasFlag(FileAttributes.Directory) && File.Exists(Application.StartupPath + Path.DirectorySeparatorChar + entry.Key))
                    File.SetLastWriteTime(Application.StartupPath + Path.DirectorySeparatorChar + entry.Key, Convert.ToDateTime(entry.Value));
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 100)
                progressBar1.Value = e.ProgressPercentage;
            statusLabel.Text = whatsGoinOn + e.ProgressPercentage + "%";
        }

        private void backgroundWorker1_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            //TrainerForm frm = new TrainerForm();
            //statusLabel.Text = "COMPLETE!";
            //frm.loadBuilds();
            if (restart) //we just updated, restart
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
                Application.Exit();
            }
            else
                Close();
        }
    }
}
