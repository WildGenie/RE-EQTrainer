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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using Memory;
using AutoItX3Lib;

namespace WindowsFormsApplication1
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
        #endregion
        private static string eqgameID;
        private static string codeFile;
        private int r = 0;
        Mem MemLib = new Mem();
        AutoItX3Lib.AutoItX3 aix3c = new AutoItX3Lib.AutoItX3();
        string winTitle = "EverQuest";

        static string lastCmd = "";

        void AppendOutputText(string text, Color color = default(Color))
        {
            int start = outputBox.TextLength;
            outputBox.AppendText(text);
            int end = outputBox.TextLength;

            outputBox.Select(start, end - start);
            {
                outputBox.SelectionColor = color;
            }
            outputBox.SelectionLength = 0;
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

        public void ParseReader(string line)
        {
            AppendOutputText("Parse Reader function is starting up...", Color.Green);
            SetWindowText(progName("WinEQ"), winTitle);

            try
            {
                MemLib.OpenProcess(eqgameID);
                if (Regex.Match(line, "teleport", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    //AppendOutputText("teleporting to " + "X:" + words[1] + " Y:" + words[2] + " Z:" + words[3] + " H:" + words[4]);
                    Teleport(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]), float.Parse(words[4]));
                }
                else if (Regex.Match(line, "target", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    AppendOutputText("Targeting " + words);
                    TargetPlayer(words);
                }
                else if (Regex.Match(line, "teleto", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    TeleportToPlayer(words);
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
                    zoneCheck(words);
                }
                else if (Regex.Match(line, "pause", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("pausing for " + words[1] + " milliseconds");
                    int timer = Convert.ToInt32(words[1]);
                    System.Threading.Thread.Sleep(timer);
                }
                else if (line.Contains("//"))
                {
                    //skip
                }
                else if (Regex.Match(line, "checkcursor", RegexOptions.IgnoreCase).Success == true)
                {
                    string words = line.Remove(0, line.IndexOf(' ') + 1);
                    CheckCursor(words);
                }
                else if (Regex.Match(line, "checkgive", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    CheckGive(words[1]);
                }
                else if (Regex.Match(line, "talktoNPC", RegexOptions.IgnoreCase).Success == true)
                {
                    char[] spaceChar = " ".ToCharArray(0, 1);
                    var commands = line.Split(spaceChar, 3);
                    AppendOutputText("sending message " + commands[2] + " to " + commands[1]);
                    SayMessageNPC(commands[1], commands[2]);
                }
                else if (Regex.Match(line, "say", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    char[] spaceChar = " ".ToCharArray(0, 1);
                    var commands = line.Split(spaceChar, 2);
                    AppendOutputText("sending message " + words[1]);
                    SayMessage(words[1]);
                }
                else if (Regex.Match(line, "press", RegexOptions.IgnoreCase).Success == true)
                {
                    string btn = line.Remove(0, line.IndexOf(' ') + 1);
                    //words = words.Remove('"');
                    AppendOutputText("pressing button " + btn);
                    Press(btn);
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
                    WalkTo(float.Parse(words[1]), float.Parse(words[2]));
                }
                else if (Regex.Match(line, "CheckPCNearby", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("checking Player distance");
                    CheckPCNearby(float.Parse(words[1]), float.Parse(words[2]), float.Parse(words[3]), Convert.ToInt32(words[4]));
                }
                else if (Regex.Match(line, "mouse", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    Mouse(Convert.ToInt32(words[1]), Convert.ToInt32(words[2]), words[3]);
                    AppendOutputText("moving mouse " + "X:" + words[1] + " Y:" + words[2] + " click:" + words[3]);
                }
                else if (Regex.Match(line, "checkhealth", RegexOptions.IgnoreCase).Success == true)
                {
                    string[] words = line.Split(' ');
                    AppendOutputText("checking health conditions for " + words[1]);
                    HealthCheck(words[1], Convert.ToInt32(words[2]));
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
                System.Threading.Thread.Sleep(60000);
                Environment.Exit(0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AppendOutputText("Welcome to the EQTrainer AutoBot program!", Color.Green);
            string[] args = Environment.GetCommandLineArgs();

            codeFile = args[1];
            bool runonce = true;
            if (args != null && args.Length > 0)
            {
                if (File.Exists(args[3]))
                {
                    using (StreamReader sr = new StreamReader(args[3]))
                    {
                        eqgameID = args[0];
                        string[] lines = File.ReadAllLines(args[3]);
                        while (args[2] == "loop" || runonce == true)
                        {
                            try
                            {
                                foreach (string line in lines)
                                {
                                    ParseReader(line);
                                    runonce = false;
                                }
                                if (runonce == false)
                                {
                                    AppendOutputText("- end of script -");
                                    System.Threading.Thread.Sleep(100);
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
            }
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

        static void HealthCheck(string name, int percent)
        {
            //To Do
        }

        static void FollowTarget(string name)
        {
            //To Do
        }

        public void CheckGive(string state)
        {
            //mac=0x007F9574 + 0x5f314 <-- no base?
            int wndState = MemLib.readByte("giveWnd", codeFile);

            if ((state == "open" && wndState.Equals(1)) || (state == "closed" && wndState.Equals(0)))
            {
                AppendOutputText("Give window " + state + "!");
            }
            else
            {
                System.Threading.Thread.Sleep(500);
                AppendOutputText("Give window is not " + state + "! Checking again in 200 milliseconds.");
                //if (lastCmd != "")
                if (lastCmd.Contains("mouse") == true)
                {
                    string[] words = lastCmd.Split(' ');
                    Mouse(Convert.ToInt32(words[1]), Convert.ToInt32(words[2]), words[3]);
                    AppendOutputText("moving mouse " + "X:" + words[1] + " Y:" + words[2] + " click:" + words[3]);
                }
                else
                    AppendOutputText("lastCmd doesnt contain mouse. lastCmd:" + lastCmd);
                System.Threading.Thread.Sleep(500);
                CheckGive(state);
            }
        }

        public void RelogChar()
        {
            aix3c.WinWaitActive(winTitle);
            AppendOutputText("Sitting and camping...");
            aix3c.Send("{ENTER}/sit{ENTER}");
            System.Threading.Thread.Sleep(1000);
            aix3c.Send("{ENTER}/camp{ENTER}");
            System.Threading.Thread.Sleep(50000);
            AppendOutputText("Pressing enter key...");
            aix3c.Send("{ENTER}");
            System.Threading.Thread.Sleep(20000);
            AppendOutputText("Assuming back in world...");
            //MemLib.OpenProcess(eqgameID); //might be closed?
            SetWindowText(progName("WinEQ"), winTitle);
        }

        public void CheckCursor(string item)
        {
            //mac = 0x007F9510 + 0x40 <-- no base?
            int cursor = new int();
            cursor = MemLib.readByte("cursorItem", codeFile);

            if (item.Equals("yes") && cursor.Equals(1))
            {
                AppendOutputText("Cursor item found! Continuing...");
                System.Threading.Thread.Sleep(250);
                return;
            }
            else if (item.Equals("no") && cursor.Equals(0))
            {
                AppendOutputText("Cursor item not found! Continuing...");
                System.Threading.Thread.Sleep(250);
                return;
            }
            else
            {
                r++;
                if (r >= 10 && item == "yes")
                {
                    AppendOutputText("Failed cursor check 10 times! Relogging character and trying last say command.");
                    r = 0;
                    RelogChar();
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
                    AppendOutputText("Failed cursor check 10 times! Giving up...");
                    return;
                }
                else
                {
                    AppendOutputText("Checking cursor item again. Item:" + item + " Memory:" + cursor.ToString());
                    System.Threading.Thread.Sleep(1000);
                    CheckCursor(item);
                }
            }
        }

        public void injectdll2(float write_h)
        {
            System.Threading.Thread.Sleep(50);

            Int32 ProcID = Convert.ToInt32(eqgameID);
            Process procs = Process.GetProcessById(ProcID);
            IntPtr hProcess = (IntPtr)OpenProcess(0x1F0FFF, true, ProcID);

            try
            {
                String strDLLName = Environment.CurrentDirectory + "\\injectdll2.dll";
                MemLib.InjectDLL(hProcess, strDLLName);
            }
            catch
            {
                AppendOutputText("Injection failed! Program needs administration privileges!");
                WriteLog("Injection failed! Program needs administration privileges! (" + DateTime.Now + ")");
            }
            MemLib.writeMemory("playerHeading", codeFile, "float", write_h.ToString());
        }

        public void Teleport(float value_x, float value_y, float value_z, float value_h, Boolean retry = false) //it's actually y,x,z
        {
            //lastCmd = "teleport " + value_x.ToString() + " " + value_y.ToString() + " " + value_z.ToString() + " " + value_h.ToString();
            if (retry)
                return;

            byte[] write_y = BitConverter.GetBytes(value_y);
            byte[] write_x = BitConverter.GetBytes(value_x);
            byte[] write_z = BitConverter.GetBytes(value_z);

            MemLib.writeUIntPtr("safeY", codeFile, write_y);
            MemLib.writeUIntPtr("safeX", codeFile, write_x);
            MemLib.writeUIntPtr("safeZ", codeFile, write_z);

            System.Threading.Thread.Sleep(50);

            float readSafeZ = MemLib.readUintPtrFloat("safeZ", codeFile);
            float readSafeX = MemLib.readUintPtrFloat("safeX", codeFile);
            float readSafeY = MemLib.readUintPtrFloat("safeY", codeFile);

            AppendOutputText("Teleporting to X:" + value_x + " Y:" + value_y + " Z:" + value_z);

            if (readSafeY.Equals(value_y) && readSafeX.Equals(value_x) && readSafeZ.Equals(value_z))
                injectdll2(value_h);
            else
                Teleport(value_x, value_y, value_z, value_h);

            System.Threading.Thread.Sleep(50);

            float y_address = MemLib.readFloat("playerY", codeFile);
            float x_address = MemLib.readFloat("playerX", codeFile);
            float z_address = MemLib.readFloat("playerZ", codeFile);

            if (!y_address.Equals(value_y) || !x_address.Equals(value_x) || !(z_address > (value_z - 2) && z_address < (value_z + 2)))
            {
                AppendOutputText("Teleport failed. Trying again.");
                Teleport(value_x, value_y, value_z, value_h, true); //try again, one more time.
            }
            return;
        }

        static void CheckDistance(string target, float y, float x, float z, int dist)
        {
            // go through spawn list, look for name (target), check via distance. Same as CheckPCNearby.
            // if (spawn_info_name.Contains(target) == false) //find our NPC's name
        }

        public int Normalise(int degrees)
        {
            int retval = degrees % 360;
            if (retval < 0)
                retval += 360;
            return retval;
        }

        public void WalkTo(float x, float y)
        {
            aix3c.WinWaitActive(winTitle);
            float yDiff;
            float xDiff;
            float myY = MemLib.readFloat("playerY", codeFile);
            float myX = MemLib.readFloat("playerX", codeFile);
            float myH = MemLib.readFloat("playerHeading", codeFile);

            double myAdj = myH / 1.42;
            xDiff = x - myX;
            yDiff = y - myY;
            double NewH = Math.Atan2(yDiff, xDiff) * (180 / Math.PI);

            MemLib.writeMemory("playerHeading", codeFile, "float", NewH.ToString());

            double adjustmentX = myX - x;
            adjustmentX = Math.Round(adjustmentX);
            double adjustmentY = myY - y;
            adjustmentY = Math.Round(adjustmentY);

            //there needs to be more math here. Guessing at the numbers is not a good way of doing this.
            /*double adjustmentH = myAdj - Normalise(Convert.ToInt32(NewH));
            adjustmentH = Math.Round(adjustmentH);
            AppendOutputText("[WalkTo] DEBUG: adjustmentX=" + Math.Abs(adjustmentX).ToString() + " adjustmentY=" + Math.Abs(adjustmentY).ToString() + " adjustmentH=" + adjustmentH.ToString() + " myadj=" + myAdj + " newh=" + Normalise(Convert.ToInt32(NewH)));
            */
            if (Math.Abs(adjustmentX) != 0 && Math.Abs(adjustmentY) != 0)
            {
                aix3c.Send("{up DOWN}");
                WalkTo(x, y);
            }
            else
            {
                aix3c.Send("{up UP}");
                return;
            }
            /*
            if (Math.Abs(adjustmentH) <= 5)
            {
                //AppendOutputText("Adjustment Finished");
                aix3c.Send("{left UP}");
                aix3c.Send("{right UP}");
                AppendOutputText("finished adjusting");
                if (Math.Abs(adjustmentX) >= 5 || Math.Abs(adjustmentY) >= 5)
                {
                    AppendOutputText("moving forward...");
                    aix3c.Send("{up DOWN}");
                    int adjustment = 40;
                    if (Math.Abs(adjustmentX) <= adjustment && Math.Abs(adjustmentY) <= adjustment)
                    {
                        System.Threading.Thread.Sleep(20);
                        aix3c.Send("{up UP}");
                    }
                    WalkTo(x, y);
                }
                else
                {
                    aix3c.Send("{up UP}");
                    aix3c.Send("{left UP}");
                    aix3c.Send("{right UP}");
                    return;
                }
            }
            else if (adjustmentH > 0)
            {
                AppendOutputText("moving right");
                //aix3c.Send("{left UP}");
                aix3c.Send("{right DOWN}");
                if (Math.Abs(adjustmentH) < 30)
                {
                    if (Math.Abs(adjustmentX) < 50 || Math.Abs(adjustmentY) < 50)
                        System.Threading.Thread.Sleep(30); //faster
                    aix3c.Send("{right UP}");
                }
                WalkTo(x, y);
            }
            else if (adjustmentH < 0) //negative
            {
                AppendOutputText("moving left");
                //aix3c.Send("{right UP}");
                aix3c.Send("{left DOWN}");
                if (Math.Abs(adjustmentH) < 20)
                {
                    if (Math.Abs(adjustmentX) < 50 || Math.Abs(adjustmentY) < 50)
                        System.Threading.Thread.Sleep(30); //faster
                    aix3c.Send("{left UP}");
                }
                WalkTo(x, y);
            }*/
        }


        public void zoneCheck(string zone)
        {
            try
            {
                string curZone = MemLib.readUIntPtrStr("mapShortName", codeFile);
                curZone = Regex.Replace(curZone, @"[^a-zA-Z0-9]*", string.Empty, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

                if (curZone == null || curZone == "" || zone == "")
                {
                    //AppendOutputText("Zone Check failed. Checking again in 1 second. [" + curZone + "," + zone + "]");
                    System.Threading.Thread.Sleep(1000);
                    zoneCheck(zone);
                }

                if (Equals(curZone, zone) == true)
                {
                    AppendOutputText("Zone Check successful. Continuing...");
                    System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    //AppendOutputText("Zone Check failed. Checking again in 1 second. [" + curZone + "," + zone + "]");
                    System.Threading.Thread.Sleep(1000);
                    zoneCheck(zone);
                }
            }
            catch (Exception e)
            {
                AppendOutputText(e + " zoneCheck exception caught.");
            }
        }

        public void CheckPCNearby(float y, float x, float z, int dist)
        {
            try
            {
                //lastCmd = "CheckPCNearby " + y.ToString() + " " + x.ToString() + " " + z.ToString() + " " + dist.ToString();
                byte[] buffer = new byte[4];
                int player_spawn_info = MemLib.readUIntPtr("spawnInfoAddress", codeFile);

                int spawn_info_address = player_spawn_info;
                int spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);

                spawn_info_address = spawn_next_spawn_info;

                for (int i = 0; i < 4096; i++) //we could make this a seperate function if we need to.
                {
                    spawn_next_spawn_info = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoNext", codeFile);

                    if (spawn_next_spawn_info == 0x00000000)
                        continue;

                    float spawn_info_y = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoY", codeFile);
                    float spawn_info_x = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoX", codeFile);
                    float spawn_info_z = MemLib.readPFloat((UIntPtr)spawn_info_address, "spawnInfoZ", codeFile);
                    int spawn_info_type = MemLib.readPInt((UIntPtr)spawn_info_address, "spawnInfoType", codeFile);
                    spawn_info_type = (byte)spawn_info_type;

                    float difference = Math.Abs(spawn_info_x - x);
                    float difference2 = Math.Abs(spawn_info_y - y);
                    float difference3 = Math.Abs(spawn_info_z - z);

                    if (spawn_info_type >= (byte)1 || difference3 >= (float)15) //Player spawn_info_type is 0. If it's above that, then we're safe.
                    {
                        spawn_info_address = spawn_next_spawn_info;
                        continue;
                    }

                    if (difference <= (float)dist && difference2 <= (float)dist)
                    {
                        AppendOutputText("Player distance check failed! Differences Y[" + difference2.ToString("0.00") + "] X[" + difference.ToString("0.00") + "] Z[" + difference3.ToString("0.00") + "]. Will recheck after 30 seconds.");
                        //AppendOutputText("DEBUG: " + spawn_info_name + " Y[" + spawn_info_y + "] X[" + spawn_info_x + "] Z[" + spawn_info_z + "]");
                        System.Threading.Thread.Sleep(30000);
                        continue;
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
            aix3c.WinWaitActive(winTitle);
            if (click == "left")
                aix3c.MouseClick("left", x, y, 3);
            else if (click == "right")
                aix3c.MouseClick("right", x, y, 3);
            //mouse coordinates/clicks not writable in titanium
            /*try
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
            catch
            {
                AppendOutputText("ERROR: Mouse Try/Catch return", Color.Red);
                WriteLog("[ERROR] Mouse Try/Catch return (" + DateTime.Now + ")");
                return;
            }*/
        }

        public void SayMessage(string message)
        {
            if (message == null || message == "")
                return;
            SetWindowText(progName("WinEQ"), winTitle);
            //lastCmd = "say " + message;
            aix3c.WinWaitActive(winTitle);
            aix3c.Send("{ENTER}" + message + "{ENTER}");
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
            SetWindowText(progName("WinEQ"), winTitle);
            TeleportToPlayer(recipient);
            TargetPlayer(recipient);
            aix3c.WinWaitActive(winTitle);
            aix3c.Send("{ENTER}" + message + "{ENTER}");
            AppendOutputText("NPC: " + recipient + " MESSAGE:" + message);
            lastCmd = "talktoNPC " + recipient + " " + message;
        }

        public void Press(string button)
        {
            aix3c.WinWaitActive(winTitle);
            aix3c.Send("{" + button + "}");
        }

    }
}
