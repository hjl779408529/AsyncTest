namespace AsyncTest
{
    partial class MainForm
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
            this.Start_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.Status_progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Start_button
            // 
            this.Start_button.Location = new System.Drawing.Point(12, 132);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(127, 54);
            this.Start_button.TabIndex = 0;
            this.Start_button.Text = "启动";
            this.Start_button.UseVisualStyleBackColor = true;
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(591, 132);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(127, 54);
            this.Cancel_button.TabIndex = 1;
            this.Cancel_button.Text = "取消";
            this.Cancel_button.UseVisualStyleBackColor = true;
            // 
            // Status_progressBar
            // 
            this.Status_progressBar.Location = new System.Drawing.Point(12, 43);
            this.Status_progressBar.Name = "Status_progressBar";
            this.Status_progressBar.Size = new System.Drawing.Size(706, 47);
            this.Status_progressBar.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 205);
            this.Controls.Add(this.Status_progressBar);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Start_button);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "主页面";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Start_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.ProgressBar Status_progressBar;
    }
}

