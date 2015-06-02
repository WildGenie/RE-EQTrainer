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

        float pX;
        float pY;
        float pXtwo;
        float pYtwo;
        int offset;
        int offsety;
        int MouseUpx;
        int MouseUpy;
        int rely;
        int relx;
        bool mymousedown = false;
        public static float playery;
        public static float playerx;
        public static float playerh;
        public static float playerLvl;
        public static string zoneshort;
        public static string playerName;
        int r;
        int g;
        int b;
        double zoom = 2.2;
        bool track = true; //track immediately when opened
        int trackx;
        int tracky;

        Bitmap BackBuffer;
        Graphics GFX;
        Graphics FormGFX;

        public static int proccID;
        public static IntPtr pHandle;
        public static int base_address;

        public static string[] args = Environment.GetCommandLineArgs();

        public TrainerForm RefToForm1 { get; set; }

        private void mapForm_Load(object sender, EventArgs e)
        {
            BackBuffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            FormGFX = pictureBox1.CreateGraphics();
            GFX = Graphics.FromImage(BackBuffer);
            zoom = TrackBar2.Value / 1;

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerAsync();
        }

        private void DrawOnForm()
        {
            FormGFX.DrawImage(BackBuffer, 0, 0);
        }

        public void SpawnList()
        {
            int player_spawn_info = this.RefToForm1.MemLib.readUIntPtr("spawnInfoAddress", this.RefToForm1.codeFile);

            int spawn_info_address = player_spawn_info;
            int spawn_next_spawn_info = this.RefToForm1.MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", this.RefToForm1.codeFile);
            spawn_info_address = spawn_next_spawn_info;

            for (int i = 0; i < 4096; i++)
            {
                spawn_next_spawn_info = this.RefToForm1.MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", this.RefToForm1.codeFile);

                if (spawn_next_spawn_info == 0x00000000)
                    break;

                string spawn_info_name = this.RefToForm1.MemLib.readPString((UIntPtr)spawn_info_address, "spawnInfoName", this.RefToForm1.codeFile);

                float spawn_info_y = this.RefToForm1.MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoY", this.RefToForm1.codeFile);
                float spawn_info_x = this.RefToForm1.MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoX", this.RefToForm1.codeFile);
                float spawn_info_z = this.RefToForm1.MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoZ", this.RefToForm1.codeFile);
                float spawn_info_heading = this.RefToForm1.MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoHeading", this.RefToForm1.codeFile);

                int spawn_info_level = this.RefToForm1.MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoLevel", this.RefToForm1.codeFile);
                int spawn_info_class = this.RefToForm1.MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoClass", this.RefToForm1.codeFile);
                int spawn_info_type = this.RefToForm1.MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoType", this.RefToForm1.codeFile);

                spawn_info_class = (byte)spawn_info_class;
                spawn_info_level = (byte)spawn_info_level;
                spawn_info_type = (byte)spawn_info_type;

                spawn_info_name = spawn_info_name.Replace("_", " ");
                spawn_info_name = Regex.Replace(spawn_info_name, @"[\d-]", string.Empty);

                if (filterBox.TextLength > 0)
                {
                    filterBox.Text = filterBox.Text.Replace("_", " ");
                    if (spawn_info_name.ToLower().Contains(filterBox.Text.ToLower()) == false)
                        spawn_info_address = spawn_next_spawn_info;
                }
                //string[] listViewSpawnListRow = { spawn_info_name, spawn_info_address.ToString("X8"), spawn_info_x.ToString(), spawn_info_y.ToString(), spawn_info_z.ToString(), spawn_info_heading.ToString(), spawn_info_level.ToString(), charClass(spawn_info_class), spawn_info_type.ToString() };
                if (!spawn_info_type.Equals(3)) //no corpses
                    Drawpoint(spawn_info_y, spawn_info_x, spawn_info_heading, spawn_info_level, spawn_info_name); //x and y are always reversed

                spawn_info_address = spawn_next_spawn_info;
            }
        }

        private void Drawpoint(float x, float y, float h, int level = 0, string name = null)
        {
            try
            {
                double friendx = x * -1;
                double friendy = y * -1;

                friendx = friendx / zoom;
                friendy = friendy / zoom;

                if (friendy < 0)
                {
                    friendy = Math.Abs(friendy);
                    friendy = 300 - friendy;
                }
                else
                    friendy = 300 + friendy;

                if (friendx > 0)
                {
                    friendx = friendx + 300;
                }
                else
                    friendx = 300 - Math.Abs(friendx);

                Pen Outline = Pens.Black; //defaults
                Color Filler = Color.Red;

                Font drawFont = new Font("Arial", 10);
                SolidBrush otherNames = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

                //MessageBox.Show("sLvl=" + level + " pLvl=" + playerLvl);

                if (level.Equals(0))
                { //ME
                    Outline = Pens.Purple;
                    Filler = Color.Purple;

                    tracky = ((int)friendx + (int)offsety - 20);
                    trackx = ((int)friendy + (int)offset - 20);

                    GFX.DrawEllipse(Outline, (int)friendy + (int)offset - 5, (int)friendx + (int)offsety - 5, 2 * 5, 2 * 5);
                    SolidBrush fillBrush = new SolidBrush(Filler);
                    GFX.FillEllipse(fillBrush, new Rectangle((int)friendy + (int)offset - 5, (int)friendx + (int)offsety - 5, 2 * 5, 2 * 5));
                    GFX.DrawPie(Outline, new Rectangle(((int)friendy + (int)offset - 50), ((int)friendx + (int)offsety - 50), 100, 100), ((int)(h / 1.42) - 250) * -1, 60);
                    SolidBrush opaqueBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                    GFX.FillPie(opaqueBrush, new Rectangle(((int)friendy + (int)offset - 50), ((int)friendx + (int)offsety - 50), 100, 100), ((int)(h / 1.42) - 250) * -1, 60);

                    GFX.DrawString(playerName, drawFont, otherNames, new System.Drawing.Point(((int)friendy + (int)offset - 20), ((int)friendx + (int)offsety - 20)));
                }
                else
                {
                    if (level == playerLvl)
                        Filler = Color.White;
                    if (level == (playerLvl + 1) || level == (playerLvl + 2))
                        Filler = Color.Yellow;
                    if (level < playerLvl)
                        Filler = Color.DarkBlue;
                    if (level <= (playerLvl - 4))
                        Filler = Color.Green;
                    GFX.DrawEllipse(Outline, (int)friendy + (int)offset - 5, (int)friendx + (int)offsety - 5, 2 * 5, 2 * 5);
                    SolidBrush fillBrush = new SolidBrush(Filler);
                    GFX.FillEllipse(fillBrush, new Rectangle((int)friendy + (int)offset - 5, (int)friendx + (int)offsety - 5, 2 * 5, 2 * 5));
                    GFX.DrawPie(Outline, new Rectangle(((int)friendy + (int)offset - 20), ((int)friendx + (int)offsety - 20), 40, 40), ((int)(h / 1.42) - 250) * -1, 60);
                    SolidBrush opaqueBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                    GFX.FillPie(opaqueBrush, new Rectangle(((int)friendy + (int)offset - 20), ((int)friendx + (int)offsety - 20), 40, 40), ((int)(h / 1.42) - 250) * -1, 60);

                    GFX.DrawString(name, drawFont, otherNames, new System.Drawing.Point(((int)friendy + (int)offset - 20), ((int)friendx + (int)offsety - 20)));
                }
            }
            catch
            {
                //error
            }
        }

        public void drawmap()
        {
            try
            {
                GFX.FillRectangle(Brushes.Tan, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height)); //background

                string mapFile = Application.StartupPath + @"\maps\" + zoneshort + ".txt";
                if (!File.Exists(mapFile))
                    return;
                StreamReader file = new StreamReader(mapFile);
                string line;

                Drawpoint(playerx, playery, playerh); //draw me
                SpawnList(); //draw spawns

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
                                GFX.DrawLine(redBrush, new Point((int)(pX + offset), (int)(pY + offsety)), new Point((int)(pXtwo + offset), (int)(pYtwo + offsety)));
                            }
                        }
                        else if (line.Contains("P"))
                        {
                            string[] words = line.Split(',');

                            string label = words[7];

                            pY = Convert.ToSingle(words[1]);
                            pX = Convert.ToSingle(words[0].Replace("P ", ""));

                            pX = Convert.ToSingle(pX / zoom);
                            pY = Convert.ToSingle(pY / zoom);

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

                            Font drawFont = new Font("Arial", 10);
                            using (SolidBrush blueb = new SolidBrush(Color.Black))
                            {
                                if (hideLabels.Checked == false)
                                    GFX.DrawString(label, drawFont, blueb, new System.Drawing.Point((int)(pX + offset), (int)(pY + offsety)));
                            }
                        }
                    }
                    catch
                    {
                        //error
                    }
                }
                DrawOnForm();
            }
            catch
            {
                //error
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    //string curZone = MemLib.RemoveSpecialCharacters(MemLib.readUIntPtrStr("mapShortName", codeFile));

                    playery = this.RefToForm1.MemLib.readFloat("playerY", this.RefToForm1.codeFile);
                    playerx = this.RefToForm1.MemLib.readFloat("playerX", this.RefToForm1.codeFile);
                    playerh = this.RefToForm1.MemLib.readFloat("playerHeading", this.RefToForm1.codeFile);
                    playerLvl = this.RefToForm1.MemLib.readInt("playerLvl", this.RefToForm1.codeFile);
                    playerName = this.RefToForm1.MemLib.readString("playerName", this.RefToForm1.codeFile);

                    zoneshort = this.RefToForm1.MemLib.RemoveSpecialCharacters(this.RefToForm1.MemLib.readUIntPtrStr("mapShortName", this.RefToForm1.codeFile));

                    if (track)
                    {
                        offset = (int)(playery / zoom) + 70;
                        offsety = (int)(playerx / zoom) - 50;
                        zoom = 2.2;
                        relx = 0;
                        rely = 0;
                    }

                    if (/*!curZone.Equals(zoneshort) &&*/ !String.IsNullOrEmpty(zoneshort) && !mymousedown)
                        drawmap();
                }
                catch
                {
                    //error
                }
            }
        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            if (track)
                track = false;
            zoom = TrackBar2.Value / 2.8;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                relx = e.X;
                rely = e.Y;
                mymousedown = true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mymousedown == true)
            {
                if (track)
                    track = false;

                int oldoff = offset;
                int oldoffy = offsety;

                offset = offset + (e.X - relx);
                offsety = offsety + (e.Y - rely);
                MouseUpx = offset;
                MouseUpy = offsety;

                drawmap();

                offset = oldoff;
                offsety = oldoffy;
                DrawOnForm();
            }
            else
            {
                if (relx != 0)
                {
                    offset = offset + (e.X - relx);
                    offsety = offsety + (e.Y - rely);
                    MouseUpx = offset;
                    MouseUpy = offsety;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                offset = MouseUpx;
                offsety = MouseUpy;

                relx = 0;
                rely = 0;
                mymousedown = false;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            TrackBar2.Focus();
        }

        private void trackPlayer_Click(object sender, EventArgs e)
        {
            track = true;
        }
    }
}
