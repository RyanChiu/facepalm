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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAMB));
            this.textBoxShowing = new System.Windows.Forms.TextBox();
            this.BtnClickMe = new System.Windows.Forms.Button();
            this.listBoxShowing = new System.Windows.Forms.ListBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxShowing
            // 
            this.textBoxShowing.Location = new System.Drawing.Point(3, 4);
            this.textBoxShowing.Multiline = true;
            this.textBoxShowing.Name = "textBoxShowing";
            this.textBoxShowing.ReadOnly = true;
            this.textBoxShowing.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxShowing.Size = new System.Drawing.Size(185, 338);
            this.textBoxShowing.TabIndex = 2;
            this.textBoxShowing.WordWrap = false;
            // 
            // BtnClickMe
            // 
            this.BtnClickMe.AutoSize = true;
            this.BtnClickMe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnClickMe.Location = new System.Drawing.Point(0, 0);
            this.BtnClickMe.Name = "BtnClickMe";
            this.BtnClickMe.Size = new System.Drawing.Size(548, 25);
            this.BtnClickMe.TabIndex = 0;
            this.BtnClickMe.Text = "ClickMe";
            this.BtnClickMe.UseVisualStyleBackColor = true;
            this.BtnClickMe.Click += new System.EventHandler(this.BtnClickMe_Click);
            // 
            // listBoxShowing
            // 
            this.listBoxShowing.FormattingEnabled = true;
            this.listBoxShowing.ItemHeight = 12;
            this.listBoxShowing.Location = new System.Drawing.Point(12, 4);
            this.listBoxShowing.MultiColumn = true;
            this.listBoxShowing.Name = "listBoxShowing";
            this.listBoxShowing.Size = new System.Drawing.Size(330, 340);
            this.listBoxShowing.TabIndex = 3;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.textBoxShowing);
            this.splitContainer.Size = new System.Drawing.Size(548, 347);
            this.splitContainer.SplitterDistance = 345;
            this.splitContainer.SplitterWidth = 3;
            this.splitContainer.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxShowing);
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.BtnClickMe);
            this.splitContainer1.Size = new System.Drawing.Size(548, 376);
            this.splitContainer1.SplitterDistance = 347;
            this.splitContainer1.TabIndex = 5;
            // 
            // FormAMB
            // 
            this.ClientSize = new System.Drawing.Size(548, 376);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAMB";
            this.Text = "Facepalm";
            this.Load += new System.EventHandler(this.FormAMB_Load);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxShowing;
        private System.Windows.Forms.Button BtnClickMe;
        private System.Windows.Forms.ListBox listBoxShowing;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

