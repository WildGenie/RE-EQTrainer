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
using System.Security.Principal;
using Microsoft.Win32;

namespace EQTrainer
{
    public partial class TrainerForm : Form
    {
        public TrainerForm()
        {
            InitializeComponent();
        }

        #region DllImports
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool FreeLibrary([In] IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(
            IntPtr hModule,
            string procName
            );

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
  
        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]  
        internal static extern Int32 WaitForSingleObject(  
            IntPtr handle,  
            Int32 milliseconds  
            );

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);
        #endregion

        #region PublicVariables
        public static int proccID;
        //public static IntPtr pHandle;
        public Mem MemLib = new Mem();

        public miniToolbar RefToMiniForm { get; set; }

        private static Boolean Follow = false;
        protected ProcessModule myProcessModule;
        public static string eqgameID = "";
        byte[] memory = new byte[4];
        byte[] memoryBig = new byte[64];
        byte[] memoryGiant = new byte[255];
        public string codeFile = "";
        public int buffRefresh = 0;

        protected override void WndProc(ref Message m) //hotbuttons
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                if (id == 1)
                    teleportBtn1.PerformClick();
                else if (id == 2)
                    teleportBtn2.PerformClick();
                else if (id == 3)
                    teleportBtn3.PerformClick();
                else if (id == 4)
                    teleportBtn4.PerformClick();
                else if (id == 5) {
                    mapForm fc = Application.OpenForms["mapForm"] != null ? (mapForm)Application.OpenForms["mapForm"] : null;
                    if (fc != null)
                    {
                        if (fc.WindowState == FormWindowState.Minimized)
                            fc.WindowState = FormWindowState.Normal;
                        else
                            fc.Close();
                    }
                    else
                        openMapSystemToolStripMenuItem.PerformClick();
                } else if (id == 6)
                    gateBtn.PerformClick();
                else if (id == 7)
                    followBtn.PerformClick();
            }
            base.WndProc(ref m);
        }
        #endregion

        private void TrainerForm_Load(object sender, EventArgs e)
        {
            try
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(this.teleportBtn1, "Teleport to Coordinates 1 (CTRL+1)");
                tt.SetToolTip(this.teleportBtn2, "Teleport to Coordinates 2 (CTRL+2)");
                tt.SetToolTip(this.teleportBtn3, "Teleport to Coordinates 3 (CTRL+3)");
                tt.SetToolTip(this.teleportBtn4, "Teleport to Coordinates 4 (CTRL+4)");
                tt.SetToolTip(this.followBtn, "Warp Follow Target (CTRL+T)");
                tt.SetToolTip(this.gateBtn, "Instant Gate (CTRL+G)");
                tt.SetToolTip(this.x_label, "forward and backwards");
                tt.SetToolTip(this.y_label, "left and right");
                tt.SetToolTip(this.z_label, "up and down");
                tt.SetToolTip(this.map_label, "Open Map System (CTRL+M)");
                tt.SetToolTip(this.button5, "Set current X Y Z (set 1)");
                tt.SetToolTip(this.button6, "Set current X Y Z (set 2)");
                tt.SetToolTip(this.button7, "Set current X Y Z (set 3)");
                tt.SetToolTip(this.button8, "Set current X Y Z (set 4)");
                tt.SetToolTip(this.button9, "Erase X Y Z (set 1)");
                tt.SetToolTip(this.button10, "Erase X Y Z (set 2)");
                tt.SetToolTip(this.button11, "Erase X Y Z (set 3)");
                tt.SetToolTip(this.button12, "Erase X Y Z (set 4)");
                tt.SetToolTip(this.button13, "Save X Y Z to file (set 1)");
                tt.SetToolTip(this.button14, "Save X Y Z to file (set 2)");
                tt.SetToolTip(this.button15, "Save X Y Z to file (set 3)");
                tt.SetToolTip(this.button16, "Save X Y Z to file (set 4)");
                tt.SetToolTip(this.button17, "Load X Y Z from file (set 1)");
                tt.SetToolTip(this.button18, "Load X Y Z from file (set 2)");
                tt.SetToolTip(this.button19, "Load X Y Z from file (set 3)");
                tt.SetToolTip(this.button20, "Load X Y Z from file (set 4)");
                if (Directory.Exists(Application.StartupPath + Path.DirectorySeparatorChar + "auto"))
                    autoLoad.InitialDirectory = Application.StartupPath + Path.DirectorySeparatorChar + "auto";
                if (Properties.Settings.Default.old_warp == true)
                {
                    tt.SetToolTip(this.button1, "Old warp enabled. Inject happens when you teleport.");
                    button1.Enabled = false;
                } else
                    tt.SetToolTip(this.button1, "Inject DLL to use teleporting.");
            }
            catch
            {
                MessageBox.Show("ERROR: Cant set tool tips!");
            }

            try
            {
                sd1.InitialDirectory = Path.Combine(Application.StartupPath, @"saves");
                sd2.InitialDirectory = Path.Combine(Application.StartupPath, @"saves");
                sd3.InitialDirectory = Path.Combine(Application.StartupPath, @"saves");
                sd4.InitialDirectory = Path.Combine(Application.StartupPath, @"saves");
                RegisterHotKey(this.Handle, 1, 2, (int)'1');
                RegisterHotKey(this.Handle, 2, 2, (int)'2');
                RegisterHotKey(this.Handle, 3, 2, (int)'3');
                RegisterHotKey(this.Handle, 4, 2, (int)'4');
                RegisterHotKey(this.Handle, 5, 2, (int)'M'); //must be uppercase
                RegisterHotKey(this.Handle, 6, 2, (int)'G');
                RegisterHotKey(this.Handle, 7, 2, (int)'T');
            }
            catch
            {
                MessageBox.Show("ERROR: Cant set hot keys!");
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

        private string charClass(int t_class)
        {
            string[] classes = new string[] { "", "Warrior", "Cleric", "Paladin", "Ranger", "Shadow Knight", "Druid", "Monk", "Bard", "Rogue", "Shaman", "Necromancer", "Wizard", "Magician", "Enchanter", "Beastlord", "Banker", "Warrior Trainer", "Cleric Trainer", "Paladin Trainer", "Ranger Trainer", "Shadow Knight Trainer", "Druid Trainer", "Monk Trainer", "Bard Trainer", "Rogue Trainer", "Shaman Trainer", "Necromancer Trainer", "Wizard Trainer", "Magician Trainer", "Enchanter Trainer", "Beastlord Trainer", "Merchant" };
            if (t_class > 0 && t_class < 33)
                return classes[t_class];
            else
                return "Unknown";
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

        private void stopFollow()
        {
            followBtn.Text = "Follow Target";
            Follow = false;
            return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name.Equals("miniToolbar"))
                    this.WindowState = FormWindowState.Minimized;
            }
            if (comboBox1.Text.Equals("EQMac"))
                gateBtn.Enabled = true;
            else
                gateBtn.Enabled = false;
            codeFile = Application.StartupPath + @"\builds\" + comboBox1.Text + @"\codes.ini";
            if (listView2.Items.Count == 0)
            {
                Process[] processlist = Process.GetProcesses();

                foreach (Process theprocess in processlist)
                {
                    if (theprocess.ProcessName == "eqgame")
                    {
                        listView2.Items.Add(theprocess.Id.ToString());
                        if (theprocess.Responding == false)
                            return;
                    }
                }
                if (listView2.Items.Count > 0) //if we didnt have items before, but now we do now
                {
                    if (listView2.SelectedItems.Count == 0) //select the first item we see and start reading
                    {
                        listView2.Items[0].Selected = true;
                        listView2.Select();
                        eqgameID = listView2.SelectedItems[0].SubItems[0].Text;
                        changeProcess();
                        if (Properties.Settings.Default.old_warp == false)
                            inject(Application.StartupPath + Path.DirectorySeparatorChar + "builds" + Path.DirectorySeparatorChar + comboBox1.Text + Path.DirectorySeparatorChar + "inject.dll");
                        backgroundWorker1.RunWorkerAsync();
                    }
                }
            }

            if (listView2.SelectedItems.Count > 0)
            {
                if (eqgameID != listView2.SelectedItems[0].SubItems[0].Text)
                {
                    eqgameID = listView2.SelectedItems[0].SubItems[0].Text; //keep maps up to date
                    changeProcess();
                }
                if (backgroundWorker1.IsBusy == false)
                    backgroundWorker1.RunWorkerAsync();
            }

            if (Follow == true)
            {
                try
                {
                    float t_z_address = MemLib.readFloat("targetZ", codeFile);
                    float t_y_address = MemLib.readFloat("targetY", codeFile);
                    float t_x_address = MemLib.readFloat("targetX", codeFile);
                    float t_h_address = MemLib.readFloat("targetHeading", codeFile);

                    double angleInDegrees = t_h_address / 1.42;
                    double cos = Math.Cos(angleInDegrees * (Math.PI / 180.0));
                    double sin = Math.Sin(angleInDegrees * (Math.PI / 180.0));
                    double reverse_x = t_x_address - Convert.ToInt32(distance.Text) * cos;
                    double reverse_y = t_y_address - Convert.ToInt32(distance.Text) * sin;

                    int tHealth = MemLib.readByte("targetHealth", codeFile);
                    tHealth = (byte)tHealth;

                    if (tHealth == 0)
                    { //targets dead
                        stopFollow();
                        return;
                    }

                    MemLib.writeMemory("playerZ", codeFile, "float", t_z_address.ToString());
                    MemLib.writeMemory("playerY", codeFile, "float", reverse_y.ToString());
                    MemLib.writeMemory("playerX", codeFile, "float", reverse_x.ToString());
                    MemLib.writeMemory("playerHeading", codeFile, "float", t_h_address.ToString());

                    followBtn.Text = "Unfollow";
                }
                catch
                {
                    stopFollow();
                }
            } 
            else
                stopFollow();
        }

        private void TrainerForm_LoadScripts()
        {
            Debug.WriteLine("Application.StartupPath: " + Application.StartupPath);
            try
            {
                listViewScripts.Items.Clear();
                string[] scripts = Directory.GetFiles(Application.StartupPath + @"\builds\" + comboBox1.Text + @"\scripts", "*.ini");

                foreach (string script in scripts)
                {
                    Debug.WriteLine("loaded script: " + script);
                    uint read_ini_result = 0;
                    StringBuilder script_name = new StringBuilder(1024);
                    read_ini_result = GetPrivateProfileString("Script", "Name", "", script_name, (uint)script_name.Capacity, script);
                    StringBuilder script_description = new StringBuilder(1024);
                    read_ini_result = GetPrivateProfileString("Script", "Description", "", script_description, (uint)script_description.Capacity, script);
                    System.IO.StreamReader script_file = new System.IO.StreamReader(script);
                    bool script_found_enable = false;
                    bool script_found_disable = false;
                    string script_instruction_enable = "";
                    string script_instruction_disable = "";
                    string script_line;
                    while ((script_line = script_file.ReadLine()) != null)
                    {
                        if (script_line.Length == 0)
                            continue;

                        if (script_line.Contains("#"))
                            continue;

                        if (script_line.Contains("//"))
                            continue;

                        if (script_line.Contains("[Enable]"))
                        {
                            script_found_enable = true;
                            script_found_disable = false;
                            continue;
                        }

                        if (script_line.Contains("[Disable]"))
                        {
                            script_found_enable = false;
                            script_found_disable = true;
                            continue;
                        }

                        if (script_found_enable == true)
                            script_instruction_enable += script_line + "^";

                        if (script_found_disable == true)
                            script_instruction_disable += script_line + "^";
                    }

                    script_file.Close();

                    script_instruction_enable = script_instruction_enable.TrimEnd('^', ' ');
                    script_instruction_disable = script_instruction_disable.TrimEnd('^', ' ');

                    string[] listviewScriptsRow = { script_name.ToString(), script_description.ToString(), script_instruction_enable, script_instruction_disable };
                    var listviewScriptsItem = new ListViewItem(listviewScriptsRow);
                    listViewScripts.Items.Add(listviewScriptsItem);
                }
            }
            catch
            {
                //MessageBox.Show("no scripts found");
            }
        }

        private void TrainerForm_RefreshSpawnList()
        {
            int player_spawn_info = MemLib.readInt("spawnInfoAddress", codeFile);

            int spawn_info_address = player_spawn_info;
            int spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);
            spawn_info_address = spawn_next_spawn_info;

            for (int i = 0; i < 4096; i++)
            {
                spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);

                if (spawn_next_spawn_info == 0x00000000)
                    break;

                string spawn_info_name = MemLib.readPString((UIntPtr)spawn_info_address, "spawnInfoName", codeFile);

                float spawn_info_y = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoY", codeFile);
                float spawn_info_x = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoX", codeFile);
                float spawn_info_z = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoZ", codeFile);
                float spawn_info_heading = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoHeading", codeFile);

                int spawn_info_level = MemLib.readPInt((UIntPtr)spawn_info_address,"spawnInfoLevel", codeFile);
                int spawn_info_class = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoClass", codeFile);
                int spawn_info_type = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoType", codeFile);

                spawn_info_class = (byte)spawn_info_class;
                spawn_info_level = (byte)spawn_info_level;
                spawn_info_type = (byte)spawn_info_type;

                if (textBoxSpawnListFilter.TextLength > 0)
                {
                    spawn_info_name = spawn_info_name.Replace("_", " ");
                    textBoxSpawnListFilter.Text = textBoxSpawnListFilter.Text.Replace("_", " ");
                    if (spawn_info_name.ToLower().Contains(textBoxSpawnListFilter.Text.ToLower()) == false)
                    {
                        spawn_info_address = spawn_next_spawn_info;
                        continue;
                    }
                }


                string[] listViewSpawnListRow = { spawn_info_name, spawn_info_address.ToString("X8"), spawn_info_x.ToString(), spawn_info_y.ToString(), spawn_info_z.ToString(), spawn_info_heading.ToString(), spawn_info_level.ToString(), charClass(spawn_info_class), spawn_info_type.ToString() };
                var listViewSpawnListItem = new ListViewItem(listViewSpawnListRow);
                listViewSpawnList.Items.Add(listViewSpawnListItem);

                spawn_info_address = spawn_next_spawn_info;
            }
        }

        private void TrainerForm_WarpToSpawn()
        {
            string x_text = listViewSpawnList.SelectedItems[0].SubItems[2].Text;
            string y_text = listViewSpawnList.SelectedItems[0].SubItems[3].Text;
            string z_text = listViewSpawnList.SelectedItems[0].SubItems[4].Text;

            string heading_text = listViewSpawnList.SelectedItems[0].SubItems[5].Text;

            Teleport(float.Parse(y_text),float.Parse(x_text),float.Parse(z_text),float.Parse(heading_text));
        }        

        private void teleportBtn1_Click(object sender, EventArgs e)
        {
            if (h_tele1.Text == "") h_tele1.Text = "0";
            if (x_tele.Text == "" || y_tele.Text == "" || z_tele.Text == "")
            {
                if (Properties.Settings.Default.no_coords == false)
                {
                    MessageBox.Show("ERROR: You need X Y and Z coordinates!");
                }
            }
            else
            {
                if (Convert.ToSingle(x_label.Text).Equals(0) && Convert.ToSingle(y_label.Text).Equals(0)) // zoning...
                    return;
                Teleport(float.Parse(x_tele.Text), float.Parse(y_tele.Text), float.Parse(z_tele.Text), float.Parse(h_tele1.Text));
            }
        }

        private void teleportBtn2_Click(object sender, EventArgs e)
        {
            if (h_tele2.Text == "") h_tele2.Text = "0";
            if (x_tele2.Text == "" || y_tele2.Text == "" || z_tele2.Text == "")
            {
                if (Properties.Settings.Default.no_coords == false)
                {
                    MessageBox.Show("ERROR: You need X Y and Z coordinates!");
                }
            }
            else
            {
                if (Convert.ToSingle(x_label.Text).Equals(0) && Convert.ToSingle(y_label.Text).Equals(0)) // zoning...
                    return;
                Teleport(float.Parse(x_tele2.Text), float.Parse(y_tele2.Text), float.Parse(z_tele2.Text), float.Parse(h_tele2.Text));
            }
        }

        private void teleportBtn3_Click(object sender, EventArgs e)
        {
            if (h_tele2.Text == "") h_tele2.Text = "0";
            if (x_tele3.Text == "" || y_tele3.Text == "" || z_tele3.Text == "")
            {
                if (Properties.Settings.Default.no_coords == false)
                {
                    MessageBox.Show("ERROR: You need X Y and Z coordinates!");
                }
            }
            else
            {
                if (Convert.ToSingle(x_label.Text).Equals(0) && Convert.ToSingle(y_label.Text).Equals(0)) // zoning...
                    return;
                Teleport(float.Parse(x_tele3.Text), float.Parse(y_tele3.Text), float.Parse(z_tele3.Text), float.Parse(h_tele3.Text));
            }
        }

        private void teleportBtn4_Click(object sender, EventArgs e)
        {
            if (h_tele4.Text == "") h_tele4.Text = "0";
            if (x_tele4.Text == "" || y_tele4.Text == "" || z_tele4.Text == "")
            {
                if (Properties.Settings.Default.no_coords == false)
                {
                    MessageBox.Show("ERROR: You need X Y and Z coordinates!");
                }
            }
            else
            {
                if (Convert.ToSingle(x_label.Text).Equals(0) && Convert.ToSingle(y_label.Text).Equals(0)) // zoning...
                    return;
                Teleport(float.Parse(x_tele4.Text), float.Parse(y_tele4.Text), float.Parse(z_tele4.Text), float.Parse(h_tele4.Text));
            }
        }

        private void map_label_Click(object sender, EventArgs e)
        {
            mapForm obj3 = new mapForm();
            obj3.RefToForm1 = this;
            obj3.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            x_tele.Text = x_label.Text;
            y_tele.Text = y_label.Text;
            z_tele.Text = z_label.Text;
            h_tele1.Text = heading_label.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            x_tele2.Text = x_label.Text;
            y_tele2.Text = y_label.Text;
            z_tele2.Text = z_label.Text;
            h_tele2.Text = heading_label.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            x_tele3.Text = x_label.Text;
            y_tele3.Text = y_label.Text;
            z_tele3.Text = z_label.Text;
            h_tele3.Text = heading_label.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            x_tele4.Text = x_label.Text;
            y_tele4.Text = y_label.Text;
            z_tele4.Text = z_label.Text;
            h_tele4.Text = heading_label.Text;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tele_label1.Clear();
            x_tele.Clear();
            y_tele.Clear();
            z_tele.Clear();
            h_tele1.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tele_label2.Clear();
            x_tele2.Clear();
            y_tele2.Clear();
            z_tele2.Clear();
            h_tele2.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tele_label3.Clear();
            x_tele3.Clear();
            y_tele3.Clear();
            z_tele3.Clear();
            h_tele3.Clear();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tele_label4.Clear();
            x_tele4.Clear();
            y_tele4.Clear();
            z_tele4.Clear();
            h_tele4.Clear();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            sd1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string[] lines = { tele_label1.Text, x_tele.Text, y_tele.Text, z_tele.Text, h_tele1.Text };
                Stream s = sd1.OpenFile();
                StreamWriter sw = new StreamWriter(s, Encoding.Unicode);
                foreach (string line in lines)
                    sw.WriteLine(line);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file. Please try again later. Error message: " + ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            sd2.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            sd3.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            sd4.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            od1.ShowDialog();
        }

        private void sd2_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string[] lines = { tele_label2.Text, x_tele2.Text, y_tele2.Text, z_tele2.Text, h_tele2.Text };
                Stream s = sd2.OpenFile();
                StreamWriter sw = new StreamWriter(s, Encoding.Unicode);
                foreach (string line in lines)
                    sw.WriteLine(line);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file. Please try again later. Error message: " + ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void sd3_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string[] lines = { tele_label3.Text, x_tele3.Text, y_tele3.Text, z_tele3.Text, h_tele3.Text };
                Stream s = sd3.OpenFile();
                StreamWriter sw = new StreamWriter(s, Encoding.Unicode);
                foreach (string line in lines)
                    sw.WriteLine(line);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file. Please try again later. Error message: " + ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void sd4_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string[] lines = { tele_label4.Text, x_tele4.Text, y_tele4.Text, z_tele4.Text, h_tele4.Text };
                Stream s = sd4.OpenFile();
                StreamWriter sw = new StreamWriter(s, Encoding.Unicode);
                foreach (string line in lines)
                    sw.WriteLine(line);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file. Please try again later. Error message: " + ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void od1_FileOk(object sender, CancelEventArgs e)
        {
            using (StreamReader sr = new StreamReader(od1.FileName))
            {
                tele_label1.Text = sr.ReadLine();
                x_tele.Text = sr.ReadLine();
                y_tele.Text = sr.ReadLine();
                z_tele.Text = sr.ReadLine();
                h_tele1.Text = sr.ReadLine();
            }
        }

        private void autoLoad_FileOk(object sender, CancelEventArgs e)
        {
            string loop = "dont";
            if (checkBox1.Checked == true)
                loop = "loop";

            string arguments = listView2.SelectedItems[0].SubItems[0].Text + " \"" + codeFile + "\" " + loop + " \"" + autoLoad.FileName + "\"";
            Process.Start(Application.StartupPath + Path.DirectorySeparatorChar + "AutoBot.exe", arguments);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            od2.ShowDialog();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            od3.ShowDialog();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            od4.ShowDialog();
        }

        private void od2_FileOk(object sender, CancelEventArgs e)
        {
            using (StreamReader sr = new StreamReader(od2.FileName))
            {
                tele_label2.Text = sr.ReadLine();
                x_tele2.Text = sr.ReadLine();
                y_tele2.Text = sr.ReadLine();
                z_tele2.Text = sr.ReadLine();
                h_tele2.Text = sr.ReadLine();
            }
        }

        private void od3_FileOk(object sender, CancelEventArgs e)
        {
            using (StreamReader sr = new StreamReader(od3.FileName))
            {
                tele_label3.Text = sr.ReadLine();
                x_tele3.Text = sr.ReadLine();
                y_tele3.Text = sr.ReadLine();
                z_tele3.Text = sr.ReadLine();
                h_tele3.Text = sr.ReadLine();
            }
        }

        private void od4_FileOk(object sender, CancelEventArgs e)
        {
            using (StreamReader sr = new StreamReader(od4.FileName))
            {
                tele_label4.Text = sr.ReadLine();
                x_tele4.Text = sr.ReadLine();
                y_tele4.Text = sr.ReadLine();
                z_tele4.Text = sr.ReadLine();
                h_tele4.Text = sr.ReadLine();
            }
        }

        private void sdall_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                string[] lines = { tele_label1.Text, x_tele.Text, y_tele.Text, z_tele.Text, h_tele1.Text, tele_label2.Text, x_tele2.Text, y_tele2.Text, z_tele2.Text, h_tele2.Text, tele_label3.Text, x_tele3.Text, y_tele3.Text, z_tele3.Text, h_tele3.Text, tele_label4.Text, x_tele4.Text, y_tele4.Text, z_tele4.Text, h_tele4.Text };
                Stream s = sdall.OpenFile();
                StreamWriter sw = new StreamWriter(s, Encoding.Unicode);
                foreach (string line in lines)
                    sw.WriteLine(line);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not write file. Please try again later. Error message: " + ex.Message, "Error Writing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void saveAllBtn_Click(object sender, EventArgs e)
        {
            sdall.ShowDialog();
        }

        private void loadAllBtn_Click(object sender, EventArgs e)
        {
            odall.ShowDialog();
        }

        private void odall_FileOk(object sender, CancelEventArgs e)
        {
            using (StreamReader sr = new StreamReader(odall.FileName))
            {
                tele_label1.Text = sr.ReadLine();
                x_tele.Text = sr.ReadLine();
                y_tele.Text = sr.ReadLine();
                z_tele.Text = sr.ReadLine();
                h_tele1.Text = sr.ReadLine();
                tele_label2.Text = sr.ReadLine();
                x_tele2.Text = sr.ReadLine();
                y_tele2.Text = sr.ReadLine();
                z_tele2.Text = sr.ReadLine();
                h_tele2.Text = sr.ReadLine();
                tele_label3.Text = sr.ReadLine();
                x_tele3.Text = sr.ReadLine();
                y_tele3.Text = sr.ReadLine();
                z_tele3.Text = sr.ReadLine();
                h_tele3.Text = sr.ReadLine();
                tele_label4.Text = sr.ReadLine();
                x_tele4.Text = sr.ReadLine();
                y_tele4.Text = sr.ReadLine();
                z_tele4.Text = sr.ReadLine();
                h_tele4.Text = sr.ReadLine();
            }
        }

        private void gateBtn_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("EQMac"))
                MemLib.writeMemory("gate", codeFile, "bytes", "2");
            else
                MessageBox.Show("Currently only works in EQMac");
        }

        private void followBtn_Click(object sender, EventArgs e)
        {
            if (Follow == true)
                Follow = false;
            else
                Follow = true;
        }

        private void resetRunBtn_Click(object sender, EventArgs e)
        {
            runBox.Text = "0.6999999881";
        }

        private void buttonAllScriptsEnabled_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewScripts.Items)
            {
                listViewItem.Checked = true;
            }
        }

        private void buttonAllScriptsDisabled_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewScripts.Items)
                listViewItem.Checked = false;
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.newagesoldier.com");
        }

        private void x64CDependenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=5555");
        }

        private void buttonRefreshSpawnList_Click(object sender, EventArgs e)
        {
            listViewSpawnList.Items.Clear();
            TrainerForm_RefreshSpawnList();
        }

        private void buttonTargetSpawn_Click(object sender, EventArgs e)
        {
            if (listViewSpawnList.SelectedItems.Count == 0)
                return;
            string address_text = listViewSpawnList.SelectedItems[0].SubItems[1].Text;
            int address_value = Convert.ToInt32(address_text, 16);
            byte[] buffer = BitConverter.GetBytes(address_value);
            MemLib.writeUIntPtr("targetSpawn", codeFile, buffer);
        }

        private void buttonWarpToSpawn_Click(object sender, EventArgs e)
        {
            TrainerForm_WarpToSpawn();
        }

        private void listViewSpawnList_DoubleClick(object sender, EventArgs e)
        {
            if (listViewSpawnList.SelectedItems.Count == 1)
                TrainerForm_WarpToSpawn();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            autoLoad.ShowDialog();
        }

        static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        void Teleport( float value_x, float value_y, float value_z, float value_h) //it's actually y,x,z
        {
            byte[] write_y = BitConverter.GetBytes(value_y);
            byte[] write_x = BitConverter.GetBytes(value_x);
            byte[] write_z = BitConverter.GetBytes(value_z);

            MemLib.writeUIntPtr("safeY", codeFile, write_y);
            MemLib.writeUIntPtr("safeX", codeFile, write_x);
            MemLib.writeUIntPtr("safeZ", codeFile, write_z);

            float readSafeZ = MemLib.readFloat("safeZ", codeFile);
            float readSafeX = MemLib.readFloat("safeX", codeFile);
            float readSafeY = MemLib.readFloat("safeY", codeFile);

            MemLib.writeMemory("playerHeading", codeFile, "float", value_h.ToString());

            if (readSafeY.Equals(value_y) && readSafeX.Equals(value_x) && readSafeZ.Equals(value_z))
            {
                if (Convert.ToSingle(x_label.Text) == 0 && Convert.ToSingle(y_label.Text) == 0) // zoning...
                    return;

                if (Properties.Settings.Default.old_warp == true)
                    inject(Application.StartupPath + Path.DirectorySeparatorChar + "builds" + Path.DirectorySeparatorChar + comboBox1.Text + Path.DirectorySeparatorChar + "old_inject.dll");
                else
                {
                    Thread ClientThread = new Thread(MemLib.ThreadStartClient);
                    ClientThread.Start();
                }
            }
            else
                Teleport(value_x, value_y, value_z, value_h); //try again.
        }

        string TrimPrefixZero(string str)
        {
            if (str.StartsWith("0"))
                str = str.Remove(0, "0".Length);
            return str;
        }

        string getBuffName(int buffAddress)
        {
            System.IO.StreamReader buffFile = new System.IO.StreamReader("buffs.txt");
            string line;
            string wordString = "N/A";
            string[] buffnames = new string[12];
            while ((line = buffFile.ReadLine()) != null)
            {
                if (line.Contains("#"))
                    continue;
                string[] words = line.Split('^');
                int wordsid = Int32.Parse(words[0]);
                if (wordsid == buffAddress)
                {
                    wordString = words[1];
                    break;
                }
            }
            buffFile.Close();
            return wordString;
        }

        string getBuffTime(ulong buffTime)
        {
            return ((buffTime / (256 * 256 * 256)) * 0.1).ToString();
        }
        string getMacBuffTime(int buffTime)
        {
            return ((buffTime / (255)) * 0.1).ToString();
        }

        int oldmpProgressBar = 0;
        int oldxpProgressBar = 0;
        int oldhpProgressBar = 0;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                comboBox1.Invoke(new MethodInvoker(delegate
                {
                    if (comboBox1.Text.Equals(" ")) //no build version selected? Let's try to detect it!
                    {
                        string[] subdirectoryEntries = Directory.GetDirectories(Application.StartupPath + @"\builds");
                        foreach (string subdirectory in subdirectoryEntries)
                        {
                            string buildDateCode = MemLib.readString(Path.GetFileName(subdirectory) + "_code", Application.StartupPath + @"\builds.ini");
                            string buildDate = MemLib.LoadCode(Path.GetFileName(subdirectory) + "_date", Application.StartupPath + @"\builds.ini");
                            if (buildDateCode.Contains(buildDate))
                            {
                                comboBox1.Invoke(new MethodInvoker(delegate { comboBox1.Text = Path.GetFileName(subdirectory); }));
                                if (Properties.Settings.Default.old_warp == false)
                                    inject(Application.StartupPath + Path.DirectorySeparatorChar + "builds" + Path.DirectorySeparatorChar + comboBox1.Text + Path.DirectorySeparatorChar + "inject.dll");
                            }
                        }
                    }
                }
                ));

                float y_address = MemLib.readFloat("playerY", codeFile);
                float x_address = MemLib.readFloat("playerX", codeFile);
                float z_address = MemLib.readFloat("playerZ", codeFile);
                float heading = MemLib.readFloat("playerHeading", codeFile);

                string map_address = RemoveSpecialCharactersTwo(MemLib.readString("mapLongName", codeFile));
                string mapShortName = MemLib.RemoveSpecialCharacters(MemLib.readString("mapShortName", codeFile));

                map_label.Invoke(new MethodInvoker(delegate { map_label.Text = map_address; }));

                string scriptDirectory = Application.StartupPath + Path.DirectorySeparatorChar + "telescripts" + Path.DirectorySeparatorChar;
                string currentZone = scriptDirectory + RemoveSpecialCharactersTwo(map_address);

                this.od1.Reset();
                this.od2.Reset();
                this.od3.Reset();
                this.od4.Reset();
                this.sd1.Reset();
                this.sd2.Reset();
                this.sd3.Reset();
                this.sd4.Reset();

                if (currentZone.Contains("Greater Faydark") && !currentZone.Contains("The Greater Faydark"))
                    currentZone = currentZone.Replace("Greater Faydark", "The Greater Faydark");

                if (Directory.Exists(currentZone))
                {
                    od1.InitialDirectory = currentZone;
                    od2.InitialDirectory = currentZone;
                    od3.InitialDirectory = currentZone;
                    od4.InitialDirectory = currentZone;
                    sd1.InitialDirectory = currentZone;
                    sd2.InitialDirectory = currentZone;
                    sd3.InitialDirectory = currentZone;
                    sd4.InitialDirectory = currentZone;
                }
                else
                {
                    od1.InitialDirectory = scriptDirectory; 
                    od2.InitialDirectory = scriptDirectory;
                    od3.InitialDirectory = scriptDirectory;
                    od4.InitialDirectory = scriptDirectory;
                    sd1.InitialDirectory = scriptDirectory;
                    sd2.InitialDirectory = scriptDirectory;
                    sd3.InitialDirectory = scriptDirectory;
                    sd4.InitialDirectory = scriptDirectory;
                }

                int bank_plat_int = MemLib.readInt("bankPlat", codeFile);
                int bank_gold_int = MemLib.readInt("bankGold", codeFile);
                int bank_silver_int = MemLib.readInt("bankSilver", codeFile);
                int bank_copper_int = MemLib.readInt("bankCopper", codeFile);

                int player_plat_int = MemLib.readInt("playerPlat", codeFile);
                int player_gold_int = MemLib.readInt("playerGold", codeFile);
                int player_silver_int = MemLib.readInt("playerSilver", codeFile);
                int player_copper_int = MemLib.readInt("playerCopper", codeFile);

                bank_plat.Invoke(new MethodInvoker(delegate { bank_plat.Text = bank_plat_int.ToString(); }));
                bank_gold.Invoke(new MethodInvoker(delegate { bank_gold.Text = bank_gold_int.ToString(); }));
                bank_silver.Invoke(new MethodInvoker(delegate { bank_silver.Text = bank_silver_int.ToString(); }));
                bank_copper.Invoke(new MethodInvoker(delegate { bank_copper.Text = bank_copper_int.ToString(); }));

                player_plat.Invoke(new MethodInvoker(delegate { if (player_plat_int >= 0) player_plat.Text = player_plat_int.ToString(); }));
                player_gold.Invoke(new MethodInvoker(delegate { if (player_gold_int >= 0) player_gold.Text = player_gold_int.ToString(); }));
                player_silver.Invoke(new MethodInvoker(delegate { if (player_silver_int >= 0) player_silver.Text = player_silver_int.ToString(); }));
                player_copper.Invoke(new MethodInvoker(delegate { if (player_copper_int >= 0) player_copper.Text = player_copper_int.ToString(); }));

                decimal cRemaining = ((decimal)(bank_copper_int + player_copper_int) / 10) - Math.Floor((decimal)(bank_copper_int + player_copper_int) / 10);
                int silverTotal = ((bank_copper_int + player_copper_int) / 10) + (bank_silver_int + player_silver_int);
                decimal sRemaining = ((decimal)(silverTotal) / 10) - Math.Floor((decimal)(silverTotal) / 10);
                int goldTotal = ((silverTotal) / 10) + (bank_gold_int + player_gold_int);
                decimal gRemaining = ((decimal)(goldTotal) / 10) - Math.Floor((decimal)(goldTotal) / 10);
                int platTotal = ((goldTotal) / 10) + (bank_plat_int + player_plat_int);

                if (platTotal >= 0)
                    total_plat.Text = platTotal.ToString();
                else
                    total_plat.Text = "0";

                string tGold = TrimPrefixZero(gRemaining.ToString().Replace(".", ""));
                if (tGold.Equals("") || gRemaining < 0)
                    total_gold.Text = "0";
                else
                    total_gold.Text = tGold;

                string tSilver = TrimPrefixZero(sRemaining.ToString().Replace(".", ""));
                if (tSilver.Equals("") || sRemaining < 0)
                    total_silver.Text = "0";
                else
                    total_silver.Text = tSilver;

                string tCopper = TrimPrefixZero(cRemaining.ToString().Replace(".", ""));
                if (tCopper.Equals("") || cRemaining < 0)
                    total_copper.Text = "0";
                else
                    total_copper.Text = tCopper;

                y_label.Invoke(new MethodInvoker(delegate { y_label.Text = y_address.ToString(); }));
                x_label.Invoke(new MethodInvoker(delegate { x_label.Text = x_address.ToString(); }));
                z_label.Invoke(new MethodInvoker(delegate { z_label.Text = z_address.ToString(); }));
                heading_label.Invoke(new MethodInvoker(delegate { heading_label.Text = heading.ToString(); }));

                string char_name = MemLib.readString("PlayerName", codeFile);
                int char_lvl = MemLib.readInt("playerLvl", codeFile);
                int current_hp = MemLib.readInt("PlayerCurrentHP", codeFile);
                int max_hp = MemLib.readInt("PlayerMaxHP", codeFile);

                int max_mp = 0;
                int current_mp = 0;

                if (comboBox1.Text.Equals("EQMac"))
                {
                    max_mp = MemLib.readByte("PlayerMaxMP", codeFile);
                    current_mp = MemLib.readByte("PlayerCurrentMP", codeFile);
                }
                else
                {
                    max_mp = MemLib.readInt("PlayerMaxMP", codeFile);
                    current_mp = MemLib.readInt("PlayerCurrentMP", codeFile);
                }

                int current_xp = MemLib.readInt("PlayerExp", codeFile);

                int mousexVal = MemLib.readInt("mousex", codeFile);
                int mouseyVal = MemLib.readInt("mousey", codeFile);

                mousey.Invoke(new MethodInvoker(delegate { mousey.Text = mouseyVal.ToString(); }));
                mousex.Invoke(new MethodInvoker(delegate { mousex.Text = mousexVal.ToString(); }));

                string t_name = MemLib.readString("targetName", codeFile);
                
                int t_level = MemLib.readByte("TargetLevel", codeFile);

                if (t_level >= 1)
                {
                    label11.Invoke(new MethodInvoker(delegate { label11.Text = "Level: " + t_level.ToString(); }));
                    followBtn.Invoke(new MethodInvoker(delegate { followBtn.Enabled = true; }));
                }
                else
                {
                    label11.Invoke(new MethodInvoker(delegate { label11.Text = "Level: "; }));
                    followBtn.Enabled = false;
                }

                if (t_level >= 1)
                {
                    label14.Invoke(new MethodInvoker(delegate { label14.Text = "Name: " + t_name; }));
                    followBtn.Enabled = true;
                }
                else
                {
                    label14.Invoke(new MethodInvoker(delegate { label14.Text = "Name: "; }));
                    followBtn.Enabled = false;
                }
                
                int t_class = MemLib.readByte("targetClass", codeFile);

                label12.Invoke(new MethodInvoker(delegate { label12.Text = "Class: " + charClass(t_class); }));
                name_label.Invoke(new MethodInvoker(delegate { name_label.Text = char_name + " (" + char_lvl.ToString() + ")"; }));

                int cur_xp;
                if (current_xp > 330)
                    cur_xp = 0;
                else
                    cur_xp = current_xp;

                float run_speed = MemLib.readFloat("runSpeed", codeFile);
                if (!runBox.Text.Equals("") && !run_speed.Equals(float.Parse(runBox.Text)))
                    MemLib.writeMemory("runSpeed", codeFile, "float", runBox.Text);

                float t_z_address = MemLib.readFloat("targetZ", codeFile);
                float t_y_address = MemLib.readFloat("targetY", codeFile);
                float t_x_address = MemLib.readFloat("targetX", codeFile);
                float t_h_address = MemLib.readFloat("targetHeading", codeFile);

                int tHealth = MemLib.readInt("targetHealth", codeFile);
                if (tHealth > 100)
                    tHealth = 100;

                int thealth_max = 100; //for NPCs, it's always 100.

                string display_thealth = "";
                if (thealth_max == 100) //FIX: Need to compare this to max HP. If max HP is not 100, then its you.
                    display_thealth = tHealth.ToString() + "%";
                else
                    display_thealth = tHealth.ToString() + " / " + thealth_max.ToString();

                t_health.Invoke(new MethodInvoker(delegate { t_health.Text = "Health: " + display_thealth; }));
                target_y.Invoke(new MethodInvoker(delegate { target_y.Text = "Y: " + t_y_address.ToString(); }));
                target_x.Invoke(new MethodInvoker(delegate { target_x.Text = "X: " + t_x_address.ToString(); }));
                target_z.Invoke(new MethodInvoker(delegate { target_z.Text = "Z: " + t_z_address.ToString(); }));
                target_h.Invoke(new MethodInvoker(delegate { target_h.Text = "H: " + t_h_address.ToString(); }));

                    int skipNum = 0;
                    int sNum = 0;
                    ListViewItem findBuff;

                    if (comboBox1.Text.Equals("EQTitanium"))
                        skipNum = 19;
                    if (comboBox1.Text.Equals("EQMac"))
                        skipNum = 9;

                    // CREATE BUFF LIST
                    Dictionary<string, int> buffsMac = new Dictionary<string, int>();
                    Dictionary<string, ulong> buffs = new Dictionary<string, ulong>();
                    for (int i = 0; i < 12; i++)
                    {
                        int buffAddress = 0;

                        if (i > 0)
                        {
                            sNum = sNum + skipNum;
                            buffAddress = MemLib.read2ByteMove("buffsInfoAddress", codeFile, sNum);
                        }
                        else
                            buffAddress = MemLib.read2Byte("buffsInfoAddress", codeFile);

                        sNum = sNum + 1;

                        if (buffAddress > 0 && buffAddress < 8445)
                        {
                            //MessageBox.Show(buffAddress.ToString() + getBuffName(buffAddress) + " time=" + MemLib.readIntMove("buffsInfoAddress", codeFile, sNum));
                            if (comboBox1.Text.Equals("EQMac"))
                                buffsMac.Add(getBuffName(buffAddress), MemLib.readIntMove("buffsInfoAddress", codeFile, sNum));
                            else
                                buffs.Add(getBuffName(buffAddress), MemLib.readUIntMove("buffsInfoAddress", codeFile, sNum));
                        }
                    }

                    //var message = string.Join(Environment.NewLine, buffsMac);
                    //MessageBox.Show(message);

                    // ADD BUFFS                    
                    if (comboBox1.Text.Equals("EQMac"))
                    {
                        foreach (KeyValuePair<string, int> entry in buffsMac)
                        {
                            findBuff = listView1.FindItemWithText(entry.Key);
                            if (findBuff == null)
                            {
                                string[] row = { getMacBuffTime(entry.Value), entry.Key };
                                var listViewItem = new ListViewItem(row);
                                listView1.Items.Add(listViewItem);
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, ulong> entry in buffs)
                        {
                            findBuff = listView1.FindItemWithText(entry.Key);
                            if (findBuff == null)
                            {
                                string[] row = { getBuffTime(entry.Value), entry.Key };
                                var listViewItem = new ListViewItem(row);
                                listView1.Items.Add(listViewItem);
                            }
                        }
                    }

                    // REMOVE & UPDATE BUFFS
                    if (buffRefresh == 30)
                    {
                        foreach (ListViewItem item in listView1.Items)
                        {
                            buffRefresh = 0;
                            if (comboBox1.Text.Equals("EQMac"))
                            {
                                if (buffsMac.ContainsKey(item.SubItems[1].Text))
                                {
                                    item.SubItems[0].Text = getMacBuffTime(buffsMac[item.SubItems[1].Text]);
                                }
                                else
                                    listView1.Items.Remove(item);
                            }
                            else
                            {
                                if (buffs.ContainsKey(item.SubItems[1].Text))
                                    item.SubItems[0].Text = getBuffTime(buffs[item.SubItems[1].Text]);
                                else
                                    listView1.Items.Remove(item);
                            }
                        }
                    }
                    buffRefresh++;

                    double xpProgressPercentage = ((double)current_xp / (double)330);
                    int xpProgressBar = (int)(xpProgressPercentage * 100);
                    if (xpProgressBar < 0) xpProgressBar = 0;

                    double hpProgressPercentage = ((double)current_hp / (double)max_hp);
                    int hpProgressBar = (int)(hpProgressPercentage * 100);
                    if (hpProgressBar < 0) hpProgressBar = 0;

                    double mpProgressPercentage = ((double)current_mp / (double)max_mp);
                    int mpProgressBar = (int)(mpProgressPercentage * 100);
                    if (mpProgressBar < 0) mpProgressBar = 0;

                    if (oldxpProgressBar != current_xp)
                    {
                        progressBarXP.Value = xpProgressBar;
                        progressBarXP.Refresh();
                        if (current_xp >= 0 && current_xp <= 330)
                            xp_stats.Text = xpProgressBar.ToString() + "%";
                        SendMessage(progressBarXP.Handle, 0x400 + 16, (IntPtr)3, IntPtr.Zero);
                        System.Threading.Thread.Sleep(50);
                        progressBarXP.CreateGraphics().DrawString(current_xp.ToString() + "/330", new Font("Arial", (float)8), Brushes.Black, new PointF(5, progressBarXP.Height / 2 - 7));
                    }
                    oldxpProgressBar = current_xp;
                    
                    if (oldhpProgressBar != current_hp)
                    {
                        progressBarHP.Value = hpProgressBar;
                        progressBarHP.Refresh();
                        if (max_hp > 1)
                            hp_stats.Text = hpProgressBar.ToString() + "%";
                        SendMessage(progressBarHP.Handle, 0x400 + 16, (IntPtr)2, IntPtr.Zero);
                        System.Threading.Thread.Sleep(50);
                        progressBarHP.CreateGraphics().DrawString(current_hp + "/" + max_hp, new Font("Arial", (float)8), Brushes.Black, new PointF(5, progressBarHP.Height / 2 - 7));
                    }
                    oldhpProgressBar = current_hp;
                    
                    if (oldmpProgressBar != current_mp)
                    {
                        progressBarMP.Value = mpProgressBar;
                        progressBarMP.Refresh();
                        if (max_mp >= 0)
                            mp_stats.Text = mpProgressBar.ToString() + "%";
                        else
                            mp_stats.Text = "[0/0] 0%";
                        progressBarMP.CreateGraphics().DrawString(current_mp + "/" + max_mp, new Font("Arial", (float)8), Brushes.Black, new PointF(5, progressBarMP.Height / 2 - 7));
                    }
                    oldmpProgressBar = current_mp;
                    
                        foreach (ListViewItem listViewItem in listViewScripts.Items)
                        {
                            int script_instructions_index = 3; // disabled column

                            if (listViewItem.Checked == true)
                                script_instructions_index = 2; // enabled column

                            string script_instructions = listViewItem.SubItems[script_instructions_index].Text;
                            string[] script_instructions_split = script_instructions.Split('^');

                            foreach (string script_instruction in script_instructions_split)
                            {
                                if (script_instruction.Length == 0)
                                    continue;

                                string[] script_instruction_split = script_instruction.Split(':');
                                string script_instruction_type = "";
                                string script_instruction_value = "";
                                int script_instruction_address_int = 0;
                                int script_instruction_value_int = 0;
                                byte[] script_instruction_value_bytes;

                                if (script_instruction_split[0] == "pointer" && script_instruction_split[2] == "offsets")
                                {
                                    int script_instruction_pointer = Int32.Parse(script_instruction_split[1], System.Globalization.NumberStyles.AllowHexSpecifier);
                                    int script_instruction_address = MemLib.readUInt((UIntPtr)script_instruction_pointer);

                                    string script_instruction_offsets = script_instruction_split[3];
                                    string[] script_instruction_offsets_split = script_instruction_offsets.Split(',');
                                    int current_offset = 1;
                                    int num_offsets = script_instruction_offsets_split.Length;

                                    foreach (string script_instruction_offset in script_instruction_offsets_split)
                                    {
                                        int script_instruction_offset_int = Int32.Parse(script_instruction_offset, System.Globalization.NumberStyles.AllowHexSpecifier);
                                        script_instruction_address_int = script_instruction_address;
                                        script_instruction_address_int += script_instruction_offset_int;
                                        if (current_offset == num_offsets)
                                            break;

                                        int script_instruction_address_after_offset = MemLib.readUInt((UIntPtr)script_instruction_address_int);
                                        script_instruction_address = script_instruction_address_after_offset;
                                        script_instruction_address_int = script_instruction_address;
                                    }
                                    script_instruction_type = script_instruction_split[4];
                                    script_instruction_value = script_instruction_split[5];
                                }
                                else
                                {
                                    script_instruction_address_int = Int32.Parse(script_instruction_split[0], System.Globalization.NumberStyles.AllowHexSpecifier);
                                    int script_instruction_address = script_instruction_address_int;
                                    script_instruction_type = script_instruction_split[1];
                                    script_instruction_value = script_instruction_split[2];
                                }

                                int i = 0;

                                try
                                {
                                    switch (script_instruction_type)
                                    {
                                        case "nops":
                                            int num_nops = Int32.Parse(script_instruction_value, System.Globalization.NumberStyles.AllowHexSpecifier);
                                            byte[] nops = new byte[num_nops];
                                            for (i = 0; i < nops.Length; i++)
                                                nops[i] = 0x90;
                                            MemLib.writeByte((UIntPtr)script_instruction_address_int, nops, num_nops);
                                            break;

                                        case "bytes":
                                            if (script_instruction_value.Contains(','))
                                            {
                                                string[] script_instruction_value_split = script_instruction_value.Split(',');
                                                int num_bytes = script_instruction_value_split.Length;
                                                byte[] write_bytes = new byte[num_bytes];
                                                for (i = 0; i < num_bytes; i++)
                                                {
                                                    script_instruction_value_int = Int32.Parse(script_instruction_value_split[i], System.Globalization.NumberStyles.AllowHexSpecifier);
                                                    script_instruction_value_bytes = BitConverter.GetBytes(script_instruction_value_int);
                                                    write_bytes[i] = script_instruction_value_bytes[0];
                                                }
                                                MemLib.writeByte((UIntPtr)script_instruction_address_int, write_bytes, num_bytes);
                                            }
                                            else
                                            {
                                                script_instruction_value_int = Int32.Parse(script_instruction_value, System.Globalization.NumberStyles.AllowHexSpecifier);
                                                script_instruction_value_bytes = BitConverter.GetBytes(script_instruction_value_int);
                                                MemLib.writeByte((UIntPtr)script_instruction_address_int, script_instruction_value_bytes, 1);
                                            }
                                            break;

                                        case "byte":
                                            script_instruction_value_int = Int32.Parse(script_instruction_value, System.Globalization.NumberStyles.AllowHexSpecifier);
                                            script_instruction_value_bytes = BitConverter.GetBytes(script_instruction_value_int);
                                            MemLib.writeByte((UIntPtr)script_instruction_address_int, script_instruction_value_bytes, 1);
                                            break;

                                        case "word":
                                            script_instruction_value_int = Int32.Parse(script_instruction_value, System.Globalization.NumberStyles.AllowHexSpecifier);
                                            script_instruction_value_bytes = BitConverter.GetBytes(script_instruction_value_int);

                                            MemLib.writeByte((UIntPtr)script_instruction_address_int, script_instruction_value_bytes, 2);
                                            break;

                                        case "dword":
                                            script_instruction_value_int = Int32.Parse(script_instruction_value, System.Globalization.NumberStyles.AllowHexSpecifier);
                                            script_instruction_value_bytes = BitConverter.GetBytes(script_instruction_value_int);

                                            MemLib.writeByte((UIntPtr)script_instruction_address_int, script_instruction_value_bytes, 4);
                                            break;

                                        case "float":
                                            float script_instruction_value_float = float.Parse(script_instruction_value);
                                            script_instruction_value_bytes = BitConverter.GetBytes(script_instruction_value_float);

                                            MemLib.writeByte((UIntPtr)script_instruction_address_int, script_instruction_value_bytes, 1);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }

                Thread.Sleep(100);
            }
        }

        private void refreshProcessesBtn_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName == "eqgame")
                    listView2.Items.Add(theprocess.Id.ToString());
            }
            if (listView2.Items.Count > 0)
            {
                if (listView2.SelectedItems.Count == 0)
                {
                    listView2.Items[0].Selected = true;
                    listView2.Select();
                }
            }
        }        

        private void textBoxSpawnListFilter_GotFocus(object sender, EventArgs e)
        {
            this.AcceptButton = buttonRefreshSpawnList;
        }

        private void buttonResetCamera_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("EQMac"))
            {
                // read from --> 0x007F94CC,0x84,0x00
                // write to (int) --> 0x0063D6C0
            }
            else
                MessageBox.Show("Currently only works for EQMac");
        }

        private void buttonCameraOnSpawn_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("EQMac"))
            {
                if (listViewSpawnList.SelectedItems.Count == 0)
                    return;

                string address_text = listViewSpawnList.SelectedItems[0].SubItems[1].Text;
                int spawn_info = Convert.ToInt32(address_text, 16);

                // read from --> spawn_info,0x84,0x00
                // write to (int) --> 0x0063D6C0
            }
            else
                MessageBox.Show("Currently only works for EQMac");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TrainerForm_LoadScripts();
        }

        private void autoItV3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.autoitscript.com/cgi-bin/getfile.pl?autoit3/autoit-v3-setup.exe");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inject(Application.StartupPath + Path.DirectorySeparatorChar + "builds" + Path.DirectorySeparatorChar + comboBox1.Text + Path.DirectorySeparatorChar + "inject.dll");
        }

        private void changeProcess()
        {
            MemLib.closeProcess();
            MemLib.OpenGameProcess(eqgameID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            miniToolbar obj2 = new miniToolbar();
            obj2.RefToForm1 = this;
            teleForm obj3 = new teleForm();

            //this.Visible = false;
            obj2.Show();
            this.WindowState = FormWindowState.Minimized;
        }

        private void formClosed(object sender, FormClosedEventArgs e)
        {
            MemLib.closeProcess();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsForm obj = new settingsForm();
            obj.RefToForm1 = this;
            obj.Show();
            obj.Location = new Point(this.Location.X, this.Location.Y);
        }

        private void openMapSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapForm obj3 = new mapForm();
            obj3.RefToForm1 = this;
            obj3.Show();
        }

        private void aboutEQTrainerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 obj = new AboutBox1();
            obj.Show();
        }

        private void softwareReadmeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("readme.txt");
        }

        private void softwareInformationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://newagesoldier.com/everquest-mac-on-pc-trainer-teleporter/");
        }

        private void softwareSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://newagesoldier.com/forum/viewforum.php?f=3");
        }

        private void autoItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.autoitscript.com/cgi-bin/getfile.pl?autoit3/autoit-v3-setup.exe");
        }

        private void cRedistributablex86ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=5555");
        }

        /*private void tele_label1_TextChanged(object sender, EventArgs e)
        {
            //TrainerForm f = (TrainerForm)this.Owner;
            //f.TextBoxText = txtChildTextBox.Text;
        }*/

    }
}
