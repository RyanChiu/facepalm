using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace GetLocked
{
    public partial class FormAMB : Form
    {
        WinEventDelegate deleFG = null;
        WinEventDelegate deleSH = null;
        public FormAMB()
        {
            InitializeComponent();
            this.Tag = 0;

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
            // Write every "activated window"'s title into the console.
            String title = GetWindowTitle(hwnd);
            Console.WriteLine($"hWnd:{hwnd}, title:{title}, eventType:{eventType.ToString()}");
            if (eventType == EVENT_SYSTEM_FOREGROUND && title != null)
            {
                Console.WriteLine($"And <{watchingTitle}/{watchingHandle.ToString()}>'s " + (IsPalmed ? "" : "not ") + "palmed.");
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
                        int i = (int)this.Tag;
                        i++;
                        this.Tag = i;
                        notifyIcon.ShowBalloonTip(500, $"Count ({this.Tag.ToString()})",
                            $"hWnd:{hwnd}, title:{title}, eventType:{eventType.ToString()}", ToolTipIcon.Info);
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

        [StructLayout(LayoutKind.Sequential)]
        public readonly struct LPRECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;
        }

        public readonly struct WindowInfo
        {
            public WindowInfo(IntPtr hWnd, string className, string title, bool isVisible, Rectangle bounds) : this()
            {
                Hwnd = hWnd;
                ClassName = className;
                Title = title;
                IsVisible = isVisible;
                Bounds = bounds;
            }

            /// <summary>
            /// 获取窗口句柄。
            /// </summary>
            public IntPtr Hwnd { get; }

            /// <summary>
            /// 获取窗口类名。
            /// </summary>
            public string ClassName { get; }

            /// <summary>
            /// 获取窗口标题。
            /// </summary>
            public string Title { get; }

            /// <summary>
            /// 获取当前窗口是否可见。
            /// </summary>
            public bool IsVisible { get; }

            /// <summary>
            /// 获取窗口当前的位置和尺寸。
            /// </summary>
            public Rectangle Bounds { get; }

            /// <summary>
            /// 获取窗口当前是否是最小化的。
            /// </summary>
            public bool IsMinimized => Bounds.Left == -32000 && Bounds.Top == -32000;
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

        [DllImport("user32")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        private static List<WindowInfo> GetAllWindowsAbove(IntPtr hWnd)
        {
            var windowInfos = new List<WindowInfo>();
            var win = GetWindow(hWnd, 3); //GW_HWNDPREV = 3
            if (win == IntPtr.Zero)
            {
                return windowInfos;
            }
            var winDetails = GetWindowDetail(win);
            windowInfos.AddRange(GetAllWindowsAbove(win));
            windowInfos.Add(winDetails);
            return windowInfos;
        }

        private static WindowInfo GetWindowDetail(IntPtr hWnd)
        {
            // get classname of the window
            var lpString = new StringBuilder(512);
            GetClassName(hWnd, lpString, lpString.Capacity);
            var className = lpString.ToString();

            // get title of the window
            var lptrString = new StringBuilder(512);
            GetWindowText(hWnd, lptrString, lptrString.Capacity);
            var title = lptrString.ToString().Trim();

            // get visibility of the window
            var isVisible = IsWindowVisible(hWnd);

            // get size of the window
            RECT rect = new RECT();
            GetWindowRect(hWnd, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

            return new WindowInfo(hWnd, className, title, isVisible, bounds);
        }
        private void BtnClickMe_Click(object sender, EventArgs e)
        {
            /**
             * a text block here, temply
             */
            RECT rect = new RECT();
            GetWindowRect(watchingHandle, ref rect);
            Rectangle watchingRect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var windows = GetAllWindowsAbove(watchingHandle);
            var windowInfos = windows.Where(I => I.IsVisible && !I.IsMinimized && I.Title != ""
                && I.Hwnd != watchingHandle).ToList();
            Console.WriteLine($"\r\nStart listing window(s) above>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" +
                $"watchingHandle:{watchingHandle},watchingTitle:{watchingTitle},watchingRect:{watchingRect.ToString()}");
            foreach (var windowInfo in windowInfos)
            {
                
                Console.WriteLine($"Hwnd:{windowInfo.Hwnd},Title:{windowInfo.Title},IsMinimized:{windowInfo.IsMinimized}," +
                    $"IsVisible:{windowInfo.IsVisible},ClassName:{windowInfo.ClassName},Bounds:{windowInfo.Bounds}\r\n" +
                    $"IsCrossed:{watchingRect.IntersectsWith(windowInfo.Bounds)}");
                
            }
            Console.WriteLine($"{windowInfos.Count}<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<end(s) up there.");

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
            if (hWnd == IntPtr.Zero) return false;
            String title = GetWindowTitle(hWnd);
            RECT r = new RECT();
            GetWindowRect(hWnd, ref r);
            Rectangle rect = new Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
            var windows = GetAllWindowsAbove(hWnd);
            var windowInfos = windows.Where(I => I.IsVisible && !I.IsMinimized && I.Title != ""
                && I.Bounds.Width > 1 && I.Bounds.Height > 1
                && I.Hwnd != hWnd).ToList();
            Console.WriteLine($"\r\nStart rolling window(s) above the watching one>>>>>>>>>>>>>>>>>>" +
                $"handle:{hWnd}, title:{title}, rect:{rect.ToString()}");
            foreach (var windowInfo in windowInfos)
            {

                Console.WriteLine($"Hwnd:{windowInfo.Hwnd},Title:{windowInfo.Title},IsMinimized:{windowInfo.IsMinimized}," +
                    $"IsVisible:{windowInfo.IsVisible},ClassName:{windowInfo.ClassName},Bounds:{windowInfo.Bounds}\r\n" +
                    $"IsCrossed:{rect.IntersectsWith(windowInfo.Bounds)}");

                if (rect.IntersectsWith(windowInfo.Bounds))
                {
                    Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<ends up with crossing found.\r\n");
                    return true;
                }
            }
            Console.WriteLine($"{windowInfos.Count}<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<just end(s) up there.\r\n");

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

        private void hideToNotifyIcon()
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIcon.Visible = true;
            IsPalmed = true;
            textPwd.Text = "";
            textPwd.Focus();
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
            hideToNotifyIcon();
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
                    hideToNotifyIcon();
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
                        hideToNotifyIcon();
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
