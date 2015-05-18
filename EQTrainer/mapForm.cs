using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace EQTrainer
{
    public partial class mapForm : Form
    {
        public mapForm()
        {
            InitializeComponent();
        }

        public TrainerForm RefToForm1 { get; set; }

        private void mapForm_Load(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //string old_map_name = "";

            while (true)
            {
                try
                {
                    string map_name = Regex.Match(this.RefToForm1.map_label.Text, @"\(([^)]*)\)").Groups[1].Value;
                    
                    if (map_name == "" || string.IsNullOrEmpty(map_name))
                        continue;

                    float y_address = Convert.ToSingle(this.RefToForm1.y_label.Text);
                    float x_address = Convert.ToSingle(this.RefToForm1.x_label.Text);
                    float z_address = Convert.ToSingle(this.RefToForm1.z_label.Text);

                    string filesPath = Application.StartupPath + Path.DirectorySeparatorChar + @"maps" + Path.DirectorySeparatorChar;
                    string locfile = filesPath + map_name + ".loc";
                    string imgfile = filesPath + map_name + ".jpg";
                    string txtfile = filesPath + map_name + ".txt";

                    FileInfo fi = new FileInfo(locfile);
                    display2.Location = new Point(0, 0);

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

                        fi = new FileInfo(txtfile);
                        if (fi.Exists)
                        {
                            string text = File.ReadAllText(txtfile);
                            textBox1.Text = text;
                        }
                       //old_map_name = map_name;
                    //}
                }
                catch
                {
                    //MessageBox.Show("BackgroundWorker2 Failed");
                }
            }
        }
    }
}
