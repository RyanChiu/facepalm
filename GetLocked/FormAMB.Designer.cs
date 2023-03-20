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
            this.BtnClickMe = new System.Windows.Forms.Button();
            this.labelShowing = new System.Windows.Forms.Label();
            this.textBoxShowing = new System.Windows.Forms.TextBox();
            this.listBoxShowing = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // BtnClickMe
            // 
            this.BtnClickMe.Location = new System.Drawing.Point(231, 509);
            this.BtnClickMe.Name = "BtnClickMe";
            this.BtnClickMe.Size = new System.Drawing.Size(463, 23);
            this.BtnClickMe.TabIndex = 0;
            this.BtnClickMe.Text = "ClickMe";
            this.BtnClickMe.UseVisualStyleBackColor = true;
            this.BtnClickMe.Click += new System.EventHandler(this.BtnClickMe_Click);
            // 
            // labelShowing
            // 
            this.labelShowing.AutoSize = true;
            this.labelShowing.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelShowing.ForeColor = System.Drawing.Color.Salmon;
            this.labelShowing.Location = new System.Drawing.Point(408, 545);
            this.labelShowing.Name = "labelShowing";
            this.labelShowing.Size = new System.Drawing.Size(103, 16);
            this.labelShowing.TabIndex = 1;
            this.labelShowing.Text = "Show me sth.";
            // 
            // textBoxShowing
            // 
            this.textBoxShowing.Location = new System.Drawing.Point(12, 12);
            this.textBoxShowing.Multiline = true;
            this.textBoxShowing.Name = "textBoxShowing";
            this.textBoxShowing.ReadOnly = true;
            this.textBoxShowing.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxShowing.Size = new System.Drawing.Size(902, 209);
            this.textBoxShowing.TabIndex = 2;
            this.textBoxShowing.WordWrap = false;
            // 
            // listBoxShowing
            // 
            this.listBoxShowing.FormattingEnabled = true;
            this.listBoxShowing.ItemHeight = 12;
            this.listBoxShowing.Location = new System.Drawing.Point(12, 227);
            this.listBoxShowing.MultiColumn = true;
            this.listBoxShowing.Name = "listBoxShowing";
            this.listBoxShowing.Size = new System.Drawing.Size(902, 256);
            this.listBoxShowing.TabIndex = 3;
            // 
            // FormAMB
            // 
            this.ClientSize = new System.Drawing.Size(926, 579);
            this.Controls.Add(this.listBoxShowing);
            this.Controls.Add(this.textBoxShowing);
            this.Controls.Add(this.labelShowing);
            this.Controls.Add(this.BtnClickMe);
            this.Name = "FormAMB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnClickMe;
        private System.Windows.Forms.Label labelShowing;
        private System.Windows.Forms.TextBox textBoxShowing;
        private System.Windows.Forms.ListBox listBoxShowing;
    }
}

