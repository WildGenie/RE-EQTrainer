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

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

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
        public static IntPtr pHandle;
        Mem MemLib = new Mem();

        public miniToolbar RefToMiniForm { get; set; }

        private static Boolean Follow = false;
        protected ProcessModule myProcessModule;
        public static string eqgameID = "";
        byte[] memory = new byte[4];
        byte[] memoryBig = new byte[64];
        byte[] memoryGiant = new byte[255];
        public string codeFile = "";

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
                /*else if (id == 5) {
                    Form MapForm = new MapForm();
                    MapForm.Show();
                }*/ else if (id == 6)
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
                //teleForm teleForm = new teleForm();
                //teleForm.Visible = false;
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
            Int32 ProcID = Convert.ToInt32(eqgameID);
            Process procs = Process.GetProcessById(ProcID);
            IntPtr hProcess = (IntPtr)OpenProcess(0x1F0FFF, true, ProcID);

            if (procs.Responding == false)
                return;

            try
            {
                String strDLLName = dll;
                MemLib.InjectDLL(hProcess, strDLLName);
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
            if (comboBox1.Text == "EQMac")
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
                        string[] listView2Rows = { /*theprocess.ProcessName,*/ theprocess.Id.ToString() };
                        var listView2Items = new ListViewItem(listView2Rows);
                        listView2.Items.Add(listView2Items);
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
            int player_spawn_info = MemLib.readUIntPtr("spawnInfoAddress", codeFile);

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

        public int DllImageAddress(string dllname)
        {
            Int32 ProcID = Convert.ToInt32(eqgameID);
            Process MyProcess = Process.GetProcessById(ProcID);
            ProcessModuleCollection modules = MyProcess.Modules;

            foreach (ProcessModule procmodule in modules)
            {
                if (dllname == procmodule.ModuleName)
                    return (int)procmodule.BaseAddress;
            }
            return -1;
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
            System.Diagnostics.Process.Start("Maps.exe", listView2.SelectedItems[0].SubItems[0].Text + " " + comboBox1.Text);
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
        { //only works in mac
            if (comboBox1.Text == "EQMac")
                MemLib.writeMemory("gate", codeFile, "bytes", "2");
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
            //WriteProcessMemory(pHandle, MemLib.LoadUIntPtrCode("targetSpawn", codeFile), buffer, (UIntPtr)buffer.Length, IntPtr.Zero);
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

            float readSafeZ = MemLib.readUintPtrFloat("safeZ", codeFile);
            float readSafeX = MemLib.readUintPtrFloat("safeX", codeFile);
            float readSafeY = MemLib.readUintPtrFloat("safeY", codeFile);

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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (comboBox1.Text.Equals(" ")) //no build version selected? Let's try to detect it!
                {
                    string[] subdirectoryEntries = Directory.GetDirectories(Application.StartupPath + @"\builds");
                    foreach (string subdirectory in subdirectoryEntries)
                    {
                        string buildDateCode = MemLib.readBigString(Path.GetFileName(subdirectory) + "_code", Application.StartupPath + @"\builds.ini");
                        string buildDate = MemLib.LoadCode(Path.GetFileName(subdirectory) + "_date", Application.StartupPath + @"\builds.ini");
                        if (buildDateCode.Contains(buildDate))
                        {
                            //if (comboBox1.InvokeRequired)
                            comboBox1.Invoke(new MethodInvoker(delegate { comboBox1.Text = Path.GetFileName(subdirectory); }));
                            if (Properties.Settings.Default.old_warp == false)
                                inject(Application.StartupPath + Path.DirectorySeparatorChar + "builds" + Path.DirectorySeparatorChar + comboBox1.Text + Path.DirectorySeparatorChar + "inject.dll");
                        }
                    }
                }

                float y_address = MemLib.readFloat("playerY", codeFile);
                float x_address = MemLib.readFloat("playerX", codeFile);
                float z_address = MemLib.readFloat("playerZ", codeFile);
                float heading = MemLib.readFloat("playerHeading", codeFile);

                string map_address = RemoveSpecialCharactersTwo(MemLib.readUIntPtrStr("mapLongName", codeFile));
                string mapShortName = MemLib.RemoveSpecialCharacters(MemLib.readUIntPtrStr("mapShortName", codeFile).ToString());
                //if (map_label.InvokeRequired)
                    map_label.Invoke(new MethodInvoker(delegate { map_label.Text = map_address + " (" + mapShortName + ")"; }));

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

                player_plat.Invoke(new MethodInvoker(delegate { player_plat.Text = player_plat_int.ToString(); }));
                player_gold.Invoke(new MethodInvoker(delegate { player_gold.Text = player_gold_int.ToString(); }));
                player_silver.Invoke(new MethodInvoker(delegate { player_silver.Text = player_silver_int.ToString(); }));
                player_copper.Invoke(new MethodInvoker(delegate { player_copper.Text = player_copper_int.ToString(); }));

                decimal cRemaining = ((decimal)(bank_copper_int + player_copper_int) / 10) - Math.Floor((decimal)(bank_copper_int + player_copper_int) / 10);
                int silverTotal = ((bank_copper_int + player_copper_int) / 10) + (bank_silver_int + player_silver_int);
                decimal sRemaining = ((decimal)(silverTotal) / 10) - Math.Floor((decimal)(silverTotal) / 10);
                int goldTotal = ((silverTotal) / 10) + (bank_gold_int + player_gold_int);
                decimal gRemaining = ((decimal)(goldTotal) / 10) - Math.Floor((decimal)(goldTotal) / 10);
                int platTotal = ((goldTotal) / 10) + (bank_plat_int + player_plat_int);

                total_plat.Text = platTotal.ToString();

                string tGold = TrimPrefixZero(gRemaining.ToString().Replace(".", ""));
                if (tGold.Equals(""))
                    total_gold.Text = "0";
                else
                    total_gold.Text = tGold;

                string tSilver = TrimPrefixZero(sRemaining.ToString().Replace(".", ""));
                if (tSilver.Equals(""))
                    total_silver.Text = "0";
                else
                    total_silver.Text = tSilver;

                string tCopper = TrimPrefixZero(cRemaining.ToString().Replace(".", ""));
                if (tCopper.Equals(""))
                    total_copper.Text = "0";
                else
                    total_copper.Text = tCopper;

                y_label.Invoke(new MethodInvoker(delegate { y_label.Text = y_address.ToString(); }));
                x_label.Invoke(new MethodInvoker(delegate { x_label.Text = x_address.ToString(); }));
                z_label.Invoke(new MethodInvoker(delegate { z_label.Text = z_address.ToString(); }));
                heading_label.Invoke(new MethodInvoker(delegate { heading_label.Text = heading.ToString(); }));

                string char_name = MemLib.readString("PlayerName", codeFile);
                int current_hp = MemLib.readInt("PlayerCurrentHP", codeFile);
                int max_hp = MemLib.readInt("PlayerMaxHP", codeFile);

                int current_mp = MemLib.readByte("PlayerCurrentMP", codeFile);
                int max_mp = MemLib.readByte("PlayerMaxMP", codeFile);
                int current_xp = MemLib.readInt("PlayerExp", codeFile);

                int mousexVal = MemLib.readUIntPtr("mousex", codeFile);
                int mouseyVal = MemLib.readUIntPtr("mousey", codeFile);

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
                name_label.Invoke(new MethodInvoker(delegate { name_label.Text = char_name; }));

                int cur_xp;
                if (current_xp > 330)
                    cur_xp = 0;
                else
                    cur_xp = current_xp;


                double xpProgressPercentage = ((double)current_xp / (double)330);
                int xpProgressBar = (int)(xpProgressPercentage * 100);
                if (xpProgressBar < 0) xpProgressBar = 0;

                double hpProgressPercentage = ((double)current_hp / (double)max_hp);
                int hpProgressBar = (int)(hpProgressPercentage * 100);
                if (hpProgressBar < 0) hpProgressBar = 0;

                double mpProgressPercentage = ((double)current_mp / (double)max_mp);
                int mpProgressBar = (int)(mpProgressPercentage * 100);
                if (mpProgressBar < 0) mpProgressBar = 0;

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

                //int[] offsets23 = { 0x3F94eC, 0x98 }; //mac
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

                //only works in EQMac until I find Titanium codes
                if (comboBox1.Text.Equals("EQMac"))
                {
                    List<int> buffSpells = new List<int>();
                    List<int> buffTimer = new List<int>();

                    int spellOffset = 0x268; //start here

                    for (int i = 1; i <= 12; i++)
                    {
                        int[] newOffsets = { 0x003F94E8, spellOffset };
                        int buffid = (System.Int16)MemLib.readIntMove(newOffsets);
                        if (buffid.ToString() != "-1")
                        {//no legit ID? Dont bother.
                            buffSpells.Add(buffid);

                            spellOffset = spellOffset + 1;
                            int[] secondOffsets = { 0x003F94E8, spellOffset };
                            int bufftime = MemLib.readIntMove(newOffsets);
                            buffTimer.Add(bufftime);
                            spellOffset = spellOffset + 9;
                        }
                    }

                    int[] buffids = buffSpells.ToArray();

                    List<double> buffTimerList = new List<double>();

                    foreach (double newBuffTimer in buffTimer)
                    {
                        double trueTimer = (newBuffTimer / 255) * 0.1;
                        buffTimerList.Add(trueTimer);
                    }

                    double[] bufftimers = buffTimerList.ToArray();

                    if (listView1.InvokeRequired)
                        listView1.Invoke(new MethodInvoker(delegate { listView1.Items.Clear(); }));
                    string[] buffnames = new string[12];
                    string line;

                    for (int i = 0; i < buffids.Count(); i++)
                    {
                        System.IO.StreamReader file = new System.IO.StreamReader("buffs.txt");
                        while ((line = file.ReadLine()) != null)
                        {
                            if (line.Contains("#"))
                                continue;
                            string[] words = line.Split('^');
                            int wordsid = Int32.Parse(words[0]);
                            if (wordsid == buffids[i])
                            {
                                buffnames[i] = words[1];
                                break;
                            }
                        }
                        file.Close();
                    }

                    for (int i = 0; i < buffids.Count(); i++)
                    {
                        if ((bufftimers[i].ToString() != "0.1") && (bufftimers[i].ToString() != "0"))
                        {
                            int newBuffTime = (int)Math.Ceiling((double)bufftimers[i] / 8);
                            string time = (newBuffTime / 60) + ":" + (newBuffTime % 60).ToString("00");

                            string[] row = { time, buffnames[i] };
                            var listViewItem = new ListViewItem(row);
                            if (listView1.InvokeRequired)
                                listView1.Invoke(new MethodInvoker(delegate { listView1.Items.Add(listViewItem); }));
                        }
                    }
                }

                //if (checkBoxScripts.Checked == false)
                //    return;

                if (listViewScripts.InvokeRequired)
                    listViewScripts.Invoke(new MethodInvoker(delegate {

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
                                //byte[] script_instruction_address = new byte[4];
                                string script_instruction_type = "";
                                string script_instruction_value = "";
                                int script_instruction_address_int = 0;
                                int script_instruction_value_int = 0;
                                byte[] script_instruction_value_bytes;

                                if (script_instruction_split[0] == "pointer" && script_instruction_split[2] == "offsets")
                                {
                                    int script_instruction_pointer = Int32.Parse(script_instruction_split[1], System.Globalization.NumberStyles.AllowHexSpecifier);

                                    //ReadProcessMemory(pHandle, (UIntPtr)script_instruction_pointer, script_instruction_address, (UIntPtr)4, IntPtr.Zero);
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

                                        //byte[] script_instruction_address_after_offset = new byte[4];
                                        //ReadProcessMemory(pHandle, (UIntPtr)script_instruction_address_int, script_instruction_address_after_offset, (UIntPtr)4, IntPtr.Zero);
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

                    }));

                xp_stats.Invoke(new MethodInvoker(delegate { if (current_xp >= 0 && current_xp <= 330) xp_stats.Text = xpProgressBar.ToString() + "%"; }));
                progressBarXP.Invoke(new MethodInvoker(delegate { progressBarXP.Value = xpProgressBar; SendMessage(progressBarXP.Handle, 1040, (IntPtr)3, IntPtr.Zero); }));
                progressBarXP.CreateGraphics().DrawString(current_xp.ToString() + "/330", new Font("Arial", (float)8), Brushes.Black, new PointF(5, progressBarXP.Height / 2 - 7));

                hp_stats.Invoke(new MethodInvoker(delegate { if (max_hp > 1) hp_stats.Text = hpProgressBar.ToString() + "%"; }));
                progressBarHP.Invoke(new MethodInvoker(delegate { progressBarHP.Value = hpProgressBar; SendMessage(progressBarHP.Handle, 1040, (IntPtr)2, IntPtr.Zero); }));
                progressBarHP.CreateGraphics().DrawString(current_hp + "/" + max_hp, new Font("Arial", (float)8), Brushes.Black, new PointF(5, progressBarHP.Height / 2 - 7));

                mp_stats.Invoke(new MethodInvoker(delegate { if (max_mp >= 0) mp_stats.Text = mpProgressBar.ToString() + "%"; else mp_stats.Text = "[0/0] 0%"; }));
                progressBarMP.Invoke(new MethodInvoker(delegate { progressBarMP.Value = mpProgressBar; SendMessage(progressBarMP.Handle, 1040, (IntPtr)0, IntPtr.Zero); }));
                progressBarMP.CreateGraphics().DrawString(current_mp + "/" + max_mp, new Font("Arial", (float)8), Brushes.Black, new PointF(5, progressBarMP.Height / 2 - 7));

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
                {
                    string[] listView2Rows = { /*theprocess.ProcessName,*/ theprocess.Id.ToString() };
                    var listView2Items = new ListViewItem(listView2Rows);
                    listView2.Items.Add(listView2Items);
                }
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
            //only works in mac
            /*byte[] buffer = new byte[4];
            ReadProcessMemory(pHandle, (UIntPtr)0x7F94CC, buffer, (UIntPtr)4, IntPtr.Zero);

            int spawn_info = BitConverter.ToInt32(buffer, 0);

            byte[] buffer2 = new byte[4];
            ReadProcessMemory(pHandle, (UIntPtr)spawn_info + 0x84, buffer2, (UIntPtr)4, IntPtr.Zero);

            int actor_info = BitConverter.ToInt32(buffer2, 0);

            byte[] buffer3 = new byte[4];
            ReadProcessMemory(pHandle, (UIntPtr)actor_info + 0x00, buffer3, (UIntPtr)4, IntPtr.Zero);

            //int view_actor = BitConverter.ToInt32(buffer3, 0);
            //MessageBox.Show("view_actor: " + view_actor);

            WriteProcessMemory(pHandle, (UIntPtr)0x0063D6C0, buffer3, (UIntPtr)4, IntPtr.Zero);*/
        }

        private void buttonCameraOnSpawn_Click(object sender, EventArgs e)
        {
            //only works in mac
            /*if (listViewSpawnList.SelectedItems.Count == 0)
                return;

            string address_text = listViewSpawnList.SelectedItems[0].SubItems[1].Text;

            int address_value = Convert.ToInt32(address_text, 16);

            int spawn_info = address_value;

            byte[] buffer2 = new byte[4];
            ReadProcessMemory(pHandle, (UIntPtr)spawn_info + 0x84, buffer2, (UIntPtr)4, IntPtr.Zero);

            int actor_info = BitConverter.ToInt32(buffer2, 0);

            byte[] buffer3 = new byte[4];
            ReadProcessMemory(pHandle, (UIntPtr)actor_info + 0x00, buffer3, (UIntPtr)4, IntPtr.Zero);

            //int view_actor = BitConverter.ToInt32(buffer3, 0);
            //MessageBox.Show("view_actor: " + view_actor);

            WriteProcessMemory(pHandle, (UIntPtr)0x0063D6C0, buffer3, (UIntPtr)4, IntPtr.Zero);*/
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
            MemLib.OpenProcess(eqgameID);
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

        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            settingsForm obj = new settingsForm();
            obj.RefToForm1 = this;
            obj.Show();
            obj.Location = new Point(this.Location.X, this.Location.Y);
        }

        /*private void tele_label1_TextChanged(object sender, EventArgs e)
        {
            //TrainerForm f = (TrainerForm)this.Owner;
            //f.TextBoxText = txtChildTextBox.Text;
        }*/

    }
}
