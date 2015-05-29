using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Memory;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static int proccID;
        public static IntPtr pHandle;
        public static int base_address;
        Mem MemLib = new Mem();

        string[] args = Environment.GetCommandLineArgs();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MemLib.OpenGameProcess(args[1]);
            backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.RunWorkerAsync();
        }

        public string RemoveSpecialCharactersTwo(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //string old_map_name = "";

            while (true)
            {
                try {
                    string codeFile = Application.StartupPath + Path.DirectorySeparatorChar + @"builds" + Path.DirectorySeparatorChar + args[2] + @"\codes.ini";
                    string map_name = MemLib.readUIntPtrStr("mapShortName", codeFile);
                    string map_longname = MemLib.readUIntPtrStr("mapLongName", codeFile);
                    map_name = MemLib.RemoveSpecialCharacters(map_name);
                    map_longname = RemoveSpecialCharactersTwo(map_longname);

                    string filesPath = Application.StartupPath + Path.DirectorySeparatorChar + @"maps" + Path.DirectorySeparatorChar;
                    string locfile = filesPath + map_name + ".loc";
                    string imgfile = filesPath + map_name + ".jpg";
                    string txtfile = filesPath + map_name + ".txt";

                    FileInfo fi = new FileInfo(locfile);
                    display2.Location = new Point(0, 0);

                    float y_address = MemLib.readFloat("playerY", codeFile);
                    float x_address = MemLib.readFloat("playerX", codeFile);
                    float z_address = MemLib.readFloat("playerZ", codeFile);

                    //MessageBox.Show("Y:" + y_address.ToString() + " X:" + x_address.ToString() + " Z:" + z_address.ToString() + " map: " + map_name);// DEBUG

                    Bitmap bmp = new Bitmap(imgfile);
                    Graphics g = Graphics.FromImage(bmp);
                    float scale_x = bmp.Width;
                    float scale_y = bmp.Height;
                    g.RotateTransform(180.0F);
                    int x = 0;
                    int y = 0;
                    int actualx = bmp.Width + 346;
                    int actualy = bmp.Height + 38;
                    if (fi.Exists)
                    {
                        int[] s = new int[4];
                        using (StreamReader sr = fi.OpenText())
                        {
                            int i;
                            for (i = 0; i < 4; i++)
                            {
                                s[i] = Convert.ToInt32(sr.ReadLine());
                            }
                        }

                        scale_x = s[2] / bmp.Width;
                        scale_y = s[3] / bmp.Height;
                        x = s[0]; //offsets
                        y = s[1];

                        g.DrawString("X", new Font("Calibri", 16, FontStyle.Bold), new SolidBrush(Color.Red), (y_address / scale_y) + y, (x_address / scale_x) + x);
                    }
                    
                    
                    //if (map_name.Equals(old_map_name) == false) //cant keep pulling images this fast. Causes a crash.
                    //{
                        display2.Image = bmp;
                        this.Width = actualx;
                        this.Height = actualy;
                        display2.Width = bmp.Width;
                        display2.Height = bmp.Height;
                        this.Controls.Add(display2);

                        textBox1.Location = new Point(bmp.Width, 0);
                        textBox1.Height = bmp.Height;
                        textBox1.Width = this.Width - bmp.Width;

                        //this.Text = map_longname + " (" + map_name + ") - EQTrainer Map System";

                        fi = new FileInfo(txtfile);
                        if (fi.Exists)
                        {
                            string text = File.ReadAllText(txtfile);
                            textBox1.Text = text;
                        }
                        //old_map_name = map_name;
                    //}

                    Thread.Sleep(200);
                } catch 
                {
                    //MessageBox.Show("BackgroundWorker2 Failed");
                }
            }
        }

        private void formClosed(object sender, FormClosedEventArgs e)
        {
            MemLib.closeProcess();
        }
    }
}
