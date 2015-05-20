﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using Memory;
using AutoItX3Lib;

namespace AutoBot
{
   public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region DllImports
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion

        private const int WM_VSCROLL = 277;
        private const int SB_PAGEBOTTOM = 7;

        public static void ScrollToBottom(RichTextBox MyRichTextBox)
        {
            SendMessage(MyRichTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
        }

        private static string eqgameID;
        private static string codeFile;
        private string[] arguments;
        Mem MemLib = new Mem();
        AutoItX3Lib.AutoItX3 aix3c = new AutoItX3Lib.AutoItX3();
        private bool stop = false;
        //bool rtry = false;

        static string lastCmd = "";

        protected override void WndProc(ref Message m) //hotbuttons
        {
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                if (id == 8)
                    cancelWorker();
            }
            base.WndProc(ref m);
        }

        void cancelWorker()
        {
            stop = true;
            AppendOutputText("All AutoBot work has been manually stopped by user!",Color.Red);
            timer1.Enabled = false;
            WriteLog("All AutoBot work has been manually stopped by user!");
            backgroundWorker1.CancelAsync();
        }

        string RemoveSpecialCharacters2(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        void AppendOutputText(string text, Color color = default(Color))
        {
            try
            {
                outputBox.SelectionStart = outputBox.TextLength;
                outputBox.SelectionLength = 0;

                outputBox.SelectionColor = color;
                outputBox.AppendText(DateTime.Now + " " + /*RemoveSpecialCharacters2(*/text/*)*/ + Environment.NewLine);
                ScrollToBottom(outputBox);
            }
            catch
            {
                WriteLog("[ERROR] AppendOutputText crashed!");
            }
        }

        public static void WriteLog(string dialog)
        {
            StreamWriter log;
            string logfile = "logfile.txt";

            if (!File.Exists(logfile))
                log = new StreamWriter(logfile);
            else
                log = File.AppendText(logfile);

            log.WriteLine(dialog);

            log.Close();
        }

        static private IntPtr progName(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, 8, 2, (int)'K');
            string[] args = Environment.GetCommandLineArgs();

            AppendOutputText("Welcome to the EQTrainer AutoBot program!", Color.Green);

            codeFile = args[2];
            if (args != null && args.Length > 0)
            {
                if (File.Exists(args[4]))
                {
                    arguments = args;
                    if (backgroundWorker1.IsBusy == false)
                    {
                        string newArgs = string.Join(",", arguments);
                        backgroundWorker1.RunWorkerAsync(newArgs);
                    }
                }
                else
                    MessageBox.Show("file doesn't exist");
            }
        }

