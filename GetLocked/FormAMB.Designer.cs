using System.Windows.Forms;

namespace GetLocked
{
    partial class FormAMB
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAMB));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.tabPalming = new System.Windows.Forms.TabPage();
            this.splitContainerSettings = new System.Windows.Forms.SplitContainer();
            this.listBoxShowing = new System.Windows.Forms.ListBox();
            this.BtnClickMe = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSettings)).BeginInit();
            this.splitContainerSettings.Panel1.SuspendLayout();
            this.splitContainerSettings.Panel2.SuspendLayout();
            this.splitContainerSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Facepalm";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(95, 26);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItemExit.Text = "E&xit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Controls.Add(this.tabPalming);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(548, 376);
            this.tabControl.TabIndex = 1;
            this.tabControl.SizeMode = TabSizeMode.Fixed;
            this.tabControl.ItemSize = new System.Drawing.Size(0, 1);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.splitContainerSettings);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(540, 350);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "tabSettings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // tabPalming
            // 
            this.tabPalming.BackgroundImage = global::Facepalm.Properties.Resources.wallhaven_4gq2xl;
            this.tabPalming.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPalming.Location = new System.Drawing.Point(4, 22);
            this.tabPalming.Name = "tabPalming";
            this.tabPalming.Padding = new System.Windows.Forms.Padding(3);
            this.tabPalming.Size = new System.Drawing.Size(540, 350);
            this.tabPalming.TabIndex = 1;
            this.tabPalming.Text = "tabPalming";
            this.tabPalming.UseVisualStyleBackColor = true;
            // 
            // splitContainerSettings
            // 
            this.splitContainerSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSettings.Location = new System.Drawing.Point(3, 3);
            this.splitContainerSettings.Name = "splitContainerSettings";
            this.splitContainerSettings.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSettings.Panel1
            // 
            this.splitContainerSettings.Panel1.Controls.Add(this.listBoxShowing);
            // 
            // splitContainerSettings.Panel2
            // 
            this.splitContainerSettings.Panel2.Controls.Add(this.BtnClickMe);
            this.splitContainerSettings.Size = new System.Drawing.Size(534, 344);
            this.splitContainerSettings.SplitterDistance = 315;
            this.splitContainerSettings.TabIndex = 6;
            // 
            // listBoxShowing
            // 
            this.listBoxShowing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxShowing.FormattingEnabled = true;
            this.listBoxShowing.ItemHeight = 12;
            this.listBoxShowing.Location = new System.Drawing.Point(0, 0);
            this.listBoxShowing.MultiColumn = true;
            this.listBoxShowing.Name = "listBoxShowing";
            this.listBoxShowing.Size = new System.Drawing.Size(534, 315);
            this.listBoxShowing.TabIndex = 4;
            // 
            // BtnClickMe
            // 
            this.BtnClickMe.AutoSize = true;
            this.BtnClickMe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnClickMe.Location = new System.Drawing.Point(0, 0);
            this.BtnClickMe.Name = "BtnClickMe";
            this.BtnClickMe.Size = new System.Drawing.Size(534, 25);
            this.BtnClickMe.TabIndex = 0;
            this.BtnClickMe.Text = "Show me the windows";
            this.BtnClickMe.UseVisualStyleBackColor = true;
            this.BtnClickMe.Click += new System.EventHandler(this.BtnClickMe_Click);
            // 
            // FormAMB
            // 
            this.ClientSize = new System.Drawing.Size(548, 376);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAMB";
            this.Text = "Facepalm";
            this.Deactivate += new System.EventHandler(this.FormAMB_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAMB_FormClosing);
            this.Load += new System.EventHandler(this.FormAMB_Load);
            this.Shown += new System.EventHandler(this.FormAMB_Shown);
            this.Resize += new System.EventHandler(this.FormAMB_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.splitContainerSettings.Panel1.ResumeLayout(false);
            this.splitContainerSettings.Panel2.ResumeLayout(false);
            this.splitContainerSettings.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSettings)).EndInit();
            this.splitContainerSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.SplitContainer splitContainerSettings;
        private System.Windows.Forms.ListBox listBoxShowing;
        private System.Windows.Forms.Button BtnClickMe;
        private System.Windows.Forms.TabPage tabPalming;
    }
}

