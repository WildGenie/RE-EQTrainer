using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Memory;

namespace AutoBot
{
    public partial class Startup : Form
    {
        public Startup()
        {
            InitializeComponent();
        }

        public Form1 RefToForm1 { get; set; }

        private void Startup_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "auto"))
                od1.InitialDirectory = Application.StartupPath + Path.DirectorySeparatorChar + "auto";

            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName == "eqgame")
                {
                    EQGameIDs.Items.Add(theprocess.Id.ToString());
                    if (theprocess.Responding == false)
                        return;
                }
            }

            try
            {
                List<string> list1 = new List<string>() { " " };
                List<string> dirs = new List<string>(Directory.GetDirectories(Application.StartupPath + @"\builds").Select(d => new DirectoryInfo(d).Name));
                list1.AddRange(dirs);
                comboBox1.DataSource = list1;
            }
            catch
            {
                MessageBox.Show("ERROR: Cant find builds ini data or processes!");
            }
        }

        public Mem MemLib = new Mem();

        private void changeProcess()
        {
            MemLib.closeProcess();
            MemLib.OpenGameProcess(Int32.Parse(EQGameIDs.Text));
        }

        private void EQGameIDs_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeProcess();

            if (!File.Exists(Application.StartupPath + @"\builds.ini"))
                return;

            string[] subdirectoryEntries = Directory.GetDirectories(Application.StartupPath + @"\builds");
            foreach (string subdirectory in subdirectoryEntries)
            {
                string buildDateCode = MemLib.readString(Path.GetFileName(subdirectory) + "_code", Application.StartupPath + @"\builds.ini");
                string buildDate = MemLib.readString(Path.GetFileName(subdirectory) + "_date", Application.StartupPath + @"\builds.ini");
                if (buildDateCode.Contains(buildDate))
                    comboBox1.Text = Path.GetFileName(subdirectory);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            od1.ShowDialog();
        }

        public void od1_FileOk(object sender, CancelEventArgs e)
        {
            scriptBox.Text = od1.FileName;
        }

        private void inject(string dll)
        {
            try
            {
                MemLib.InjectDLL(dll);
            }
            catch
            {
                //MessageBox.Show("Injection failed! Program needs administration privileges!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loop = "dont";
            if (loopScript.Checked == true)
                loop = "loop";

            inject(Application.StartupPath + Path.DirectorySeparatorChar + "builds" + Path.DirectorySeparatorChar + comboBox1.Text + Path.DirectorySeparatorChar + "inject.dll");

            this.RefToForm1.startAutoBot(Int32.Parse(EQGameIDs.Text), Application.StartupPath + @"\builds\" + comboBox1.Text + @"\codes.ini", loop, od1.FileName);
            this.Close();
        }
    }
}
