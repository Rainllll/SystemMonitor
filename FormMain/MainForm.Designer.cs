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
        private System.Windows.Forms.Button btnTestAlert;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabControlPanel;
        private System.Windows.Forms.TabPage tabWarnings;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.TabPage tabScreen;
        private System.Windows.Forms.TabPage tabPieChart;
        private System.Windows.Forms.GroupBox gbWarningQuery;
        private System.Windows.Forms.Button btnQueryWarnings;
        private System.Windows.Forms.DateTimePicker dtpWarningEnd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpWarningStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbWarningData;
        private System.Windows.Forms.DataGridView dataGridViewWarnings;
        private System.Windows.Forms.GroupBox gbHistoryQuery;
        private System.Windows.Forms.Button btnQueryHistory;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbHistoryData;
        private System.Windows.Forms.DataGridView dataGridViewHistory;
        private System.Windows.Forms.GroupBox gbScreenSettings;
        private System.Windows.Forms.Button btnRefreshInterval;
        private System.Windows.Forms.TextBox txtRefreshInterval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gbScreenDisplay;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPie;
        private System.Windows.Forms.Label lblScreenTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblScreenTemp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblScreenMemory;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblScreenCpu;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox gbPieChartQuery;
        private System.Windows.Forms.Button btnQueryPieChart;
        private System.Windows.Forms.ComboBox cmbMetricType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpPieChartEnd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtpPieChartStart;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox gbPieChartDisplay;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTimeRangePie;
        private System.Windows.Forms.Label lblPieChartStats;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series seriesPieChart = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.btnSetInterval = new System.Windows.Forms.Button();
            this.lblInterval = new System.Windows.Forms.Label();
            this.btnSetThresholds = new System.Windows.Forms.Button();
            this.btnTestAlert = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabControlPanel = new System.Windows.Forms.TabPage();
            
            this.tabWarnings = new System.Windows.Forms.TabPage();
            this.gbWarningQuery = new System.Windows.Forms.GroupBox();
            this.btnQueryWarnings = new System.Windows.Forms.Button();
            this.dtpWarningEnd = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpWarningStart = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.gbWarningData = new System.Windows.Forms.GroupBox();
            this.dataGridViewWarnings = new System.Windows.Forms.DataGridView();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.gbHistoryQuery = new System.Windows.Forms.GroupBox();
            this.btnQueryHistory = new System.Windows.Forms.Button();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.gbHistoryData = new System.Windows.Forms.GroupBox();
            this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
            this.tabScreen = new System.Windows.Forms.TabPage();
            this.gbScreenSettings = new System.Windows.Forms.GroupBox();
            this.btnRefreshInterval = new System.Windows.Forms.Button();
            this.txtRefreshInterval = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gbScreenDisplay = new System.Windows.Forms.GroupBox();
            this.chartPie = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblScreenTime = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblScreenTemp = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblScreenMemory = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblScreenCpu = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabPieChart = new System.Windows.Forms.TabPage();
            this.gbPieChartQuery = new System.Windows.Forms.GroupBox();
            this.btnQueryPieChart = new System.Windows.Forms.Button();
            this.cmbMetricType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpPieChartEnd = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpPieChartStart = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.gbPieChartDisplay = new System.Windows.Forms.GroupBox();
            this.chartTimeRangePie = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblPieChartStats = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabControlPanel.SuspendLayout();
            this.tabWarnings.SuspendLayout();
            this.gbWarningQuery.SuspendLayout();
            this.gbWarningData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWarnings)).BeginInit();
            this.tabHistory.SuspendLayout();
            this.gbHistoryQuery.SuspendLayout();
            this.gbHistoryData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).BeginInit();
            this.tabScreen.SuspendLayout();
            this.gbScreenSettings.SuspendLayout();
            this.gbScreenDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPie)).BeginInit();
            this.tabPieChart.SuspendLayout();
            this.gbPieChartQuery.SuspendLayout();
            this.gbPieChartDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTimeRangePie)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 50);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(980, 620);
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
            // btnTestAlert
            // 
            this.btnTestAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestAlert.Location = new System.Drawing.Point(870, 680);
            this.btnTestAlert.Name = "btnTestAlert";
            this.btnTestAlert.Size = new System.Drawing.Size(120, 30);
            this.btnTestAlert.TabIndex = 8;
            this.btnTestAlert.Text = "测试报警";
            this.btnTestAlert.UseVisualStyleBackColor = true;
            this.btnTestAlert.Click += new System.EventHandler(this.btnTestAlert_Click);
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabControlPanel);
            this.tabControl.Controls.Add(this.tabWarnings);
            this.tabControl.Controls.Add(this.tabHistory);
            this.tabControl.Controls.Add(this.tabScreen);
            this.tabControl.Controls.Add(this.tabPieChart);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1024, 768);
            this.tabControl.TabIndex = 9;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabControlPanel
            // 
            this.tabControlPanel.Controls.Add(this.btnTestAlert);
            this.tabControlPanel.Controls.Add(this.btnSetThresholds);
            this.tabControlPanel.Controls.Add(this.btnSetInterval);
            this.tabControlPanel.Controls.Add(this.txtInterval);
            this.tabControlPanel.Controls.Add(this.lblInterval);
            this.tabControlPanel.Controls.Add(this.btnStop);
            this.tabControlPanel.Controls.Add(this.btnStart);
            this.tabControlPanel.Controls.Add(this.txtLog);
            this.tabControlPanel.Location = new System.Drawing.Point(4, 29);
            this.tabControlPanel.Name = "tabControlPanel";
            this.tabControlPanel.Padding = new System.Windows.Forms.Padding(3);
            this.tabControlPanel.Size = new System.Drawing.Size(1016, 735);
            this.tabControlPanel.TabIndex = 0;
            this.tabControlPanel.Text = "控制面板";
            this.tabControlPanel.UseVisualStyleBackColor = true;
            
            
            
            
            
            
            
            
            
            
            
            
            // 
            // tabWarnings
            // 
            this.tabWarnings.Controls.Add(this.gbWarningQuery);
            this.tabWarnings.Controls.Add(this.gbWarningData);
            this.tabWarnings.Location = new System.Drawing.Point(4, 29);
            this.tabWarnings.Name = "tabWarnings";
            this.tabWarnings.Padding = new System.Windows.Forms.Padding(3);
            this.tabWarnings.Size = new System.Drawing.Size(1016, 735);
            this.tabWarnings.TabIndex = 2;
            this.tabWarnings.Text = "预警查询";
            this.tabWarnings.UseVisualStyleBackColor = true;
            // 
            // gbWarningQuery
            // 
            this.gbWarningQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWarningQuery.Controls.Add(this.btnQueryWarnings);
            this.gbWarningQuery.Controls.Add(this.dtpWarningEnd);
            this.gbWarningQuery.Controls.Add(this.label9);
            this.gbWarningQuery.Controls.Add(this.dtpWarningStart);
            this.gbWarningQuery.Controls.Add(this.label8);
            this.gbWarningQuery.Location = new System.Drawing.Point(6, 6);
            this.gbWarningQuery.Name = "gbWarningQuery";
            this.gbWarningQuery.Size = new System.Drawing.Size(1004, 60);
            this.gbWarningQuery.TabIndex = 0;
            this.gbWarningQuery.TabStop = false;
            this.gbWarningQuery.Text = "查询条件";
            // 
            // btnQueryWarnings
            // 
            this.btnQueryWarnings.Location = new System.Drawing.Point(620, 20);
            this.btnQueryWarnings.Name = "btnQueryWarnings";
            this.btnQueryWarnings.Size = new System.Drawing.Size(100, 30);
            this.btnQueryWarnings.TabIndex = 4;
            this.btnQueryWarnings.Text = "查询";
            this.btnQueryWarnings.UseVisualStyleBackColor = true;
            this.btnQueryWarnings.Click += new System.EventHandler(this.btnQueryWarnings_Click);
            // 
            // dtpWarningEnd
            // 
            this.dtpWarningEnd.Location = new System.Drawing.Point(490, 25);
            this.dtpWarningEnd.Name = "dtpWarningEnd";
            this.dtpWarningEnd.Size = new System.Drawing.Size(120, 28);
            this.dtpWarningEnd.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(460, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 18);
            this.label9.TabIndex = 2;
            this.label9.Text = "至";
            // 
            // dtpWarningStart
            // 
            this.dtpWarningStart.Location = new System.Drawing.Point(330, 25);
            this.dtpWarningStart.Name = "dtpWarningStart";
            this.dtpWarningStart.Size = new System.Drawing.Size(120, 28);
            this.dtpWarningStart.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "时间范围:";
            // 
            // gbWarningData
            // 
            this.gbWarningData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWarningData.Controls.Add(this.dataGridViewWarnings);
            this.gbWarningData.Location = new System.Drawing.Point(6, 72);
            this.gbWarningData.Name = "gbWarningData";
            this.gbWarningData.Size = new System.Drawing.Size(1004, 657);
            this.gbWarningData.TabIndex = 1;
            this.gbWarningData.TabStop = false;
            this.gbWarningData.Text = "预警数据";
            // 
            // dataGridViewWarnings
            // 
            this.dataGridViewWarnings.AllowUserToAddRows = false;
            this.dataGridViewWarnings.AllowUserToDeleteRows = false;
            this.dataGridViewWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewWarnings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewWarnings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewWarnings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewWarnings.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewWarnings.Location = new System.Drawing.Point(10, 30);
            this.dataGridViewWarnings.Name = "dataGridViewWarnings";
            this.dataGridViewWarnings.ReadOnly = true;
            this.dataGridViewWarnings.RowTemplate.Height = 28;
            this.dataGridViewWarnings.Size = new System.Drawing.Size(984, 617);
            this.dataGridViewWarnings.TabIndex = 0;
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.gbHistoryQuery);
            this.tabHistory.Controls.Add(this.gbHistoryData);
            this.tabHistory.Location = new System.Drawing.Point(4, 29);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistory.Size = new System.Drawing.Size(1016, 735);
            this.tabHistory.TabIndex = 3;
            this.tabHistory.Text = "历史数据";
            this.tabHistory.UseVisualStyleBackColor = true;
            // 
            // gbHistoryQuery
            // 
            this.gbHistoryQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbHistoryQuery.Controls.Add(this.btnQueryHistory);
            this.gbHistoryQuery.Controls.Add(this.dtpEnd);
            this.gbHistoryQuery.Controls.Add(this.label2);
            this.gbHistoryQuery.Controls.Add(this.dtpStart);
            this.gbHistoryQuery.Controls.Add(this.label1);
            this.gbHistoryQuery.Location = new System.Drawing.Point(6, 6);
            this.gbHistoryQuery.Name = "gbHistoryQuery";
            this.gbHistoryQuery.Size = new System.Drawing.Size(1004, 60);
            this.gbHistoryQuery.TabIndex = 0;
            this.gbHistoryQuery.TabStop = false;
            this.gbHistoryQuery.Text = "查询条件";
            // 
            // btnQueryHistory
            // 
            this.btnQueryHistory.Location = new System.Drawing.Point(620, 20);
            this.btnQueryHistory.Name = "btnQueryHistory";
            this.btnQueryHistory.Size = new System.Drawing.Size(100, 30);
            this.btnQueryHistory.TabIndex = 4;
            this.btnQueryHistory.Text = "查询";
            this.btnQueryHistory.UseVisualStyleBackColor = true;
            this.btnQueryHistory.Click += new System.EventHandler(this.btnQueryHistory_Click);
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(490, 25);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(120, 28);
            this.dtpEnd.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(460, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "至";
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(330, 25);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(120, 28);
            this.dtpStart.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(250, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "时间范围:";
            // 
            // gbHistoryData
            // 
            this.gbHistoryData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbHistoryData.Controls.Add(this.dataGridViewHistory);
            this.gbHistoryData.Location = new System.Drawing.Point(6, 72);
            this.gbHistoryData.Name = "gbHistoryData";
            this.gbHistoryData.Size = new System.Drawing.Size(1004, 657);
            this.gbHistoryData.TabIndex = 1;
            this.gbHistoryData.TabStop = false;
            this.gbHistoryData.Text = "历史数据";
            // 
            // dataGridViewHistory
            // 
            this.dataGridViewHistory.AllowUserToAddRows = false;
            this.dataGridViewHistory.AllowUserToDeleteRows = false;
            this.dataGridViewHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistory.Location = new System.Drawing.Point(10, 30);
            this.dataGridViewHistory.Name = "dataGridViewHistory";
            this.dataGridViewHistory.ReadOnly = true;
            this.dataGridViewHistory.RowTemplate.Height = 28;
            this.dataGridViewHistory.Size = new System.Drawing.Size(984, 617);
            this.dataGridViewHistory.TabIndex = 0;
            // 
            // tabScreen
            // 
            this.tabScreen.Controls.Add(this.gbScreenSettings);
            this.tabScreen.Controls.Add(this.gbScreenDisplay);
            this.tabScreen.Location = new System.Drawing.Point(4, 29);
            this.tabScreen.Name = "tabScreen";
            this.tabScreen.Padding = new System.Windows.Forms.Padding(3);
            this.tabScreen.Size = new System.Drawing.Size(1016, 735);
            this.tabScreen.TabIndex = 4;
            this.tabScreen.Text = "实时监控";
            this.tabScreen.UseVisualStyleBackColor = true;
            // 
            // gbScreenSettings
            // 
            this.gbScreenSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbScreenSettings.Controls.Add(this.btnRefreshInterval);
            this.gbScreenSettings.Controls.Add(this.txtRefreshInterval);
            this.gbScreenSettings.Controls.Add(this.label7);
            this.gbScreenSettings.Location = new System.Drawing.Point(6, 6);
            this.gbScreenSettings.Name = "gbScreenSettings";
            this.gbScreenSettings.Size = new System.Drawing.Size(1004, 60);
            this.gbScreenSettings.TabIndex = 0;
            this.gbScreenSettings.TabStop = false;
            this.gbScreenSettings.Text = "设置";
            // 
            // btnRefreshInterval
            // 
            this.btnRefreshInterval.Location = new System.Drawing.Point(620, 20);
            this.btnRefreshInterval.Name = "btnRefreshInterval";
            this.btnRefreshInterval.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshInterval.TabIndex = 2;
            this.btnRefreshInterval.Text = "设置";
            this.btnRefreshInterval.UseVisualStyleBackColor = true;
            this.btnRefreshInterval.Click += new System.EventHandler(this.btnRefreshInterval_Click);
            // 
            // txtRefreshInterval
            // 
            this.txtRefreshInterval.Location = new System.Drawing.Point(490, 25);
            this.txtRefreshInterval.Name = "txtRefreshInterval";
            this.txtRefreshInterval.Size = new System.Drawing.Size(120, 28);
            this.txtRefreshInterval.TabIndex = 1;
            this.txtRefreshInterval.Text = "5";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(410, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "刷新间隔(秒):";
            // 
            // gbScreenDisplay
            // 
            this.gbScreenDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbScreenDisplay.Controls.Add(this.chartPie);
            this.gbScreenDisplay.Controls.Add(this.lblScreenTime);
            this.gbScreenDisplay.Controls.Add(this.label12);
            this.gbScreenDisplay.Controls.Add(this.lblScreenTemp);
            this.gbScreenDisplay.Controls.Add(this.label11);
            this.gbScreenDisplay.Controls.Add(this.lblScreenMemory);
            this.gbScreenDisplay.Controls.Add(this.label10);
            this.gbScreenDisplay.Controls.Add(this.lblScreenCpu);
            this.gbScreenDisplay.Controls.Add(this.label13);
            this.gbScreenDisplay.Location = new System.Drawing.Point(6, 72);
            this.gbScreenDisplay.Name = "gbScreenDisplay";
            this.gbScreenDisplay.Size = new System.Drawing.Size(1004, 657);
            this.gbScreenDisplay.TabIndex = 1;
            this.gbScreenDisplay.TabStop = false;
            this.gbScreenDisplay.Text = "实时监控数据";
            // 
            // chartPie
            // 
            this.chartPie.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea4.Name = "ChartArea1";
            this.chartPie.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartPie.Legends.Add(legend4);
            this.chartPie.Location = new System.Drawing.Point(10, 120);
            this.chartPie.Name = "chartPie";
            this.chartPie.Size = new System.Drawing.Size(984, 520);
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "CPU使用率";
            this.chartPie.Series.Add(series4);
            
            // 添加内存使用率系列
            var seriesMemory = new System.Windows.Forms.DataVisualization.Charting.Series();
            seriesMemory.ChartArea = "ChartArea1";
            seriesMemory.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            seriesMemory.Legend = "Legend1";
            seriesMemory.Name = "内存使用率";
            this.chartPie.Series.Add(seriesMemory);
            
            // 添加CPU温度系列
            var seriesTemp = new System.Windows.Forms.DataVisualization.Charting.Series();
            seriesTemp.ChartArea = "ChartArea1";
            seriesTemp.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            seriesTemp.Legend = "Legend1";
            seriesTemp.Name = "CPU温度";
            this.chartPie.Series.Add(seriesTemp);
            
            this.chartPie.TabIndex = 7;
            // 
            // lblScreenTime
            // 
            this.lblScreenTime.AutoSize = true;
            this.lblScreenTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScreenTime.Location = new System.Drawing.Point(850, 60);
            this.lblScreenTime.Name = "lblScreenTime";
            this.lblScreenTime.Size = new System.Drawing.Size(148, 22);
            this.lblScreenTime.TabIndex = 6;
            this.lblScreenTime.Text = "2023-01-01 12:00:00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(780, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 18);
            this.label12.TabIndex = 5;
            this.label12.Text = "更新时间:";
            // 
            // lblScreenTemp
            // 
            this.lblScreenTemp.AutoSize = true;
            this.lblScreenTemp.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScreenTemp.ForeColor = System.Drawing.Color.Red;
            this.lblScreenTemp.Location = new System.Drawing.Point(680, 30);
            this.lblScreenTemp.Name = "lblScreenTemp";
            this.lblScreenTemp.Size = new System.Drawing.Size(54, 22);
            this.lblScreenTemp.TabIndex = 4;
            this.lblScreenTemp.Text = "0.0 °C";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(610, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 18);
            this.label11.TabIndex = 3;
            this.label11.Text = "CPU温度:";
            // 
            // lblScreenMemory
            // 
            this.lblScreenMemory.AutoSize = true;
            this.lblScreenMemory.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScreenMemory.ForeColor = System.Drawing.Color.Blue;
            this.lblScreenMemory.Location = new System.Drawing.Point(490, 30);
            this.lblScreenMemory.Name = "lblScreenMemory";
            this.lblScreenMemory.Size = new System.Drawing.Size(66, 22);
            this.lblScreenMemory.TabIndex = 2;
            this.lblScreenMemory.Text = "0.0 MB";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(410, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 18);
            this.label10.TabIndex = 1;
            this.label10.Text = "可用内存:";
            // 
            // lblScreenCpu
            // 
            this.lblScreenCpu.AutoSize = true;
            this.lblScreenCpu.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScreenCpu.ForeColor = System.Drawing.Color.Green;
            this.lblScreenCpu.Location = new System.Drawing.Point(290, 30);
            this.lblScreenCpu.Name = "lblScreenCpu";
            this.lblScreenCpu.Size = new System.Drawing.Size(54, 22);
            this.lblScreenCpu.TabIndex = 0;
            this.lblScreenCpu.Text = "0.0%";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(210, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 18);
            this.label13.TabIndex = 0;
            this.label13.Text = "CPU使用率:";
            // 
            // tabPieChart
            // 
            this.tabPieChart.Controls.Add(this.gbPieChartQuery);
            this.tabPieChart.Controls.Add(this.gbPieChartDisplay);
            this.tabPieChart.Location = new System.Drawing.Point(4, 29);
            this.tabPieChart.Name = "tabPieChart";
            this.tabPieChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPieChart.Size = new System.Drawing.Size(1016, 735);
            this.tabPieChart.TabIndex = 5;
            this.tabPieChart.Text = "饼图分析";
            this.tabPieChart.UseVisualStyleBackColor = true;
            // 
            // gbPieChartQuery
            // 
            this.gbPieChartQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPieChartQuery.Controls.Add(this.btnQueryPieChart);
            this.gbPieChartQuery.Controls.Add(this.cmbMetricType);
            this.gbPieChartQuery.Controls.Add(this.label14);
            this.gbPieChartQuery.Controls.Add(this.dtpPieChartEnd);
            this.gbPieChartQuery.Controls.Add(this.label15);
            this.gbPieChartQuery.Controls.Add(this.dtpPieChartStart);
            this.gbPieChartQuery.Controls.Add(this.label16);
            this.gbPieChartQuery.Location = new System.Drawing.Point(6, 6);
            this.gbPieChartQuery.Name = "gbPieChartQuery";
            this.gbPieChartQuery.Size = new System.Drawing.Size(1004, 60);
            this.gbPieChartQuery.TabIndex = 0;
            this.gbPieChartQuery.TabStop = false;
            this.gbPieChartQuery.Text = "查询条件";
            // 
            // btnQueryPieChart
            // 
            this.btnQueryPieChart.Location = new System.Drawing.Point(630, 20);
            this.btnQueryPieChart.Name = "btnQueryPieChart";
            this.btnQueryPieChart.Size = new System.Drawing.Size(100, 30);
            this.btnQueryPieChart.TabIndex = 6;
            this.btnQueryPieChart.Text = "查询";
            this.btnQueryPieChart.UseVisualStyleBackColor = true;
            this.btnQueryPieChart.Click += new System.EventHandler(this.btnQueryPieChart_Click);
            // 
            // cmbMetricType
            // 
            this.cmbMetricType.FormattingEnabled = true;
            this.cmbMetricType.Items.AddRange(new object[] {
            "CPU使用率",
            "可用内存",
            "CPU温度"});
            this.cmbMetricType.Location = new System.Drawing.Point(500, 25);
            this.cmbMetricType.Name = "cmbMetricType";
            this.cmbMetricType.Size = new System.Drawing.Size(120, 28);
            this.cmbMetricType.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(410, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 18);
            this.label14.TabIndex = 4;
            this.label14.Text = "指标类型:";
            // 
            // dtpPieChartEnd
            // 
            this.dtpPieChartEnd.Location = new System.Drawing.Point(280, 25);
            this.dtpPieChartEnd.Name = "dtpPieChartEnd";
            this.dtpPieChartEnd.Size = new System.Drawing.Size(120, 28);
            this.dtpPieChartEnd.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(240, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 18);
            this.label15.TabIndex = 2;
            this.label15.Text = "至";
            // 
            // dtpPieChartStart
            // 
            this.dtpPieChartStart.Location = new System.Drawing.Point(110, 25);
            this.dtpPieChartStart.Name = "dtpPieChartStart";
            this.dtpPieChartStart.Size = new System.Drawing.Size(120, 28);
            this.dtpPieChartStart.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 18);
            this.label16.TabIndex = 0;
            this.label16.Text = "时间范围:";
            // 
            // gbPieChartDisplay
            // 
            this.gbPieChartDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPieChartDisplay.Controls.Add(this.chartTimeRangePie);
            this.gbPieChartDisplay.Controls.Add(this.lblPieChartStats);
            this.gbPieChartDisplay.Location = new System.Drawing.Point(6, 72);
            this.gbPieChartDisplay.Name = "gbPieChartDisplay";
            this.gbPieChartDisplay.Size = new System.Drawing.Size(1004, 657);
            this.gbPieChartDisplay.TabIndex = 1;
            this.gbPieChartDisplay.TabStop = false;
            this.gbPieChartDisplay.Text = "饼图展示";
            // 
            // chartTimeRangePie
            // 
            this.chartTimeRangePie.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea5.Name = "ChartArea1";
            this.chartTimeRangePie.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartTimeRangePie.Legends.Add(legend5);
            this.chartTimeRangePie.Location = new System.Drawing.Point(10, 40);
            this.chartTimeRangePie.Name = "chartTimeRangePie";
            this.chartTimeRangePie.Size = new System.Drawing.Size(984, 500);
            seriesPieChart.ChartArea = "ChartArea1";
            seriesPieChart.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            seriesPieChart.Legend = "Legend1";
            seriesPieChart.Name = "数据分布";
            this.chartTimeRangePie.Series.Add(seriesPieChart);
            this.chartTimeRangePie.TabIndex = 1;
            // 
            // lblPieChartStats
            // 
            this.lblPieChartStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPieChartStats.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPieChartStats.Location = new System.Drawing.Point(10, 550);
            this.lblPieChartStats.Name = "lblPieChartStats";
            this.lblPieChartStats.Size = new System.Drawing.Size(984, 100);
            this.lblPieChartStats.TabIndex = 0;
            this.lblPieChartStats.Text = "统计信息";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "系统监控程序";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabControlPanel.ResumeLayout(false);
            this.tabControlPanel.PerformLayout();
            this.tabWarnings.ResumeLayout(false);
            this.gbWarningQuery.ResumeLayout(false);
            this.gbWarningQuery.PerformLayout();
            this.gbWarningData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWarnings)).EndInit();
            this.tabHistory.ResumeLayout(false);
            this.gbHistoryQuery.ResumeLayout(false);
            this.gbHistoryQuery.PerformLayout();
            this.gbHistoryData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).EndInit();
            this.tabScreen.ResumeLayout(false);
            this.gbScreenSettings.ResumeLayout(false);
            this.gbScreenSettings.PerformLayout();
            this.gbScreenDisplay.ResumeLayout(false);
            this.gbScreenDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPie)).EndInit();
            this.tabPieChart.ResumeLayout(false);
            this.gbPieChartQuery.ResumeLayout(false);
            this.gbPieChartQuery.PerformLayout();
            this.gbPieChartDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTimeRangePie)).EndInit();
            this.ResumeLayout(false);

        }
    }
}