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
        static private Gram g = new Gram();
        static System.Threading.Timer myTimer;

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

            if (this.TopMost)
            {
                this.TopMost = false;
            } else
            {
                this.TopMost = true;
            }
            this.Location = new System.Drawing.Point(rect.Left, rect.Top);
            this.Size = new System.Drawing.Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            this.Show();
        }

        private void FormAMB_Load(object sender, EventArgs e)
        {
            myTimer = new System.Threading.Timer(G.Display, this, 2000, 1000);
            Console.WriteLine("Timer started.");
            Console.ReadLine();
            this.Text = "Timer started";
        }
    }

    class Gram
    {
        static private int TimesCalled = 0;
        public void Display(object obj)
        {
            Form frm = (Form)obj;
            Console.WriteLine("{0} {1} keep running.", frm.Text, ++TimesCalled);
            frm.Text += TimesCalled.ToString();
        }
    }
}
