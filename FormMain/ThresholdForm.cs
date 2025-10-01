using System;
using System.Windows.Forms;

namespace FormMain
{
    public partial class ThresholdForm : Form
    {
        // 属性用于存储阈值设置
        public double CpuUsageWarn { get; set; }
        public double CpuUsageDanger { get; set; }
        public double AvailMemoryWarn { get; set; }
        public double AvailMemoryDanger { get; set; }
        public double CpuTempWarn { get; set; }
        public double CpuTempDanger { get; set; }
        public double VolatilityThreshold { get; set; }

        public ThresholdForm(double cpuUsageWarn, double cpuUsageDanger, 
                            double availMemoryWarn, double availMemoryDanger, 
                            double cpuTempWarn, double cpuTempDanger, 
                            double volatilityThreshold)
        {
            InitializeComponent();
            
            // 初始化表单值
            this.CpuUsageWarn = cpuUsageWarn;
            this.CpuUsageDanger = cpuUsageDanger;
            this.AvailMemoryWarn = availMemoryWarn;
            this.AvailMemoryDanger = availMemoryDanger;
            this.CpuTempWarn = cpuTempWarn;
            this.CpuTempDanger = cpuTempDanger;
            this.VolatilityThreshold = volatilityThreshold;
            
            // 显示当前阈值
            txtCpuUsageWarn.Text = cpuUsageWarn.ToString();
            txtCpuUsageDanger.Text = cpuUsageDanger.ToString();
            txtAvailMemoryWarn.Text = availMemoryWarn.ToString();
            txtAvailMemoryDanger.Text = availMemoryDanger.ToString();
            txtCpuTempWarn.Text = cpuTempWarn.ToString();
            txtCpuTempDanger.Text = cpuTempDanger.ToString();
            txtVolatilityThreshold.Text = volatilityThreshold.ToString();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCpuUsageWarn = new System.Windows.Forms.TextBox();
            this.txtCpuUsageDanger = new System.Windows.Forms.TextBox();
            this.txtAvailMemoryWarn = new System.Windows.Forms.TextBox();
            this.txtAvailMemoryDanger = new System.Windows.Forms.TextBox();
            this.txtCpuTempWarn = new System.Windows.Forms.TextBox();
            this.txtCpuTempDanger = new System.Windows.Forms.TextBox();
            this.txtVolatilityThreshold = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPU使用率警告:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "CPU使用率危险:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "可用内存警告:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "可用内存危险:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "CPU温度警告:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "CPU温度危险:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "波动率阈值:";
            // 
            // txtCpuUsageWarn
            // 
            this.txtCpuUsageWarn.Location = new System.Drawing.Point(130, 17);
            this.txtCpuUsageWarn.Name = "txtCpuUsageWarn";
            this.txtCpuUsageWarn.Size = new System.Drawing.Size(100, 28);
            this.txtCpuUsageWarn.TabIndex = 7;
            // 
            // txtCpuUsageDanger
            // 
            this.txtCpuUsageDanger.Location = new System.Drawing.Point(130, 47);
            this.txtCpuUsageDanger.Name = "txtCpuUsageDanger";
            this.txtCpuUsageDanger.Size = new System.Drawing.Size(100, 28);
            this.txtCpuUsageDanger.TabIndex = 8;
            // 
            // txtAvailMemoryWarn
            // 
            this.txtAvailMemoryWarn.Location = new System.Drawing.Point(130, 77);
            this.txtAvailMemoryWarn.Name = "txtAvailMemoryWarn";
            this.txtAvailMemoryWarn.Size = new System.Drawing.Size(100, 28);
            this.txtAvailMemoryWarn.TabIndex = 9;
            // 
            // txtAvailMemoryDanger
            // 
            this.txtAvailMemoryDanger.Location = new System.Drawing.Point(130, 107);
            this.txtAvailMemoryDanger.Name = "txtAvailMemoryDanger";
            this.txtAvailMemoryDanger.Size = new System.Drawing.Size(100, 28);
            this.txtAvailMemoryDanger.TabIndex = 10;
            // 
            // txtCpuTempWarn
            // 
            this.txtCpuTempWarn.Location = new System.Drawing.Point(130, 137);
            this.txtCpuTempWarn.Name = "txtCpuTempWarn";
            this.txtCpuTempWarn.Size = new System.Drawing.Size(100, 28);
            this.txtCpuTempWarn.TabIndex = 11;
            // 
            // txtCpuTempDanger
            // 
            this.txtCpuTempDanger.Location = new System.Drawing.Point(130, 167);
            this.txtCpuTempDanger.Name = "txtCpuTempDanger";
            this.txtCpuTempDanger.Size = new System.Drawing.Size(100, 28);
            this.txtCpuTempDanger.TabIndex = 12;
            // 
            // txtVolatilityThreshold
            // 
            this.txtVolatilityThreshold.Location = new System.Drawing.Point(130, 197);
            this.txtVolatilityThreshold.Name = "txtVolatilityThreshold";
            this.txtVolatilityThreshold.Size = new System.Drawing.Size(100, 28);
            this.txtVolatilityThreshold.TabIndex = 13;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(50, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(150, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ThresholdForm
            // 
            this.ClientSize = new System.Drawing.Size(270, 280);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtVolatilityThreshold);
            this.Controls.Add(this.txtCpuTempDanger);
            this.Controls.Add(this.txtCpuTempWarn);
            this.Controls.Add(this.txtAvailMemoryDanger);
            this.Controls.Add(this.txtAvailMemoryWarn);
            this.Controls.Add(this.txtCpuUsageDanger);
            this.Controls.Add(this.txtCpuUsageWarn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ThresholdForm";
            this.Text = "阈值设置";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCpuUsageWarn;
        private System.Windows.Forms.TextBox txtCpuUsageDanger;
        private System.Windows.Forms.TextBox txtAvailMemoryWarn;
        private System.Windows.Forms.TextBox txtAvailMemoryDanger;
        private System.Windows.Forms.TextBox txtCpuTempWarn;
        private System.Windows.Forms.TextBox txtCpuTempDanger;
        private System.Windows.Forms.TextBox txtVolatilityThreshold;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证并保存阈值设置
                double cpuUsageWarn = double.Parse(txtCpuUsageWarn.Text);
                double cpuUsageDanger = double.Parse(txtCpuUsageDanger.Text);
                double availMemoryWarn = double.Parse(txtAvailMemoryWarn.Text);
                double availMemoryDanger = double.Parse(txtAvailMemoryDanger.Text);
                double cpuTempWarn = double.Parse(txtCpuTempWarn.Text);
                double cpuTempDanger = double.Parse(txtCpuTempDanger.Text);
                double volatilityThreshold = double.Parse(txtVolatilityThreshold.Text);
                
                // 验证阈值的合理性
                if (cpuUsageWarn >= cpuUsageDanger)
                {
                    MessageBox.Show("CPU使用率警告阈值必须小于危险阈值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (availMemoryWarn <= availMemoryDanger)
                {
                    MessageBox.Show("可用内存警告阈值必须大于危险阈值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cpuTempWarn >= cpuTempDanger)
                {
                    MessageBox.Show("CPU温度警告阈值必须小于危险阈值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // 更新属性
                this.CpuUsageWarn = cpuUsageWarn;
                this.CpuUsageDanger = cpuUsageDanger;
                this.AvailMemoryWarn = availMemoryWarn;
                this.AvailMemoryDanger = availMemoryDanger;
                this.CpuTempWarn = cpuTempWarn;
                this.CpuTempDanger = cpuTempDanger;
                this.VolatilityThreshold = volatilityThreshold;
                
                // 设置对话框结果为OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入有效的数值: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}