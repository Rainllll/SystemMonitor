namespace FormMain
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Button btnSetInterval;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Button btnSetThresholds;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.btnSetInterval = new System.Windows.Forms.Button();
            this.lblInterval = new System.Windows.Forms.Label();
            this.btnSetThresholds = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 50);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(560, 300);
            this.txtLog.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 30);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(93, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 30);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(270, 16);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(50, 28);
            this.txtInterval.TabIndex = 4;
            this.txtInterval.Text = "1";
            // 
            // btnSetInterval
            // 
            this.btnSetInterval.Location = new System.Drawing.Point(330, 12);
            this.btnSetInterval.Name = "btnSetInterval";
            this.btnSetInterval.Size = new System.Drawing.Size(75, 30);
            this.btnSetInterval.TabIndex = 5;
            this.btnSetInterval.Text = "设置周期";
            this.btnSetInterval.UseVisualStyleBackColor = true;
            this.btnSetInterval.Click += new System.EventHandler(this.btnSetInterval_Click);
            // 
            // btnSetThresholds
            // 
            this.btnSetThresholds.Location = new System.Drawing.Point(420, 12);
            this.btnSetThresholds.Name = "btnSetThresholds";
            this.btnSetThresholds.Size = new System.Drawing.Size(100, 30);
            this.btnSetThresholds.TabIndex = 6;
            this.btnSetThresholds.Text = "设置阈值";
            this.btnSetThresholds.UseVisualStyleBackColor = true;
            this.btnSetThresholds.Click += new System.EventHandler(this.btnSetThresholds_Click);
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(200, 20);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(89, 18);
            this.lblInterval.TabIndex = 3;
            this.lblInterval.Text = "采集周期:";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.btnSetThresholds);
            this.Controls.Add(this.btnSetInterval);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.lblInterval);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtLog);
            this.Name = "MainForm";
            this.Text = "数据采集程序";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}