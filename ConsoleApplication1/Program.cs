using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace WinMan_Loader
{
    class Program
    {
        // import the function in your class
        //[DllImport("User32.dll")]
        //static extern int SetForegroundWindow(IntPtr point);
        [DllImport("User32.dll", SetLastError = true)]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd);

        //...
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
        UIntPtr dwExtraInfo);


        private static void Main()
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
                const int KEYEVENTF_EXTENDEDKEY = 0x1;
                const int KEYEVENTF_KEYUP = 0x2;
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,(UIntPtr)0);
                ActivateApp("winman.exe");
                ActivateApp("Logon - Winman Live");                
                SendKeys.SendWait(oPass);
                SendKeys.SendWait("{ENTER}");
                //DateTime Tthen = DateTime.Now;
                //do
                //{
                //    Application.DoEvents();
                //} while (Tthen.AddSeconds(1) > DateTime.Now);
                //keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)1);
                //keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)1);
            }
            else
            {
                ActivateApp("winman.exe");
                ActivateApp("Logon - Winman Live");
                SendKeys.SendWait(oPass);
                SendKeys.SendWait("{ENTER}");
            }    
            
           
        }

        static void ActivateApp(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);

            // Activate the first application we find with this name
            if (p.Count() > 0)
                SetForegroundWindow(p[0].MainWindowHandle);
        }

    }
}
