using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Linq;

namespace GetLocked
{
    public partial class FormAMB : Form
    {
        public FormAMB()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.listBoxShowing.MouseDoubleClick += new MouseEventHandler(listBoxShowing_MouseDoubleClick);
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
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

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
                MessageBox.Show(watchingTitle + ", watched.");
            }
        }

        void changeWatchingTitle(String w)
        {
            watchingTitle = w;
            this.Text = mainFormTitle + "->[" + watchingTitle + "]";
        }
        void showMeOverU(IntPtr handle)
        {
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

        public String getConfigValue(String key)
        {
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

        private void FormAMB_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void FormAMB_Deactivate(object sender, EventArgs e)
        {
            this.TopMost = false;
            IsPalmed = false;
        }

        //check if minimized
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x112)
            {
                if (m.WParam.ToInt32() == 0xF020) {
                    this.TopMost = false;
                    IsPalmed = false;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void FormAMB_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIcon.Visible = true;
            IsPalmed = false;
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
                if (!IsPalmed)
                {
                    ;
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
                                    //Todo: MainWindowTitle named "name" focused, do something, then.
                                    ProcChkBk pcb = new ProcChkBk();
                                    pcb.Id = myProcess.MainWindowHandle;
                                    pcb.Msg = watchingTitle + " focused.(" + DateTime.Now.ToString() + ")";
                                    bgWorker.ReportProgress(0, pcb);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            /*
            int Interval = e.ProgressPercentage;
            String Title = e.UserState.ToString();
            this.Text = Interval.ToString() + "--" + Title;
            */
            ProcChkBk pcb = e.UserState as ProcChkBk;
            listBoxShowing.Items.Add(pcb.Msg);
            showMeOverU(pcb.Id);
            this.Text = mainFormTitle + "->[" + watchingTitle + "]";
        }
    }

    class ProcChkBk
    {
        public IntPtr Id;
        public String Msg;
    }
}
