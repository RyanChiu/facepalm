﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using System.CodeDom;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;

namespace GetLocked
{
    public partial class FormAMB : Form
    {
        WinEventDelegate deleFG = null;
        WinEventDelegate deleSH = null;
        public FormAMB()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.listBoxShowing.MouseDoubleClick += new MouseEventHandler(listBoxShowing_MouseDoubleClick);

            deleFG = new WinEventDelegate(WinEventProc);
            deleSH = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, 
                IntPtr.Zero, deleFG, 0, 0, WINEVENT_OUTOFCONTEXT);
            _ = SetWinEventHook(EVENT_OBJECT_CLOAKED, EVENT_OBJECT_CLOAKED,
                IntPtr.Zero, deleSH, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, 
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, 
            WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint EVENT_OBJECT_CLOAKED = 0x8017;
        private const int GWL_STYLE = -16;
        private const uint WS_VISIBLE = 0x10000000;

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        private string GetWindowTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(hWnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, 
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // Write every "activated window"'s title on the console line.
            String title = GetWindowTitle(hwnd);
            Console.WriteLine(title + ", and the 'eventType is valued: " + eventType.ToString());
            if (eventType == EVENT_SYSTEM_FOREGROUND)
            {
                Console.WriteLine("And <" + watchingTitle + "/" + watchingHandle.ToString() + ">'s " + (IsPalmed ? "" : "not ") + "palmed.");
                Console.WriteLine("And it's " + (isOverCovered(watchingHandle) ? "covered" : "not covered") + ".");
                if (title == watchingTitle)
                {
                    //MessageBox.Show(title);
                    Console.WriteLine(title + ", gotcha.");
                    if (!IsPalmed && !isOverCovered(watchingHandle))
                    {
                        showMeOverU(hwnd);
                        this.Text = mainFormTitle + "->[" + watchingTitle + "]";
                    }
                }
                else
                {
                    Console.WriteLine("Now, it's not the watching one on top, it's <" + title + ">.");
                    
                    if (isOverCovered(watchingHandle))
                    {
                        Console.WriteLine("***Now this <" + watchingTitle + "> window is invisible.***");
                        //IsPalmed = false;
                    }
                }
            }
        }

        private String watchingTitle = "Signal";
        private IntPtr watchingHandle = IntPtr.Zero;
        private String mainFormTitle = "Facepalm";
        static private Boolean IsPalmed = false;
        private Boolean is1st = true;

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint wWcmd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        private void BtnClickMe_Click(object sender, EventArgs e)
        {
            listBoxShowing.Items.Clear();
            Process[] myProcesses = Process.GetProcesses();
            int cnt = 0;
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.MainWindowTitle.Length > 0 && Process.GetCurrentProcess().Id != myProcess.Id)
                {
                    listBoxShowing.Items.Add(myProcess.MainWindowTitle);
                }
                if (myProcess.MainWindowTitle == watchingTitle)
                {
                    cnt++;
                    MessageBox.Show("Hey, [" + watchingTitle + ", " + myProcess.Id + "," + myProcess.SessionId + "], there you are.(" + cnt + " times)");
                }
                else
                {
                    // do nothing or ...
                }
            }
        }

        void listBoxShowing_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxShowing.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                // to do what you want
                /* save the picked (double clicked) item to a local file like ini or sth. like that,
                 * and watch the item, then.
                */
                changeWatchingTitle(listBoxShowing.Items[index].ToString());
                setConfigValue("watching", watchingTitle);
                MessageBox.Show("Window [" + watchingTitle + "], watched.");
            }
        }

        bool isItVisible(IntPtr hWnd)
        {
            UInt32 style = GetWindowLong(hWnd, GWL_STYLE);
            bool visible = ((style & WS_VISIBLE) != WS_VISIBLE);
            return visible;
        }

        bool isOverCovered(IntPtr hWnd) {
            RECT rect = new RECT();
            IntPtr desktop = GetDesktopWindow();
            IntPtr win = GetWindow(desktop, 5);
            Console.WriteLine("******************loop starts*******************");
            while (win != IntPtr.Zero)
            {
                if (GetWindowRect(win, ref rect))
                {
                    Rectangle winrect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                    if (winrect == Rectangle.Empty)
                    {
                        Console.WriteLine("No window of this app [" + GetWindowTitle(win) + "]");
                    }
                    else
                    {
                        Console.WriteLine($"Current app [{GetWindowTitle(win)}]'s rectangle: {winrect.ToString()}");
                        GetWindowRect(watchingHandle, ref rect);
                        Rectangle watchingrect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                        if (watchingrect.IntersectsWith(winrect))
                        {
                            Console.WriteLine($"{this.watchingTitle} and {GetWindowTitle(win)} are  crossing to each other.");
                        }
                        else
                        {
                            Console.WriteLine("----------------------------");
                        }
                    }
                }
                win = GetWindow(win, 2);
            }
            Console.WriteLine("******************loop ends*******************");

            if (GetWindowRect(hWnd, ref rect))
            {
                Rectangle winrect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                if (winrect == Rectangle.Empty)
                {
                    Console.WriteLine("Can't get the rectangle of the window.");
                    return false;
                }
                Console.WriteLine($"Current Main Rectangle: {winrect.ToString()}");
            }

            return false;
        }

        void changeWatchingTitle(String w)
        {
            watchingTitle = w;
            this.Text = mainFormTitle + "->[" + w + "]";
            notifyIcon.Text = this.Text;
        }
        void showMeOverU(IntPtr handle)
        {
            // Select the tab to show
            tabControl.SelectTab(1);
            textPwd.Focus();

            watchingHandle = handle;

            RECT rect = new RECT
            {
                Left = 0,
                Top = 0,
                Right = 11,
                Bottom = 11
            };
            _ = GetWindowRect(handle, ref rect);

            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.TopMost = true;
            this.Location = new System.Drawing.Point(rect.Left, rect.Top);
            this.Size = new System.Drawing.Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            this.Show();
            this.Refresh();
            IsPalmed = true;
        }

        private string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public String getConfigValue(String key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            return ConfigurationManager.AppSettings[key];
        }

        public bool setConfigValue(String key, String value) 
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config != null)
                {
                    AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
                    if (appSettings.Settings.AllKeys.Contains(key))
                    {
                        appSettings.Settings[key].Value = value;
                    } else
                    {
                        appSettings.Settings.Add(key, value);
                    }
                    config.Save();
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private void hideToNotifyIcon_switchToWatchingWindow(IntPtr hWnd)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIcon.Visible = true;
            IsPalmed = true;
            textPwd.Text = "";
            textPwd.Focus();
            if (hWnd != IntPtr.Zero)
            {
                SwitchToThisWindow(hWnd, true);
            }
        }

        private void FormAMB_Shown(object sender, EventArgs e)
        {
            if (is1st)
            {
                is1st = false;
                IsPalmed = false;
            } else
            {
                //
            }
        }

        private void FormAMB_Deactivate(object sender, EventArgs e)
        {
            //this.TopMost = false;
            //IsPalmed = false;
        }

        private void FormAMB_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            hideToNotifyIcon_switchToWatchingWindow(watchingHandle);
        }

        private void FormAMB_Resize(object sender, EventArgs e)
        {
            /*
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.notifyIcon.Visible = true;
                IsPalmed = false;
            }
            */
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            tabControl.SelectTab(0);
            this.Show();
            this.WindowState = FormWindowState.Normal;
            //notifyIcon.Visible = false;
            this.ShowInTaskbar = true;
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }

        private void FormAMB_Load(object sender, EventArgs e)
        {
            String watching = getConfigValue("watching");
            if (!String.IsNullOrEmpty(watching))
            {
                changeWatchingTitle(watching);
            }
            //backgroundWorker.RunWorkerAsync();
        }

        private void textPwd_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 4)
            {
                String pwd = getConfigValue("password");
                if (String.IsNullOrEmpty(pwd))
                {
                    MessageBox.Show("It seems that it's your first time entering the password," +
                        " please remember it, a 4 digits. You need to enter the same one the next time.");
                    setConfigValue("password", MD5Hash(tb.Text));
                    hideToNotifyIcon_switchToWatchingWindow(watchingHandle);
                } 
                else
                {
                    if (pwd != MD5Hash(tb.Text))
                    {
                        tb.Text = "";
                        tb.Focus();
                        MessageBox.Show("Wrong, please re-enter it.");
                    }
                    else
                    {
                        hideToNotifyIcon_switchToWatchingWindow(watchingHandle);
                    }
                }
            }
        }

        private void textPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;
            base.OnKeyPress(e);
            if (tb.ReadOnly)
            {
                return;
            }
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            } 
            else
            {
                
            }
        }

        private void FormAMB_Activated(object sender, EventArgs e)
        {
            textPwd.Focus();
        }
    }
}
