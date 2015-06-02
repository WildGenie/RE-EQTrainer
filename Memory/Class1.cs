﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace Memory
{
    public class Mem
    {
        #region DllImports
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            Int32 dwProcessId
            );

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            string lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            UIntPtr dwSize,
            uint dwFreeType
            );

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(
            IntPtr hModule,
            string procName
        );

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        private static extern bool _CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(
        IntPtr hObject
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(
            string lpModuleName
        );

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern Int32 WaitForSingleObject(
            IntPtr handle,
            Int32 milliseconds
        );

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
          IntPtr hProcess,
          IntPtr lpThreadAttributes,
          uint dwStackSize,
          UIntPtr lpStartAddress, // raw Pointer into remote process  
          IntPtr lpParameter,
          uint dwCreationFlags,
          out IntPtr lpThreadId
        );

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        // privileges
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;
        #endregion

        public static IntPtr pHandle;

        Process procs = null;

        public void OpenGameProcess(string eqgameID)
        {
            Int32 ProcID = Convert.ToInt32(eqgameID);
            procs = Process.GetProcessById(ProcID);
            //IntPtr hProcess = (IntPtr)OpenProcess(0x1F0FFF, 1, ProcID);

            if (procs.Responding == false)
                return;

            pHandle = OpenProcess(0x1F0FFF, 1, ProcID);
            mainModule = procs.MainModule;
            foreach (ProcessModule Module in procs.Modules)
            {
                if (Module.ModuleName.Contains("dpvs"))
                    dpvsModule = Module.BaseAddress;
                if (Module.ModuleName.Contains("DSETUP"))
                    dsetupModule = Module.BaseAddress;
            }
        }

        public void setFocus()
        {
            //int style = GetWindowLong(procs.MainWindowHandle, -16);
            //if ((style & 0x20000000) == 0x20000000) //minimized
            //    SendMessage(procs.Handle, 0x0112, (IntPtr)0xF120, IntPtr.Zero);
            SetForegroundWindow(procs.MainWindowHandle);
        }

        public string LoadCode(string name, string path)
        {
            StringBuilder returnCode = new StringBuilder(1024);
            uint read_ini_result = GetPrivateProfileString("codes", name, "", returnCode, (uint)path.Length, path);
            return returnCode.ToString();
        }

        public Int32 LoadIntCode(string name, string path)
        {
            int intValue = Convert.ToInt32(LoadCode(name, path), 16);
            if (intValue >= 0)
                return intValue;
            else
                return 0;
        }

        public void ThreadStartClient(object obj)
        {
            ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            using (NamedPipeClientStream pipeStream = new NamedPipeClientStream("EQTPipe"))
            {
                if (!pipeStream.IsConnected)
                    pipeStream.Connect();

                //MessageBox.Show("[Client] Pipe connection established");
                using (StreamWriter sw = new StreamWriter(pipeStream))
                {
                    if (sw.AutoFlush == false)
                        sw.AutoFlush = true;
                    sw.WriteLine("warp");
                }
            }
        }

        public UIntPtr LoadUIntPtrCode(string name, string path)
        {
            uint uintValue = Convert.ToUInt32(LoadCode(name, path), 16);
            if (uintValue >= 0)
                return (UIntPtr)uintValue;
            else
                return (UIntPtr)0;
        }

        public ProcessModule mainModule;
        public IntPtr dpvsModule;
        public IntPtr dsetupModule;

        public string CutString(string mystring)
        {
            char[] chArray = mystring.ToCharArray();
            string str = "";
            for (int i = 0; i < mystring.Length; i++)
            {
                if ((chArray[i] == ' ') && (chArray[i + 1] == ' '))
                {
                    return str;
                }
                if (chArray[i] == '\0')
                {
                    return str;
                }
                str = str + chArray[i].ToString();
            }
            return mystring.TrimEnd(new char[] { '0' });
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z'))
                    sb.Append(c);
                else
                    break;
            }
            return sb.ToString();
        }

        #region readMemory
        public float readFloat(string code, string file)
        {
            byte[] memory = new byte[4];

            UIntPtr theCode = getCode(code, file);
            if (!LoadCode(code, file).Contains(","))
                theCode = LoadUIntPtrCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
            {
                float address = BitConverter.ToSingle(memory, 0);
                return (float)Math.Round(address, 2);
            }
            else
                return 0;
        }

        public string readString(string code, string file)
        {
            byte[] memoryNormal = new byte[32];

            UIntPtr theCode = getCode(code, file);
            if (!LoadCode(code, file).Contains(","))
                theCode = LoadUIntPtrCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memoryNormal, (UIntPtr)32, IntPtr.Zero))
                return CutString(System.Text.Encoding.UTF8.GetString(memoryNormal));
            else
                return "";
        }

        public string readBigString(string code, string file)
        {
            byte[] memoryNormal = new byte[20];

            UIntPtr theCode = getCode(code, file);
            if (!LoadCode(code, file).Contains(","))
                theCode = LoadUIntPtrCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memoryNormal, (UIntPtr)20, IntPtr.Zero))
                return System.Text.Encoding.UTF8.GetString(memoryNormal);
            else
                return "";
        }

        public int readInt(string code, string file)
        {
            byte[] memory = new byte[4];
            UIntPtr theCode = getCode(code, file);
            if (!LoadCode(code, file).Contains(","))
                theCode = LoadUIntPtrCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        public int readIntMove(string code, string file, int moveQty)
        {
            byte[] memory = new byte[4];
            UIntPtr theCode = getCode(code, file);
            if (!LoadCode(code, file).Contains(","))
                theCode = LoadUIntPtrCode(code, file);

            theCode = theCode + moveQty;

            if (ReadProcessMemory(pHandle, theCode, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        public int readByte(string code, string file)
        {
            byte[] memoryTiny = new byte[4];

            UIntPtr theCode = getCode(code, file);
            if (!LoadCode(code, file).Contains(","))
                theCode = LoadUIntPtrCode(code, file);

            if (ReadProcessMemory(pHandle, theCode, memoryTiny, (UIntPtr)1, IntPtr.Zero))
                return BitConverter.ToInt32(memoryTiny, 0);
            else
                return 0;
        }

        public int readPByte(UIntPtr address, string code, string file)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, address + LoadIntCode(code, file), memory, (UIntPtr)1, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        public int readUInt(UIntPtr code)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, code, memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        public float readPFloat(UIntPtr address, string code, string file)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, address + LoadIntCode(code, file), memory, (UIntPtr)4, IntPtr.Zero))
            {
                float spawn = BitConverter.ToSingle(memory, 0);
                return (float)Math.Round(spawn, 2);
            }
            else
                return 0;
        }

        public int readPInt(UIntPtr address, string code, string file)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, address + LoadIntCode(code, file), memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        public string readPString(UIntPtr address, string code, string file)
        {
            byte[] memoryNormal = new byte[32];
            if (ReadProcessMemory(pHandle, address + LoadIntCode(code, file), memoryNormal, (UIntPtr)32, IntPtr.Zero))
                return CutString(System.Text.Encoding.ASCII.GetString(memoryNormal));
            else
                return "";
        }

        public float readUintPtrFloat(string code, string file)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, LoadUIntPtrCode(code, file), memory, (UIntPtr)4, IntPtr.Zero))
            {
                float spawn = BitConverter.ToSingle(memory, 0);
                return (float)Math.Round(spawn, 2);
            }
            else
                return 0;
        }

        public int readUIntPtr(string code, string file)
        {
            byte[] memory = new byte[4];
            if (ReadProcessMemory(pHandle, LoadUIntPtrCode(code, file), memory, (UIntPtr)4, IntPtr.Zero))
                return BitConverter.ToInt32(memory, 0);
            else
                return 0;
        }

        public string readUIntPtrStr(string code, string file)
        {
            byte[] memoryNormal = new byte[32];
            if (ReadProcessMemory(pHandle, LoadUIntPtrCode(code, file), memoryNormal, (UIntPtr)32, IntPtr.Zero))
                return System.Text.Encoding.ASCII.GetString(memoryNormal);
            else
                return "";
        }
        #endregion

        #region writeMemory
        public void writeMemory(string code, string file, string type, string write)
        {
            byte[] memory = new byte[4];
            if (type == "float")
                memory = BitConverter.GetBytes(Convert.ToSingle(write));
            if (type == "bytes")
                memory = BitConverter.GetBytes(Convert.ToInt32(write));
            WriteProcessMemory(pHandle, getCode(code, file), memory, (UIntPtr)4, IntPtr.Zero);
        }

        public void writeUIntPtr(string code, string file, byte[] write)
        {
            WriteProcessMemory(pHandle, LoadUIntPtrCode(code, file), write, (UIntPtr)write.Length, IntPtr.Zero);
        }

        public void writeByte(UIntPtr code, byte[] write, int size)
        {
            WriteProcessMemory(pHandle, code, write, (UIntPtr)size, IntPtr.Zero);
        }
        #endregion

        public UIntPtr getCode(string name, string path)
        {
            string theCode = LoadCode(name, path);
            if (theCode == "")
                return UIntPtr.Zero;
            bool main = false;
            bool dpvs = false;
            bool dsetup = false;
            string newOffsets = theCode;
            if (theCode.Contains("base") || theCode.Contains("dpvs") || theCode.Contains("dsetup"))
                newOffsets = theCode.Substring(theCode.IndexOf('+') + 1);
            if (theCode.Contains("base") )
                main = true;
            else if (theCode.Contains("dpvs"))
                dpvs = true;
            else if (theCode.Contains("dsetup"))
                dsetup = true;

            List<int> offsetsList = new List<int>();
            List<string> testlist = new List<string>();

            string[] newerOffsets = newOffsets.Split(',');
            foreach (string oldOffsets in newerOffsets)
            {
                offsetsList.Add(Convert.ToInt32(oldOffsets, 16));
            }
            int[] offsets = offsetsList.ToArray();

            byte[] memoryAddress = new byte[4];
            if (main == true)
                ReadProcessMemory(pHandle, (UIntPtr)((int)mainModule.BaseAddress + offsets[0]), memoryAddress, (UIntPtr)4, IntPtr.Zero);
            else if (dpvs == true)
                ReadProcessMemory(pHandle, (UIntPtr)((int)dpvsModule + offsets[0]), memoryAddress, (UIntPtr)4, IntPtr.Zero);
            else if (dsetup == true)
                ReadProcessMemory(pHandle, (UIntPtr)((int)dsetupModule + offsets[0]), memoryAddress, (UIntPtr)4, IntPtr.Zero);
            else
                ReadProcessMemory(pHandle, (UIntPtr)(offsets[0]), memoryAddress, (UIntPtr)4, IntPtr.Zero);

            uint num1 = BitConverter.ToUInt32(memoryAddress, 0);

            //if (offsetsList.Count < 1)
            //    return (UIntPtr)Convert.ToUInt32(theCode);

            UIntPtr base1 = (UIntPtr)0;

            for (int i = 1; i < offsets.Length; i++)
            {
                base1 = new UIntPtr(num1 + Convert.ToUInt32(offsets[i]));
                ReadProcessMemory(pHandle, base1, memoryAddress, (UIntPtr)4, IntPtr.Zero);
                num1 = BitConverter.ToUInt32(memoryAddress, 0);
            }
            return base1;
        }

        public void closeProcess()
        {
            CloseHandle(pHandle);
        }

        public void InjectDLL(String strDLLName)
        {
            IntPtr bytesout;

            foreach (ProcessModule pm in procs.Modules)
            {
                if (pm.ModuleName.StartsWith("inject", StringComparison.InvariantCultureIgnoreCase))
                    return;
            }

            if (procs.Responding == false)
                return;

            Int32 LenWrite = strDLLName.Length + 1;
            IntPtr AllocMem = (IntPtr)VirtualAllocEx(pHandle, (IntPtr)null, (uint)LenWrite, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

            WriteProcessMemory(pHandle, AllocMem, strDLLName, (UIntPtr)LenWrite, out bytesout);
            UIntPtr Injector = (UIntPtr)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (Injector == null)
                return;

            IntPtr hThread = (IntPtr)CreateRemoteThread(pHandle, (IntPtr)null, 0, Injector, AllocMem, 0, out bytesout);
            if (hThread == null)
                return;

            int Result = WaitForSingleObject(hThread, 10 * 1000);
            if (Result == 0x00000080L || Result == 0x00000102L)
            {
                if (hThread != null)
                    CloseHandle(hThread);
                return;
            }
            VirtualFreeEx(pHandle, AllocMem, (UIntPtr)0, 0x8000);

            if (hThread != null)
                CloseHandle(hThread);

            return;
        }
    }
}
