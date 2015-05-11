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
            backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            MemLib.OpenProcess(args[1]);
            string old_map_name = "";

            while (true)
            {
                try {
                    string codeFile = Application.StartupPath + Path.DirectorySeparatorChar + @"builds" + Path.DirectorySeparatorChar + args[2] + @"\codes.ini";
                    string map_name = MemLib.readUIntPtrStr("mapShortName", codeFile);
                    map_name = MemLib.RemoveSpecialCharacters(map_name);

                    if (map_name == "" || string.IsNullOrEmpty(map_name))
                        continue;

                    float y_address = MemLib.readFloat("playerY", codeFile);
                    float x_address = MemLib.readFloat("playerX", codeFile);
                    float z_address = MemLib.readFloat("playerZ", codeFile);

                    string filesPath = Application.StartupPath + Path.DirectorySeparatorChar + @"maps" + Path.DirectorySeparatorChar;
                    string locfile = filesPath + map_name + ".loc";
                    string imgfile = filesPath + map_name + ".jpg";
                    string txtfile = filesPath + map_name + ".txt";
                    //MessageBox.Show("Y:" + y_address.ToString() + " X:" + x_address.ToString() + " Z:" + z_address.ToString() + " map: " + map_name);// DEBUG

                    if (File.Exists(txtfile))
                    {
                        string text = File.ReadAllText(txtfile);
                        textBox1.Text = text;
                    }
                    else
                        textBox1.Text = "No data file for this map.";

                    if (File.Exists(imgfile))
                    {
                        Bitmap bmp = new Bitmap(imgfile);
                        Graphics g = Graphics.FromImage(bmp);
                        float scale_x = bmp.Width;
                        float scale_y = bmp.Height;
                        g.RotateTransform(180.0F);
                        int x = 0;
                        int y = 0;
                        int actualx = bmp.Width + 346;
                        int actualy = bmp.Height + 38;

                        if (File.Exists(locfile))
                        {
                            FileInfo fi = new FileInfo(locfile);
                            display2.Location = new Point(0, 0);
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
                            }

                            g.DrawString("X", new Font("Calibri", 12), new SolidBrush(Color.Red), (y_address / scale_y) + y, (x_address / scale_x) + x);
                        }

                        if (map_name != old_map_name) //cant keep pulling images this fast. Causes a crash.
                        {
                            display2.Image = bmp;
                            this.Width = actualx;
                            this.Height = actualy;
                            display2.Width = bmp.Width;
                            display2.Height = bmp.Height;
                            this.Controls.Add(display2);

                            textBox1.Location = new Point(bmp.Width, 0);
                            textBox1.Height = bmp.Height;
                        }
                        else
                            old_map_name = map_name;
                    }
                    else
                    {
                        Bitmap bmp = new Bitmap(filesPath + "NA.jpg");
                        Graphics g = Graphics.FromImage(bmp);
                        float scale_x = bmp.Width;
                        float scale_y = bmp.Height;
                        g.RotateTransform(180.0F);
                        int actualx = bmp.Width + 346;
                        int actualy = bmp.Height + 38;
                    }

                    Thread.Sleep(200);
                } catch 
                {
                    //MessageBox.Show("BackgroundWorker2 Failed");
                }
            }
        }
    }
}
