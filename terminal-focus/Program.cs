using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace terminal_focus
{
    class Program
    {
        // FindWindow
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        // SetForegroundWindow
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private static void sendKeys(string windowName, string keys)
        {
            if (SpinWait.SpinUntil(() => FindWindow(null, windowName) != IntPtr.Zero, TimeSpan.FromSeconds(30)))
            {
                IntPtr hwnd = FindWindow(null, windowName);
                SetForegroundWindow(hwnd);
                SendKeys.SendWait(keys);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Process p = new Process();
                p.StartInfo.FileName = args[0];

                switch (args[0])
                {
                    case "wt.exe":
                        p.StartInfo.Arguments = "-p \"" + args[1] + "\"";
                        p.Start();
                        sendKeys(args[1], "+{F11}");
                        break;
                }
            }
        }
    }
}