        public void ParseReader(string line)
        {
            try
            {
                if (stop)
                    return;

                if (Regex.Match(line, "teleport", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    //AppendOutputText("teleporting to " + "X:" + words[1] + " Y:" + words[2] + " Z:" + words[3] + " H:" + words[4]);
                    try
                    {
                        //rtry = false;
                        Teleport(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]), float.Parse(words[4]));
                    }
                    catch
                    {
                        AppendOutputText("ERROR: Teleport crashed.");
                        WriteLog("[ERROR] Teleport Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "target", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    AppendOutputText("Targeting " + words);
                    try
                    {
                        TargetPlayer(words);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: TargetPlayer crashed.");
                        WriteLog("[ERROR] TargetPlayer Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "teleto", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    try
                    {
                        TeleportToPlayer(words);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: TeleportToPlayer crashed.");
                        WriteLog("[ERROR] TeleportToPlayer Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (string.Equals(line, "relog", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    AppendOutputText("Relogging your character. This will take 1 minute.");
                    RelogChar();
                }
                else if (Regex.Match(line, "checkzone", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    AppendOutputText("Checking if in zone " + words);
                    try
                    {
                        zoneCheck(words);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: checkzone crashed.");
                        WriteLog("[ERROR] checkzone Try/Catch return (" + DateTime.Now + ")");
                    }
                    //lastCmd = "checkzone " + words;
                }
                else if (Regex.Match(line, "pause", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("pausing for " + words[1] + " milliseconds");
                    try
                    {
                        int timer = Convert.ToInt32(words[1]);
                        System.Threading.Thread.Sleep(timer);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: pause crashed.");
                        WriteLog("[ERROR] pause Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (line.Contains("//"))
                {
                    //skip
                }
                else if (Regex.Match(line, "setfocus", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    try
                    {
                        setFocus();
                    }
                    catch
                    {
                        AppendOutputText("ERROR: GetFocus crashed.");
                        WriteLog("[ERROR] GetFocus Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "checkcursor", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    try
                    {
                        CheckCursor(words);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: CheckCursor crashed.");
                        WriteLog("[ERROR] checkcursor Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "checkgive", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    try
                    {
                        CheckGive(words);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: CheckGive crashed.");
                        WriteLog("[ERROR] CheckGive Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "talktoNPC", RegexOptions.IgnoreCase).Success == true)
                {
                    char[] spaceChar = " ".ToCharArray(0, 1);
                    var commands = line.Split(spaceChar, 3);
                    AppendOutputText("sending message " + commands[2] + " to " + commands[1]);
                    try
                    {
                        SayMessageNPC(commands[1], commands[2]);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: SayMessageNPC crashed.");
                        WriteLog("[ERROR] SayMessageNPC Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "say", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    char[] spaceChar = " ".ToCharArray(0, 1);
                    var commands = line.Split(spaceChar, 2);
                    AppendOutputText("sending message " + words[1]);
                    try
                    {
                        SayMessage(words[1]);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: SayMessage crashed.");
                        WriteLog("[ERROR] SayMessage Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "press", RegexOptions.IgnoreCase).Success == true)
                {
                    string btn = line.Remove(0, line.IndexOf(' ') + 1);
                    //words = words.Remove('"');
                    AppendOutputText("pressing button " + btn);
                    try
                    {
                        Press(btn);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: Press crashed.");
                        WriteLog("[ERROR] Press Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "CheckTargetDistance", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("checking NPC distance for " + words[1]);
                    CheckDistance(words[1], float.Parse(words[2]), float.Parse(words[3]), float.Parse(words[4]), Convert.ToInt32(words[5]));
                }
                else if (Regex.Match(line, "walkto", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("Walking to X:" + words[1] + " Y:" + words[2]);
                    WalkTo(float.Parse(words[1]), float.Parse(words[2]));
                }
                else if (Regex.Match(line, "CheckPCNearby", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("checking Player distance");
                    try
                    {
                        CheckPCNearby(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]), Convert.ToInt32(words[4]));
                    }
                    catch
                    {
                        AppendOutputText("ERROR: Press crashed.");
                        WriteLog("[ERROR] Press Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "healCheck", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    try
                    {
                        healCheck(Convert.ToInt32(words[1]), words[2]);
                    }
                    catch
                    {
                        AppendOutputText("ERROR: healCheck crashed.");
                        WriteLog("[ERROR] healCheck Try/Catch return (" + DateTime.Now + ")");
                    }
                }
                else if (Regex.Match(line, "mouse", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    if (words.Length != 4)
                        AppendOutputText("Warning: Mouse command argument count incorrect. given=" + (words.Length - 1).ToString() + " expected=3", Color.Red);
                    else
                    {
                        Mouse(Convert.ToInt32(words[1]), Convert.ToInt32(words[2]), words[3]);
                        AppendOutputText("moving mouse " + "X:" + words[1] + " Y:" + words[2] + " click:" + words[3]);
                        lastCmd = "mouse " + words[1] + " " + words[2] + " " + words[3];
                    }
                }
                else if (Regex.Match(line, "checkhealth", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("checking health conditions for " + words[1]);
                    //HealthCheck(words[1], Convert.ToInt32(words[2]));
                }
                else if (Regex.Match(line, "warpfollowtarget", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    FollowWarpTarget(words[1]);
                }
                else
                {
                    //skip
                }
            }
            catch
            {
                AppendOutputText("ERROR: ParseReader Try/Catch return. Game Crashed.", Color.Red);
                WriteLog("[ERROR] ParseReader Try/Catch return. Game Crashed. (" + DateTime.Now + ")");
                return;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string value = (string)e.Argument;
            string[] args = value.Split(',');

            // not always needed
            //string winTitle = Process.GetProcessById(Convert.ToInt32(eqgameID)).MainWindowTitle;
            //AppendOutputText("Checking if window " + winTitle + " is active...");

            using (StreamReader sr = new StreamReader(args[4]))
            {
                eqgameID = args[1];

                AppendOutputText("Opening process " + eqgameID);
                MemLib.OpenProcess(eqgameID);

                string[] lines = File.ReadAllLines(args[4]);
                while (true) //infinite loop
                {
                    try
                    {
                        /*if (aix3c.WinExists(winTitle) == 0)
                        {
                            AppendOutputText("No instance of EverQuest running.");
                            continue;
                        }*/

                        foreach (string line in lines)
                        {
                            if (backgroundWorker1.CancellationPending == true)
                            {
                                e.Cancel = true;
                                return;
                            }
                            
                            //if (line.Contains("mouse") || line.Contains("press") || line.Contains("move"))
                                //aix3c.WinWaitActive(winTitle);
                            ParseReader(line);
                        }
                        AppendOutputText("END OF SCRIPT");
                        if (args[3] != "loop")
                        {
                            AppendOutputText("Script was told not to loop.",Color.Blue);
                            return;
                        }
                    }
                    catch
                    {
                        AppendOutputText("ERROR: Main StreamReader loop stopped", Color.Red);
                        WriteLog("[ERROR] Main StreamReader loop stopped (" + DateTime.Now + ")");
                    }
                }
            }
        }

        public void setFocus()
        {
            IntPtr hWnd = IntPtr.Zero;
            //if (this.RefToForm1.listView2.Items.Count > 0 && this.RefToForm1.listView2.SelectedItems.Count == 0)
            //{
            //string procID = this.RefToForm1.listView2.SelectedItems[0].SubItems[1].Text;
            Process EQProc = Process.GetProcessById(Convert.ToInt32(eqgameID));
            SetForegroundWindow(EQProc.MainWindowHandle);
            //}
        }

        public void TargetPlayer(string name)
        {
            //lastCmd = "target " + name;
            int player_spawn_info = MemLib.readUIntPtr("spawnInfoAddress", codeFile);

            int spawn_info_address = player_spawn_info;
            int spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);
            spawn_info_address = spawn_next_spawn_info;

            for (int i = 0; i < 4096; i++) //we could make this a seperate function if we need to.
            {
                spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);

                if (spawn_next_spawn_info == 0x00000000)
                    break;

                string spawn_info_name = MemLib.readPString((UIntPtr)spawn_info_address, "spawnInfoName", codeFile);

                if (spawn_info_name.Contains(name) == true)
                {
                    //AppendOutputText("Found spawn name... " + "Name: " + spawn_info_name + " Address: " + spawn_info_address.ToString("X8")); //DEBUG
                    string address_text = spawn_info_address.ToString("X8"); //this is dumb, but I'm tired of the crashes.
                    int address_value = Convert.ToInt32(address_text, 16);
                    byte[] nameBuffer = BitConverter.GetBytes(address_value);
                    MemLib.writeUIntPtr("targetSpawn", codeFile, nameBuffer);
                    return;
                }

                //AppendOutputText("Checking spawn name... " + "Name: " + spawn_info_name + " Address: " + spawn_info_address.ToString("X8")); //DEBUG
                spawn_info_address = spawn_next_spawn_info;
            }
        }

        public void TeleportToPlayer(string name)
        {
            //lastCmd = "target " + name;
            int player_spawn_info = MemLib.readUIntPtr("spawnInfoAddress", codeFile);
            string spawn_info_name = null;
            float spawn_info_y, spawn_info_x, spawn_info_z, spawn_info_heading = 0;

            int spawn_info_address = player_spawn_info;
            int spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);
            spawn_info_address = spawn_next_spawn_info;

            for (int i = 0; i < 4096; i++) //we could make this a seperate function if we need to.
            {
                spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);

                if (spawn_next_spawn_info == 0x00000000)
                    break;

                spawn_info_name = MemLib.readPString((UIntPtr)spawn_info_address, "spawnInfoName", codeFile);

                if (spawn_info_name.Contains(name) == true)
                {
                    spawn_info_y = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoY", codeFile);
                    spawn_info_x = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoX", codeFile);
                    spawn_info_z = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoZ", codeFile);
                    spawn_info_heading = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoHeading", codeFile);
                    //AppendOutputText("Teleporting To: " + "Name: " + spawn_info_name + " Addr: " + spawn_info_address.ToString("X8") + " Y:" + spawn_info_y.ToString() + " X:" + spawn_info_x.ToString() + " H:" + spawn_info_heading.ToString()); //DEBUG
                    Teleport(spawn_info_y, spawn_info_x, spawn_info_z, spawn_info_heading);
                    return;
                }

                //AppendOutputText("Checking spawn name... " + "Name: " + spawn_info_name + " Address: " + spawn_info_address.ToString("X8")); //DEBUG
                spawn_info_address = spawn_next_spawn_info;
            }
        }

        public void FollowWarpTarget(string distance)
        {
            float t_z_address = MemLib.readFloat("targetZ", codeFile);
            float t_y_address = MemLib.readFloat("targetY", codeFile);
            float t_x_address = MemLib.readFloat("targetX", codeFile);
            float t_h_address = MemLib.readFloat("targetHeading", codeFile);

            if (t_z_address == 0 && t_y_address == 0 && t_x_address == 0 && t_h_address == 0)
            {
                AppendOutputText("ERROR: No target", Color.Red);
                return;
            }

            double angleInDegrees = t_h_address / 1.42;
            double cos = Math.Cos(angleInDegrees * (Math.PI / 180.0));
            double sin = Math.Sin(angleInDegrees * (Math.PI / 180.0));
            double reverse_x = t_x_address - Convert.ToInt32(distance) * cos;
            double reverse_y = t_y_address - Convert.ToInt32(distance) * sin;

            MemLib.writeMemory("playerZ", codeFile, "float", t_z_address.ToString());
            MemLib.writeMemory("playerY", codeFile, "float", reverse_y.ToString());
            MemLib.writeMemory("playerX", codeFile, "float", reverse_x.ToString());
            MemLib.writeMemory("playerHeading", codeFile, "float", t_h_address.ToString());
        }

        public void healCheck(int percent, string targets)
        {
            if (stop)
                return;

            System.Threading.Thread.Sleep(2000);

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
                //int index = spawn_info_name.IndexOf("0");
                // if (index > 0)
                //    spawn_info_name = spawn_info_name.Substring(0, index);
                int spawn_info_type = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoType", codeFile);
                spawn_info_type = (byte)spawn_info_type;

                //DEBUG
                /*if (spawn_info_type == (byte)0)
                    AppendOutputText("Player in zone. " + spawn_info_name );*/

                if (targets != null)
                {
                    string[] targetsNames = new string[20];
                    if (targets.Contains(','))
                        targetsNames = targets.Split(',');
                    else
                        targetsNames[0] = targets;

                    if (Array.IndexOf(targetsNames, spawn_info_name) >= 0)
                    {
                        string addressString = spawn_info_address.ToString("X8");

                        int address_value = Convert.ToInt32(addressString, 16);
                        byte[] buffer = BitConverter.GetBytes(address_value);
                        MemLib.writeUIntPtr("targetSpawn", codeFile, buffer);

                        int tHealth = MemLib.readByte("targetHealth", codeFile);

                        AppendOutputText("Targeting player " + spawn_info_name + "(" + addressString + ") T:" + tHealth.ToString() + " P:" + percent.ToString() + ".");

                        if (tHealth == 0)
                        {
                            AppendOutputText("ERROR: No target or is dead.", Color.Red);
                            return;
                        }

                        if (tHealth > percent)
                        {
                            AppendOutputText("Target health good, checking again... ", Color.Green);
                            if (Array.IndexOf(targetsNames, spawn_info_name) == 0)
                                healCheck(percent, targets);
                        }
                        else
                        {
                            AppendOutputText("Target health bad, doing something... ", Color.Red);
                            return;
                        }

                        spawn_info_address = spawn_next_spawn_info;
                        continue;
                    }
                }

                //Player spawn_info_type is 0.
                if (spawn_info_type >= (byte)1 )
                {
                    spawn_info_address = spawn_next_spawn_info;
                    continue;
                }

                spawn_info_address = spawn_next_spawn_info;
            }
        }

        public void CheckGive(string state)
        {
            //mac=0x007F9574 + 0x5f314 <-- no base?
            if (stop)
                return;

            int wndState = MemLib.readByte("giveWnd", codeFile);

            if ((state == "open" && wndState.Equals(1)) || (state == "closed" && wndState.Equals(0)))
            {
                AppendOutputText("Give window " + state + "!");
            }
            else
            {
                AppendOutputText("Give window is not " + state + "! Checking again in 200 milliseconds.");
                doLastCmd();
                System.Threading.Thread.Sleep(200);
                CheckGive(state);
            }
        }

        public void doLastCmd()
        {
            if (lastCmd.Contains("mouse") == true)
            {
                string[] words = lastCmd.Split(' ');
                Mouse(Convert.ToInt32(words[1]), Convert.ToInt32(words[2]), words[3]);
                AppendOutputText("[LASTCMD] mouse " + "X:" + words[1] + " Y:" + words[2] + " click:" + words[3]);
            }
            if (lastCmd.Contains("teleport") == true)
            {
                string[] words = lastCmd.Split(' ');
                Teleport(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]), float.Parse(words[4]));
                AppendOutputText("[LASTCMD] teleport " + "X:" + words[1] + " Y:" + words[2] + " z:" + words[3] + " h:" + words[4]);
            }
        }

        public void RelogChar()
        {
            AppendOutputText("Sitting and camping...");
            aix3c.Send("{ENTER}/sit{ENTER}");
            System.Threading.Thread.Sleep(1000);
            aix3c.Send("{ENTER}/camp{ENTER}");
            System.Threading.Thread.Sleep(50000);
            AppendOutputText("Pressing enter key...");
            aix3c.Send("{ENTER}");
            System.Threading.Thread.Sleep(20000);
            AppendOutputText("Assuming back in world...");
        }

        public void CheckCursor(string item)
        {
            //mac = 0x007F9510 + 0x40 <-- no base?
            if (stop)
                return;

            int cursor = new int();
            cursor = MemLib.readByte("cursorItem", codeFile);

            if (item.Equals("yes") && cursor.Equals(1))
            {
                AppendOutputText("Cursor item found! Continuing...");
                System.Threading.Thread.Sleep(500);
            }
            else if (item.Equals("no") && cursor.Equals(0))
            {
                AppendOutputText("Cursor item not found! Continuing...");
                System.Threading.Thread.Sleep(500);
            }
            else
            {
                /*r++;
                if (r >= 10 && item == "yes")
                {
                    AppendOutputText("Failed cursor check 10 times! Relogging character and trying last say command.");
                    r = 0;
                    RelogChar();
                    WriteLog("[ERROR] Cursor item not found 10 times, relogging... (" + DateTime.Now + ")");
                    if (lastCmd.Contains("talktoNPC") == true)
                    {
                        char[] spaceChar = " ".ToCharArray(0, 1);
                        var commands = lastCmd.Split(spaceChar, 3);
                        AppendOutputText("Re-sending message " + commands[2] + " to " + commands[1]);
                        SayMessageNPC(commands[1], commands[2]);
                        System.Threading.Thread.Sleep(1000);
                        CheckCursor(item);
                    }
                }
                else if (r >= 10 && item == "no")
                {
                    if (lastCmd.Contains("mouse") == true && r < 20)
                    {
                        string[] words = lastCmd.Split(' ');
                        Mouse(Convert.ToInt32(words[1]), Convert.ToInt32(words[2]), words[3]);
                        AppendOutputText("moving mouse " + "X:" + words[1] + " Y:" + words[2] + " click:" + words[3]);
                        CheckCursor(item);
                    }
                    else
                    {
                        AppendOutputText("Failed cursor check 10 times! Giving up...");
                        r = 0;
                        WriteLog("[ERROR] Cursor item check failed 10 times. Giving up... (" + DateTime.Now + ")");
                    }
                }*/
                doLastCmd();
                AppendOutputText("Checking cursor item again. Item:" + item + " Memory:" + cursor.ToString());
                System.Threading.Thread.Sleep(500);
                CheckCursor(item);
            }
        }

        public double truncateNum(double num)
        {
            return Math.Truncate(100 * num) / 100;
        }

        public void changeSafeXYZ(float value_x, float value_y, float value_z)
        {
            byte[] write_z = new byte[1];
            write_z = BitConverter.GetBytes(value_z);
            MemLib.writeUIntPtr("safeZ", codeFile, write_z);

            byte[] write_x = new byte[1];
            write_x = BitConverter.GetBytes(value_x);
            MemLib.writeUIntPtr("safeX", codeFile, write_x);

            byte[] write_y = new byte[1];
            write_y = BitConverter.GetBytes(value_y);
            MemLib.writeUIntPtr("safeY", codeFile, write_y);
        }

       public bool tpSafeCheck(float value_x, float value_y, float value_z){
           float readSafeZ = MemLib.readUintPtrFloat("safeZ", codeFile);
           float readSafeX = MemLib.readUintPtrFloat("safeX", codeFile);
           float readSafeY = MemLib.readUintPtrFloat("safeY", codeFile);

           AppendOutputText("Checking Safe X:" + value_x + " Y:" + value_y + " Z:" + value_z, Color.Green);

           if (readSafeY.Equals(value_y) && readSafeX.Equals(value_x) && readSafeZ.Equals(value_z) && tpPlayerCheck() == true)
               return true;
           else
           {
               AppendOutputText("Bad Safe X:" + readSafeX + " Y:" + readSafeY + " Z:" + readSafeZ, Color.Red);
               return false;
           }
       }

       public bool tpPlayerCheck(/*float value_x, float value_y, float value_z*/)
       {
           float y_address = MemLib.readFloat("playerY", codeFile);
           float x_address = MemLib.readFloat("playerX", codeFile);
           //float z_address = MemLib.readFloat("playerZ", codeFile);

           /*double value_y_rounded = Math.Round(Convert.ToDouble(value_y));
           double value_x_rounded = Math.Round(Convert.ToDouble(value_x));
           double value_z_rounded = Math.Round(Convert.ToDouble(value_z));

           double y_diff = Math.Abs(Math.Round((value_y_rounded - y_address)));
           double x_diff = Math.Abs(Math.Round((value_x_rounded - x_address)));
           double z_diff = Math.Abs(Math.Round((value_z_rounded - z_address)));

           AppendOutputText("Checking Player X:" + x_address + "[" + x_diff + "] Y:" + y_address + "[" + y_diff + "] Z:" + z_address + "[" + z_diff + "]", Color.Green);*/

           if (y_address.Equals(0) && x_address.Equals(0)) //zoning...
               return false;
           else
               return true;

           /*if ((x_diff > (double)10) || (y_diff > (double)10) || (z_diff > (double)2))
           {
               AppendOutputText("Bad Player X:" + x_address + " Y:" + y_address + " Z:" + z_address, Color.Red);
               return false;
           }
           else
               return true;*/
       }

        public void Teleport(float value_x, float value_y, float value_z, float value_h) //it's actually y,x,z
        {
            if (stop)
                return;

            lastCmd = "teleport " + value_x.ToString() + " " + value_y.ToString() + " " + value_z.ToString() + " " + value_h.ToString();
            changeSafeXYZ(value_x, value_y, value_z);

            AppendOutputText("Teleporting to X:" + value_x + " Y:" + value_y + " Z:" + value_z,Color.Green);

            if (tpSafeCheck(value_x, value_y, value_z))
            {
                MemLib.writeMemory("playerHeading", codeFile, "float", value_h.ToString());
                Thread ClientThread = new Thread(MemLib.ThreadStartClient);
                ClientThread.Start();
            }
            else
            {
                //System.Threading.Thread.Sleep(1000);
                Teleport(value_x, value_y, value_z, value_h);
            }

           System.Threading.Thread.Sleep(250);

           /*if (tpPlayerCheck(value_x, value_y, value_z) == false)
            {
                System.Threading.Thread.Sleep(1000);
                Teleport(value_x, value_y, value_z, value_h);
            }*/
        }

        static void CheckDistance(string target, float y, float x, float z, int dist)
        {
            // go through spawn list, look for name (target), check via distance. Same as CheckPCNearby.
            // if (spawn_info_name.Contains(target) == false) //find our NPC's name
        }

        public double Normalise(double degrees)
        {
            double retval = degrees % 512;
            if (retval < 0)
                retval += 512;
            return retval;
        }

        public void WalkTo(float x, float y)
        {
            float yDiff;
            float xDiff;

            float myY = MemLib.readFloat("playerY", codeFile);
            float myX = MemLib.readFloat("playerX", codeFile);
            float myH = MemLib.readFloat("playerHeading", codeFile);

            xDiff = x - myX;
            yDiff = y - myY;
            double NewH = Math.Atan2(System.Convert.ToDouble(yDiff), System.Convert.ToDouble(xDiff)) * (256 / Math.PI);
            NewH = Normalise(Convert.ToInt32(NewH));

            //MemLib.writeMemory("playerHeading", codeFile, "float", NewH.ToString());
            double adjustmentX = myX - x;
            adjustmentX = Math.Round(adjustmentX);
            double adjustmentY = myY - y;
            adjustmentY = Math.Round(adjustmentY);

            double adjustmentH = myH - NewH;
            adjustmentH = Math.Round(adjustmentH);

            //AppendOutputText("[WalkTo] DEBUG: adjustmentX=" + Math.Abs(adjustmentX).ToString() + " adjustmentY=" + Math.Abs(adjustmentY).ToString() + " adjustmentH=" + adjustmentH.ToString() + " myH=" + myH + " newh=" + NewH);

            if (adjustmentX < 4 && adjustmentX > -4 && adjustmentY < 4 && adjustmentY > -4)
            {
                aix3c.Send("{up UP}");
                aix3c.Send("{left UP}");
                aix3c.Send("{right UP}");
            }
            else if (adjustmentH > 2)
            {
                aix3c.Send("{left UP}");
                aix3c.Send("{right DOWN}");
                WalkTo(x, y);
            }
            else if (adjustmentH < -2)
            {
                aix3c.Send("{right UP}");
                aix3c.Send("{left DOWN}");
                WalkTo(x, y);
            }
            else
            {
                aix3c.Send("{left UP}");
                aix3c.Send("{right UP}");
                aix3c.Send("{up DOWN}");
                WalkTo(x, y);
            }
        }

        int zoneCheckTimer = 0;
        public void zoneCheck(string zone)
        {
            if (stop)
                return;

            try
            {
                //rtry = true; //dont retry teleport
                string curZone = MemLib.RemoveSpecialCharacters(MemLib.readUIntPtrStr("mapShortName", codeFile).ToString());
                //curZone = Regex.Replace(curZone, @"[^a-zA-Z0-9]*", string.Empty, /*RegexOptions.CultureInvariant |*/ RegexOptions.IgnoreCase);

                if (curZone == null || curZone == "" || zone == "")
                {
                    AppendOutputText("Zone Check failed. Null given.");
                    //System.Threading.Thread.Sleep(1000);
                    //zoneCheck(zone);
                }

                //double y_address = Math.Round(Convert.ToDouble(MemLib.readFloat("playerY", codeFile)));
                //double x_address = Math.Round(Convert.ToDouble(MemLib.readFloat("playerX", codeFile)));

                if (Equals(curZone, zone) == true /*&& !y_address.Equals(0) && !x_address.Equals(0)*/)
                {
                    AppendOutputText("Zone Check successful. Continuing...");
                    zoneCheckTimer = 0;
                    //lastCmd = "";
                    return;
                }
                else
                {
                    //AppendOutputText("Zone Check failed(2), trying again [" + curZone + "," + zone + "]");
                    System.Threading.Thread.Sleep(1000);
                    zoneCheckTimer++; //180 seconds, and we stop.
                    //AppendOutputText("[DEBUG] ZONE CHECK TICK: " + zoneCheckTimer.ToString(), Color.Blue); //DEBUG
                    if (zoneCheckTimer == 180)
                    {
                        AppendOutputText("ZONE CHECK FAILED!", Color.Red);
                        /*aix3c.Send("{ENTER}");
                        System.Threading.Thread.Sleep(10000);
                        aix3c.Send("{ENTER}");*/
                        return;
                    } 
                    else
                        zoneCheck(zone);
                }
            }
            catch (Exception e)
            {
                AppendOutputText(e + " zoneCheck exception caught.");
            }
        }

        public void CheckPCNearby(float y, float x, float z, int dist, string ignore = null)
        {
            try
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
                    //int index = spawn_info_name.IndexOf("0");
                   // if (index > 0)
                    //    spawn_info_name = spawn_info_name.Substring(0, index);

                    float spawn_info_y = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoY", codeFile);
                    float spawn_info_x = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoX", codeFile);
                    float spawn_info_z = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoZ", codeFile);
                    int spawn_info_type = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoType", codeFile);
                    spawn_info_type = (byte)spawn_info_type;

                    float difference = Math.Abs(spawn_info_x - x);
                    float difference2 = Math.Abs(spawn_info_y - y);
                    float difference3 = Math.Abs(spawn_info_z - z);

                    //DEBUG
                    /*if (spawn_info_type == (byte)0)
                        AppendOutputText("Player in zone. " + spawn_info_name );*/

                    if (ignore != null)
                    {
                        string[] ignoreNames = new string[20];
                        if (ignore.Contains(','))
                            ignoreNames = ignore.Split(',');
                        else
                            ignoreNames[0] = ignore;

                        if (Array.IndexOf(ignoreNames, spawn_info_name) >= 0)
                        {
                            AppendOutputText("Ignoring player " + spawn_info_name + ".");
                            spawn_info_address = spawn_next_spawn_info;
                            continue;
                        }
                    }

                    //Player spawn_info_type is 0. If it's above that, then we're safe.
                    if (spawn_info_type >= (byte)1 || difference3 >= (float)50)
                    {
                        spawn_info_address = spawn_next_spawn_info;
                        continue;
                    }

                    if (difference <= (float)dist && difference2 <= (float)dist && difference3 <= (float)30)
                    {
                        AppendOutputText("Player distance check failed! Differences Y[" + difference2.ToString("0.00") + "] X[" + difference.ToString("0.00") + "] Z[" + difference3.ToString("0.00") + "] [" + spawn_info_name + "]. Will recheck in 30 sec.");
                        //AppendOutputText("DEBUG: " + spawn_info_name + " Y[" + spawn_info_y + "] X[" + spawn_info_x + "] Z[" + spawn_info_z + "]");
                        System.Threading.Thread.Sleep(30000);
                        CheckPCNearby(y, x, z, dist, ignore);
                        break;
                    }

                    spawn_info_address = spawn_next_spawn_info;
                }
            }
            catch
            {
                AppendOutputText("ERROR: OVERFLOW!", Color.Red);
                System.Threading.Thread.Sleep(30000);
            }
        }

        public void Mouse(int x, int y, string click)
        {
            System.Threading.Thread.Sleep(200);
            lastCmd = "mouse " + x.ToString() + " " + y.ToString() + " " + click;
            try
            {
                if (codeFile.Contains("EQMac"))
                {
                    MemLib.writeUIntPtr("mousex", codeFile, BitConverter.GetBytes(x));
                    MemLib.writeUIntPtr("mousey", codeFile, BitConverter.GetBytes(y));
                    if (click == "left")
                        MemLib.writeUIntPtr("mouseClick", codeFile, BitConverter.GetBytes(128));
                    else if (click == "right")
                        MemLib.writeUIntPtr("mouseClick", codeFile, BitConverter.GetBytes(1677612));
                    else if (click == "hold")
                        MemLib.writeUIntPtr("mouseClick", codeFile, BitConverter.GetBytes(16777217));
                }
                else
                {
                    if (click == "left")
                        aix3c.MouseClick("left", x, y, 2, 0);
                    else if (click == "right")
                        aix3c.MouseClick("right", x, y, 2, 0);
                }
            }
            catch
            {
                AppendOutputText("ERROR: Mouse Try/Catch return", Color.Red);
                WriteLog("[ERROR] Mouse Try/Catch return (" + DateTime.Now + ")");
            }
        }

        public void SayMessage(string message)
        {
            if (message == null || message == "")
                return;
            //CheckWindowAcive();
            //lastCmd = "say " + message;
            //CheckWindowAcive();
            aix3c.Send
                
            ("{ENTER}" + message + "{ENTER}");
            //this was mac. Only opened chat and type in a message, couldnt send.
            //uint openMessage = (uint)0x79856C;
            //uint writeMessage = (uint)mem.ReadPointer(0x00809478) + 0x175FC; //re-write this later.
            //WriteInt(openMessage, 65792);
            //WriteString(writeMessage, message); //re-write this later.
            lastCmd = "say " + message;
        }

        public void SayMessageNPC(string recipient, string message)
        {
            if (recipient == null || recipient == "" || message == null || message == "")
                return;
            TeleportToPlayer(recipient);
            TargetPlayer(recipient);
            //CheckWindowAcive();
            System.Threading.Thread.Sleep(1000);
            SayMessage(message);
            AppendOutputText("NPC: " + recipient + " MESSAGE:" + message);
            lastCmd = "talktoNPC " + recipient + " " + message;
        }

        public void Press(string button)
        {
            //CheckWindowAcive();
            aix3c.Send("{" + button + "}");
            lastCmd = "Press " + button;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cancelWorker();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int curTime = Convert.ToInt32(timer.Text);
            timer.Text = (curTime + 1).ToString();
        }

    }
}
