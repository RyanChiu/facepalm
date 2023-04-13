using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;
using System.CodeDom;
using System.Security.Cryptography;
using System.Text;

namespace GetLocked
{
    public partial class FormAMB : Form
    {
        WinEventDelegate dele = null;
        public FormAMB()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.listBoxShowing.MouseDoubleClick += new MouseEventHandler(listBoxShowing_MouseDoubleClick);

            dele = new WinEventDelegate(WinEventProc);
            IntPtr m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, dele, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            // Write every "activated window"'s title on the console line.
            Console.WriteLine(GetActiveWindowTitle());
        }

        private String watchingTitle = "Signal";
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
        public static extern bool IsWindowVisible(IntPtr hWnd);

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
            this.TopMost = false;
            IsPalmed = false;
        }

        private void FormAMB_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            hideToNotifyIcon();
        }

        private void FormAMB_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.notifyIcon.Visible = true;
                IsPalmed = false;
            }
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
            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = sender as BackgroundWorker;
            while (true)
            {
                Process[] myProcesses = Process.GetProcesses();
                IntPtr id = GetForegroundWindow();
                foreach (Process myProcess in myProcesses)
                {
                    if (myProcess.MainWindowTitle.Length > 0)
                    {
                        if (myProcess.MainWindowHandle == id)
                        {
                            if (myProcess.MainWindowTitle == watchingTitle)
                            {
                                //Todo: MainWindowTitle named "watchingTile" focused.
                                if (!IsPalmed) // if it's not palmed, then report it
                                {
                                    ProcChkBk pcb = new ProcChkBk();
                                    pcb.Id = myProcess.MainWindowHandle;
                                    pcb.Msg = watchingTitle + " focused.(" + DateTime.Now.ToString() + ")";
                                    bgWorker.ReportProgress(0, pcb);
                                }
                            }
                            else
                            {
                                IsPalmed = false;
                            }
                        }
                    }
                }
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ProcChkBk pcb = e.UserState as ProcChkBk;
            listBoxShowing.Items.Add(pcb.Msg);
            showMeOverU(pcb.Id);
            this.Text = mainFormTitle + "->[" + watchingTitle + "]";
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

    class ProcChkBk
    {
        public IntPtr Id;
        public String Msg;
    }
}
