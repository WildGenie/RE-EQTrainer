using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Memory;

namespace EQTrainer
{
    class main
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        public static Mem MemLib = new Mem();

        public static string charClass(int t_class)
        {
            string[] classes = new string[] { "", "Warrior", "Cleric", "Paladin", "Ranger", "Shadow Knight", "Druid", "Monk", "Bard", "Rogue", "Shaman", "Necromancer", "Wizard", "Magician", "Enchanter", "Beastlord", "Banker", "Warrior Trainer", "Cleric Trainer", "Paladin Trainer", "Ranger Trainer", "Shadow Knight Trainer", "Druid Trainer", "Monk Trainer", "Bard Trainer", "Rogue Trainer", "Shaman Trainer", "Necromancer Trainer", "Wizard Trainer", "Magician Trainer", "Enchanter Trainer", "Beastlord Trainer", "Merchant" };
            if (t_class > 0 && t_class < 33)
                return classes[t_class];
            else
                return "Unknown";
        }

        public static string RemoveSpecialCharactersTwo(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ')
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static void inject(string eqgameID, string dll)
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

        public static void Teleport(string codeFile, float value_x, float value_y, float value_z, float value_h) //it's actually y,x,z
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
                Thread ClientThread = new Thread(MemLib.ThreadStartClient);
                ClientThread.Start();
            }
            else
                Teleport(codeFile, value_x, value_y, value_z, value_h); //try again.
        }
    }
}
