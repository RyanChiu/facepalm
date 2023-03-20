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
        }

        private String mainWinTitle = "Signal";

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
            labelShowing.Text = "Hello, world!";
            if (textBoxShowing.Text == string.Empty)
            {
                this.listBoxShowing.MouseDoubleClick += new MouseEventHandler(listBoxShowing_MouseDoubleClick);
                textBoxShowing.Text = "Hello! World!";
                BtnClickMe.Text = "Show me the system processes!";
            }
            else
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
    }
}
