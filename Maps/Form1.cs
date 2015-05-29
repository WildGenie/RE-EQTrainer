using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Memory;
using System.IO;

//SOURCE: http://www.redguides.com/forums/showthread.php/30319-Titanium-Client-Real-Time-Maps

namespace MapsV2
{
    public partial class Form1 : Form
    {
        float pX;
        float pY;
        float pXtwo;
        float pYtwo;
        int r;
        int g;
        int b;
        double zoom = 22.2;
        int offset;
        int offsety;
        string logfile;
        int MouseUpx;
        int MouseUpy;
        string zonelong;
        int rely;
        int relx;
        bool mymousedown = false;
        float playery;
        float playerx;
        float playerh;
        string zoneshort;
        Array zones;
        string currentzone;

        Bitmap BackBuffer;
        Graphics GFX;
        Graphics FormGFX;

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
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync();
        }

        private void Drawpoint(){ 
            //friend?
            /*double friendx = TextBox3.Text * -1;
            double friendy = TextBox4.Text * -1;

            friendx = friendx / zoom;
            friendy = friendy / zoom;

            if (friendy < 0) {
                friendy = Math.Abs(friendy);
                friendy = 300 - friendy;
            } else
                friendy = 300 + friendy;

            if (friendx > 0) {
                friendx = friendx + 300;
            } else
                friendx = 300 - Math.Abs(friendx);*/

            using (SolidBrush redBrush = new SolidBrush(Color.Blue))
            {
                GFX.DrawPie(Pens.Red, new Rectangle(Convert.ToInt32(playerx + offset - 20), Convert.ToInt32(playery + offsety - 15), 40, 40), (int)((int)((playerh) * (360 / 510)) + 285) * -1, 30);
            }
        }

        public void drawmap()
        {
            //GFX.DrawImage((int)(My.Resources.ResourceManager.GetObject("bg"), Image), new Point(0, 0)); //draw stock background image

            //Drawpoint(); //draw char xyzh on map (disable for now)
            string mapFile = Application.StartupPath + @"\maps\" + zoneshort + ".txt";
            if (!File.Exists(mapFile))
                MessageBox.Show("Missing zone file " + mapFile);
            StreamReader file = new StreamReader(mapFile);
            string line;

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    // LINES
                    if (line.Contains("L"))
                    {
                        string[] words = line.Split(',');

                        r = Convert.ToInt32(words[6].Trim());
                        g = Convert.ToInt32(words[7].Trim());
                        b = Convert.ToInt32(words[8].Trim());

                        pY = Convert.ToSingle(words[1].Trim());
                        pX = Convert.ToSingle(words[0].Replace("L", "").Trim());

                        pXtwo = Convert.ToSingle(words[3].Trim());
                        pYtwo = Convert.ToSingle(words[4].Trim());

                        pXtwo = Convert.ToSingle(pXtwo / zoom);
                        pYtwo = Convert.ToSingle(pYtwo / zoom);

                        pX = Convert.ToInt32(pX / zoom);
                        pY = Convert.ToInt32(pY / zoom);

                        if (pY < 0)
                        {
                            pY = Math.Abs(pY);
                            pY = 300 - pY;
                        }
                        else
                            pY = 300 + pY;

                        if (pYtwo < 0)
                        {
                            pYtwo = Math.Abs(pYtwo);
                            pYtwo = 300 - pYtwo;
                        }
                        else
                            pYtwo = 300 + pYtwo;

                        if (pX > 0)
                            pX = pX + 300;
                        else
                            pX = 300 - Math.Abs(pX);

                        if (pXtwo > 0)
                            pXtwo = pXtwo + 300;
                        else
                            pXtwo = 300 - Math.Abs(pXtwo);

                        using (Pen redBrush = new Pen(Color.FromArgb(r, g, b)))
                        {
                            GFX.DrawLine(redBrush, new Point(Convert.ToInt32(pX + offset), Convert.ToInt32(pY + offsety)), new Point(Convert.ToInt32(pXtwo + offset), Convert.ToInt32(pYtwo + offsety))); //issue with converting to integer
                        }
                    }
                    else if (line.Contains("P"))
                    {
                        string[] words = line.Split(',');

                        string label = words[7];

                        pY = Convert.ToInt32(words[1]);
                        pX = Convert.ToInt32(words[0].Replace("P ", ""));

                        pX = Convert.ToInt32(pX / zoom);
                        pY = Convert.ToInt32(pY / zoom);

                        if (pY < 0)
                        {
                            pY = Math.Abs(pY);
                            pY = 300 - pY;
                        }
                        else
                            pY = 300 + pY;

                        if (pX > 0)
                            pX = pX + 300;
                        else
                            pX = 300 - Math.Abs(pX);

                        MessageBox.Show(pX + "," + pY + ":" + pXtwo + "," + pYtwo); //DEBUG
                        Font drawFont = new Font("Arial", 10);
                        using (SolidBrush blueb = new SolidBrush(Color.Black))
                        {
                            //if (hideLabels.Checked == false)
                            //{
                                //label filter
                                //if (label.ToUpper.Contains(txtFilter.Text.ToUpper))
                                //    GFX.DrawString(label, drawFont, blueb, new System.Drawing.Point(pX + offset, pY + offsety));
                                //} else
                                GFX.DrawString(label, drawFont, blueb, new System.Drawing.Point(Convert.ToInt32(pX + offset), Convert.ToInt32(pY + offsety))); //issue with converting to integer
                            //}
                        }
                    }
                }
                catch
                {
                    //error
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    string codeFile = Application.StartupPath + Path.DirectorySeparatorChar + @"builds" + Path.DirectorySeparatorChar + args[2] + @"\codes.ini";
                    zoneshort = MemLib.RemoveSpecialCharacters(MemLib.readUIntPtrStr("mapShortName", codeFile));

                    /*playery = MemLib.readFloat("playerY", codeFile);
                    playerx = MemLib.readFloat("playerX", codeFile);
                    playerh = MemLib.readFloat("playerH", codeFile);*/
                    drawmap();
                }
                catch
                {
                    //error
                }
            }
        }
    }
}
