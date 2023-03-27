using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        private String mainWinTitle = "Signal";
        private String mainFormTitle = "Facepalm";
        private Boolean is1st = true;
        static private Gram g = new Gram();
        static private System.Threading.Timer myTimer;

        internal static Gram G { get => g; set => g = value; }

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
            textBoxShowing.Text = string.Empty;
            listBoxShowing.Items.Clear();
            Process[] myProcesses = Process.GetProcesses();
            int cnt = 0;
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.MainWindowTitle.Length > 0)
                {
                    textBoxShowing.Text += "Task name: " + myProcess.MainWindowTitle
                       + " [Id:" + myProcess.Id
                       + ", sessionId:" + myProcess.SessionId
                       + ", procName:" + myProcess.ProcessName
                       // + ", Container:" + myProcess.Container.Components.ToString()
                       + "]" + "\r\n";

                    listBoxShowing.Items.Add(myProcess.MainWindowTitle);
                }
                if (myProcess.MainWindowTitle == mainWinTitle)
                {
                    cnt++;
                    MessageBox.Show("Hey, [" + mainWinTitle + ", " + myProcess.Id + "," + myProcess.SessionId + "], there you are.(" + cnt + " times)");
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
                String mainWinTitle = listBoxShowing.Items[index].ToString();
                MessageBox.Show(mainWinTitle);
                Process[] myProcesses = Process.GetProcesses();
                foreach (Process myProcess in myProcesses)
                {
                    if (myProcess.MainWindowTitle == mainWinTitle)
                    {
                        SwitchToThisWindow(myProcess.MainWindowHandle, true);
                        showMeOverU(myProcess.MainWindowHandle);
                    }
                }
            }
        }

        private void timerStart()
        {
            myTimer = new System.Threading.Timer(G.Display, this, 500, 1000);
            this.mainFormTitle = "Timer started";
        }

        private void timerStop()
        {
            if (myTimer != null)
            {
                myTimer.Dispose();
            }
        }

        public void checkIfFocused(String name)
        {
            String title = "";
            Process[] myProcesses = Process.GetProcesses();
            IntPtr id = GetForegroundWindow();
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.MainWindowTitle.Length > 0)
                {
                    if (myProcess.MainWindowHandle == id)
                    {
                        title = myProcess.MainWindowTitle;
                        if (myProcess.MainWindowTitle == name)
                        {
                            //MainWindowTitle named "name" found, do something, then.
                            SwitchToThisWindow(myProcess.MainWindowHandle, true);
                            showMeOverU(myProcess.MainWindowHandle);
                            timerStop();
                        }
                        break;
                    }
                }
            }
            Console.WriteLine("Got you, 'Name:{0}, ID:{1}'!", (title == "" ? "N/A" : title), GetForegroundWindow().ToString());
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

            this.TopMost = true;
            this.Location = new System.Drawing.Point(rect.Left, rect.Top);
            this.Size = new System.Drawing.Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            this.Show();
            this.Refresh();
        }

        public String getFormTitle()
        {
            return mainFormTitle;
        }

        private void FormAMB_Load(object sender, EventArgs e)
        {
            timerStart();
        }

        private void FormAMB_Shown(object sender, EventArgs e)
        {
            if (is1st)
            {
                is1st = false;
            } else
            {
                timerStop();
            }
        }

        private void FormAMB_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerStop();
        }

        private void FormAMB_Deactivate(object sender, EventArgs e)
        {
            this.TopMost = false;
            timerStop();
            timerStart();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x112)
            {
                if (m.WParam.ToInt32() == 0xF020) {
                    this.TopMost = false;
                    timerStop();
                    timerStart();
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
        }

        private void FormAMB_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.notifyIcon.Visible = true;
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
            myTimer.Dispose();
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }
    }

    class Gram
    {
        static private int TimesCalled = 0;
        public void Display(object obj)
        {
            FormAMB frm = (FormAMB)obj;
            frm.Text = String.Format("{0} {1}s, keep running.", frm.getFormTitle(), ++TimesCalled);
            frm.checkIfFocused("");
        }
    }
}
