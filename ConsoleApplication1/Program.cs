using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using WindowsInput;
using System.Windows.Automation;
using System.Collections.Generic;
using System.Text;

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
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //...

        private static void Main(string[] args)
        {
            String path = @"c:\temp\Password.txt";
            String oPass = File.ReadAllText(path);
            //String ProcWindow = "Logon - Winman Live";

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
                //ActivateApp("WinMan.exe");
                ActivateApp("Winman");
                //ActivateApp("Logon - Winman Live");
                InputSimulator.SimulateTextEntry(oPass);
                System.Threading.Thread.Sleep(100);
                InputSimulator.SimulateKeyUp(VirtualKeyCode.SHIFT);
            }
            else
            {
                //ActivateApp("WinMan.exe");
                ActivateApp("Winman");
                //ActivateApp("Logon - Winman Live");
                InputSimulator.SimulateTextEntry(oPass);
                System.Threading.Thread.Sleep(100);                
            }
            InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);           
        }

        static void ActivateApp(string processName)
        {
            //Process[] p = Process.GetProcessesByName(processName);

            ////AutomationElement element = AutomationElement.FromHandle(p[0].MainWindowHandle);
            ////if (element != null)
            ////{
            ////    SetForegroundWindow(p[0].MainWindowHandle);
            ////    SwitchToThisWindow(p[0].MainWindowHandle);
            //    //element.SetFocus();
            ////}

            ////// Activate the first application we find with this name
            //if (p.Count() > 0)
            //{
            //    //SetForegroundWindow(p[0].MainWindowHandle);
            //    //ShowWindow(p[0].MainWindowHandle, 9);
            //    SwitchToThisWindow(p[0].MainWindowHandle, true);
            //}

            List<IntPtr> AllWindowsPtrs = GetAllWindows();
            foreach (IntPtr ThisWindowPtr in AllWindowsPtrs)
            {
                if (GetTitle(ThisWindowPtr).Contains(processName) == true)
                {
                    SwitchToThisWindow(ThisWindowPtr, true);
                }
            }

        }

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        protected static extern bool EnumWindows(Win32Callback enumProc, IntPtr lParam);

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            List<IntPtr> pointers = GCHandle.FromIntPtr(pointer).Target as List<IntPtr>;
            pointers.Add(handle);
            return true;
        }

        private static List<IntPtr> GetAllWindows()
        {
            Win32Callback enumCallback = new Win32Callback(EnumWindow);
            List<IntPtr> AllWindowPtrs = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(AllWindowPtrs);
            try
            {
                EnumWindows(enumCallback, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return AllWindowPtrs;
        }

        [DllImport("User32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr windowHandle, StringBuilder stringBuilder, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", SetLastError = true)]
        internal static extern int GetWindowTextLength(IntPtr hwnd);
        private static string GetTitle(IntPtr handle)
        {
            int length = GetWindowTextLength(handle);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(handle, sb, sb.Capacity);
            return sb.ToString();
        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        //private static void SwitchToAllChromeWinows()
        //{
        //    List<IntPtr> AllWindowsPtrs = GetAllWindows();
        //    foreach (IntPtr ThisWindowPtr in AllWindowsPtrs)
        //    {
        //        if (GetTitle(ThisWindowPtr).Contains("Google Chrome") == true)
        //        {
        //            SwitchToThisWindow(ThisWindowPtr, true);
        //        }
        //    }
        //}

    }
}
