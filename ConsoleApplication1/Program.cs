using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;
using WindowsInput;

namespace WinMan_Loader
{
    class Program
    {
        // import the function in your class
        //[DllImport("User32.dll")]
        //static extern int SetForegroundWindow(IntPtr point);
        public const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //...

        private static void Main(string[] args)
        {
            String path = @"c:\temp\Password.txt";
            String oPass = File.ReadAllText(path);

            Process p = Process.Start(@"\\VS02\WinManV7\Live\WinMan.exe");           
            p.WaitForInputIdle();
            IntPtr h = p.MainWindowHandle;
            while (p.MainWindowHandle == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(100);
            }
                      
            if (Control.IsKeyLocked(Keys.CapsLock)) // Checks Capslock is on
            {                
                InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);                
                ActivateApp("WinMan.exe *32");
                ActivateApp("WinMan");
                ActivateApp("Logon - Winman Live");
                //ActivateApp("Logon - Winman Live");                
                InputSimulator.SimulateTextEntry(oPass);
                System.Threading.Thread.Sleep(100);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
            }
            else
            {
                ActivateApp("WinMan.exe *32");
                ActivateApp("WinMan");
                ActivateApp("Logon - Winman Live");
                //ActivateApp("Logon - Winman Live");                
                InputSimulator.SimulateTextEntry(oPass);
                System.Threading.Thread.Sleep(100);                
            }
            InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);           
        }

        static void ActivateApp(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);

            // Activate the first application we find with this name
            if (p.Count() > 0)
            {                  
                SetForegroundWindow(p[0].MainWindowHandle);
                ShowWindow(p[0].MainWindowHandle, 9);
            }
        }


    }
}
