using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using LibreHardwareMonitor.Hardware;
using MySqlConnector;//右键项目 → 管理 NuGet 包 → 搜索并安装 MySqlConnector
using System.Windows.Forms.DataVisualization.Charting;

namespace FormMain
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer _timer;
        private int _intervalSeconds = 1; // 默认采集周期 1s
        private string _logPath = @"D:\data_log.txt"; // 日志文件路径
        private PerformanceCounter cpuCounter;
        private Computer computer;

        // 数据库连接字符串（从App.config中读取）
        private string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        
        // 阈值配置
        private double _cpuUsageWarn = 80.0;  // CPU使用率警告阈值
        private double _cpuUsageDanger = 90.0; // CPU使用率危险阈值
        private double _availMemoryWarn = 500.0; // 可用内存警告阈值(MB)
        private double _availMemoryDanger = 100.0; // 可用内存危险阈值(MB)
        private double _cpuTempWarn = 85.0;  // CPU温度警告阈值(°C)
        private double _cpuTempDanger = 95.0; // CPU温度危险阈值(°C)
        private double _volatilityThreshold = 20.0; // 波动率阈值(%)
        
        // 历史数据缓存
        private Queue<double> _cpuUsageHistory = new Queue<double>(10);
        private Queue<double> _availMemoryHistory = new Queue<double>(10);
        private Queue<double> _cpuTempHistory = new Queue<double>(10);
        
        // 用于折线图的数据存储
        private Queue<DateTime> _timeHistory = new Queue<DateTime>(60); // 保存60个时间点
        private Queue<double> _cpuUsageLineData = new Queue<double>(60); // 保存60个CPU使用率数据点
        private Queue<double> _memoryUsageLineData = new Queue<double>(60); // 保存60个内存使用率数据点
        private Queue<double> _cpuTempLineData = new Queue<double>(60); // 保存60个CPU温度数据点
        
        
        

        public MainForm()
        {
            Logger.Log("MainForm构造函数开始执行");
            InitializeComponent();
            Logger.Log("InitializeComponent执行完成");

            // 初始化 CPU 使用率计数器
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500); // 预热

            // 初始化 LibreHardwareMonitor
            computer = new Computer() { IsCpuEnabled = true }; // 注意：IsCpuEnabled
            computer.Open();
            
            // 初始化定时器
            InitTimer();
        }

        private void InitTimer()
        {
            _timer = new System.Timers.Timer(_intervalSeconds * 1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }



        





        

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CollectAndSaveData();
        }

        

        private void CollectAndSaveData()
        {
            try
            {
                // CPU 使用率
                float cpuUsage = cpuCounter.NextValue();

                // 可用内存
                var memCounter = new PerformanceCounter("Memory", "Available MBytes");
                float availableMemory = memCounter.NextValue();//单位是MB

                // CPU 温度
                double cpuTemp = -1;
                foreach (IHardware hardware in computer.Hardware)
                {
                    if (hardware.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.Cpu) // 注意：Cpu
                    {
                        hardware.Update();
                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature)
                            {
                                cpuTemp = sensor.Value.GetValueOrDefault();
                                break; // 只取第一个温度传感器
                            }
                        }
                    }
                    if (cpuTemp >= 0) break;
                }

                string log = String.Format("{0}: CPU={1:F2}%, 可用内存={2}MB, CPU温度={3:F1}°C", 
                    DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), cpuUsage, availableMemory, cpuTemp);
                Log(log);

                // 保存到数据库（在其他电脑可改为local-pc-02）
                bool saveSuccess = SaveToDatabase("local-pc-01", DateTime.Now, cpuUsage, availableMemory, cpuTemp);
                if (!saveSuccess)
                {
                    Log("数据保存到数据库失败");
                }

                // 添加到历史数据
                UpdateHistoryData(cpuUsage, availableMemory, cpuTemp);
                
                // 检查预警条件
                CheckAlerts("local-pc-01", DateTime.Now, cpuUsage, availableMemory, cpuTemp);

                // 更新 UI
                if (!this.IsDisposed && this.IsHandleCreated)
                {
                    this.Invoke(new Action(() =>
                    {
                        txtLog.AppendText(log + Environment.NewLine);
                        
                        // 更新实时数据显示
                        UpdateRealTimeDisplay((float)cpuUsage, (float)availableMemory, (float)cpuTemp);
                    }));
                }
            }
            catch (Exception ex)
            {
                Log("采集错误: " + ex.Message);

                ////作用：在UI界面上显示错误信息
                //string errorMsg = $"采集错误: {ex.Message}";
                //Log(errorMsg);
                //this.Invoke(new Action(() =>
                //{
                //    txtLog.AppendText(errorMsg + Environment.NewLine);
                //}));

            }
        }

        // 将数据保存到MySQL数据库
        private bool SaveToDatabase(string deviceId, DateTime collectTime, double cpuUsage, double availableMemory, double cpuTemp)
        {
            // SQL插入语句
            string sql = @"
                INSERT INTO t_metrics 
                (device_id, collect_time, cpu_usage, avail_memory, cpu_temp)
                VALUES 
                (@deviceId, @collectTime, @cpuUsage, @availableMemory, @cpuTemp)";
            /*
             string sql = @"
                INSERT INTO realtime_metrics 
                (device_id, collect_time, cpu_usage, available_mem, cpu_temp, status)
                VALUES 
                (@deviceId, @collectTime, @cpuUsage, @availableMemory, @cpuTemp, 1)";
             */

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        // 添加参数（防止SQL注入）
                        command.Parameters.AddWithValue("@deviceId", deviceId);
                        command.Parameters.AddWithValue("@collectTime", collectTime);
                        command.Parameters.AddWithValue("@cpuUsage", (float)cpuUsage);
                        command.Parameters.AddWithValue("@availableMemory", (float)availableMemory);
                        command.Parameters.AddWithValue("@cpuTemp", (float)cpuTemp);


                        // 执行插入并返回是否成功，如果 rowsAffected > 0 为 true（例如等于 1），说明有至少一行数据被成功插入，方法返回 true
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Log(String.Format("数据库错误: {0}", ex.Message));
                    return false;
                }
            }
        }
        // 保存预警信息到数据库
        private bool SaveAlertToDatabase(string deviceId, DateTime warningTime, string warningContent, 
                                         string metricType, double metricValue, string thresholdType, 
                                         double thresholdValue)
        {
            string sql = @"
                INSERT INTO t_warning 
                (device_id, warning_time, warning_content, metric_type, metric_value, 
                threshold_type, threshold_value, processing_status)
                VALUES 
                (@deviceId, @warningTime, @warningContent, @metricType, @metricValue, 
                @thresholdType, @thresholdValue, 'UNHANDLED')";

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@deviceId", deviceId);
                        command.Parameters.AddWithValue("@warningTime", warningTime);
                        command.Parameters.AddWithValue("@warningContent", warningContent);
                        command.Parameters.AddWithValue("@metricType", metricType);
                        command.Parameters.AddWithValue("@metricValue", (float)metricValue);
                        command.Parameters.AddWithValue("@thresholdType", thresholdType);
                        command.Parameters.AddWithValue("@thresholdValue", (float)thresholdValue);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Log(String.Format("预警信息保存失败: {0}", ex.Message));
                    return false;
                }
            }
        }
        
        // 更新历史数据
        private void UpdateHistoryData(double cpuUsage, double availableMemory, double cpuTemp)
        {
            const int historySize = 10;
            const int lineDataSize = 60; // 折线图数据点数量
            
            // 更新CPU使用率历史
            if (_cpuUsageHistory.Count >= historySize)
                _cpuUsageHistory.Dequeue();
            _cpuUsageHistory.Enqueue(cpuUsage);
            
            // 更新可用内存历史
            if (_availMemoryHistory.Count >= historySize)
                _availMemoryHistory.Dequeue();
            _availMemoryHistory.Enqueue(availableMemory);
            
            // 更新CPU温度历史
            if (_cpuTempHistory.Count >= historySize && cpuTemp >= 0)
                _cpuTempHistory.Dequeue();
            if (cpuTemp >= 0)
                _cpuTempHistory.Enqueue(cpuTemp);
            
            // 更新折线图数据
            DateTime now = DateTime.Now;
            
            // 更新时间历史
            if (_timeHistory.Count >= lineDataSize)
                _timeHistory.Dequeue();
            _timeHistory.Enqueue(now);
            
            // 更新CPU使用率折线图数据
            if (_cpuUsageLineData.Count >= lineDataSize)
                _cpuUsageLineData.Dequeue();
            _cpuUsageLineData.Enqueue(cpuUsage);
            
            // 更新内存使用率折线图数据（计算内存使用率）
            double totalMemory = 8192; // 8GB
            double memoryUsage = ((totalMemory - availableMemory) / totalMemory) * 100;
            if (_memoryUsageLineData.Count >= lineDataSize)
                _memoryUsageLineData.Dequeue();
            _memoryUsageLineData.Enqueue(memoryUsage);
            
            // 更新CPU温度折线图数据
            if (cpuTemp >= 0)
            {
                if (_cpuTempLineData.Count >= lineDataSize)
                    _cpuTempLineData.Dequeue();
                _cpuTempLineData.Enqueue(cpuTemp);
            }
        }
        
        // 计算波动率
        private double CalculateVolatility(Queue<double> history, double currentValue)
        {
            if (history.Count < 2 || currentValue == 0)
                return 0;
            
            double previousValue = history.Last();
            double change = Math.Abs(currentValue - previousValue);
            double volatility = (change / previousValue) * 100;
            
            return volatility;
        }
        
        // 检查预警条件
        private void CheckAlerts(string deviceId, DateTime collectTime, double cpuUsage, double availableMemory, double cpuTemp)
        {
            // 检查CPU使用率
            if (cpuUsage >= _cpuUsageDanger)
            {
                string content = String.Format("CPU使用率过高({0:F2}%), 达到危险阈值({1}%)", cpuUsage, _cpuUsageDanger);
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU使用率", cpuUsage, "DANGER", _cpuUsageDanger);
                LogWarning(content);
            }
            else if (cpuUsage >= _cpuUsageWarn)
            {
                string content = String.Format("CPU使用率较高({0:F2}%), 达到警告阈值({1}%)", cpuUsage, _cpuUsageWarn);
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU使用率", cpuUsage, "WARN", _cpuUsageWarn);
                LogWarning(content);
            }
            
            // 检查可用内存
            if (availableMemory <= _availMemoryDanger)
            {
                string content = String.Format("可用内存过低({0:F2}MB), 达到危险阈值({1}MB)", availableMemory, _availMemoryDanger);
                SaveAlertToDatabase(deviceId, collectTime, content, "可用内存", availableMemory, "DANGER", _availMemoryDanger);
                LogWarning(content);
            }
            else if (availableMemory <= _availMemoryWarn)
            {
                string content = String.Format("可用内存较低({0:F2}MB), 达到警告阈值({1}MB)", availableMemory, _availMemoryWarn);
                SaveAlertToDatabase(deviceId, collectTime, content, "可用内存", availableMemory, "WARN", _availMemoryWarn);
                LogWarning(content);
            }
            
            // 检查CPU温度
            if (cpuTemp >= _cpuTempDanger)
            {
                string content = String.Format("CPU温度过高({0:F2}°C), 达到危险阈值({1}°C)", cpuTemp, _cpuTempDanger);
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU温度", cpuTemp, "DANGER", _cpuTempDanger);
                LogWarning(content);
            }
            else if (cpuTemp >= _cpuTempWarn)
            {
                string content = String.Format("CPU温度较高({0:F2}°C), 达到警告阈值({1}°C)", cpuTemp, _cpuTempWarn);
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU温度", cpuTemp, "WARN", _cpuTempWarn);
                LogWarning(content);
            }
            
            // 检查波动率预警
            if (_cpuUsageHistory.Count >= 2)
            {
                double cpuVolatility = CalculateVolatility(_cpuUsageHistory, cpuUsage);
                if (cpuVolatility >= _volatilityThreshold)
                {
                    string content = String.Format("CPU使用率波动较大({0:F2}%), 超过阈值({1}%)", cpuVolatility, _volatilityThreshold);
                    SaveAlertToDatabase(deviceId, collectTime, content, "CPU使用率波动率", cpuVolatility, "WARN", _volatilityThreshold);
                    LogWarning(content);
                }
            }
        }
        
        // 记录警告信息
        private void LogWarning(string message)
        {
            string warningMsg = String.Format("[{0}] 警告: {1}", DateTime.Now.ToString("HH:mm:ss"), message);
            Log(warningMsg);
            
            // 在UI上显示警告
            if (!this.IsDisposed && this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    txtLog.AppendText(warningMsg + Environment.NewLine);
                    // 高亮显示警告信息
                    int startPos = txtLog.Text.LastIndexOf(warningMsg);
                    if (startPos >= 0)
                    {
                        txtLog.SelectionStart = startPos;
                        txtLog.SelectionLength = warningMsg.Length;
                        txtLog.SelectionColor = Color.Red;
                        txtLog.SelectionStart = txtLog.Text.Length;
                        txtLog.SelectionColor = txtLog.ForeColor;
                    }
                    
                    // 弹出报警提示框，5秒后自动关闭
                    ShowAutoClosingMessageBox(message, "系统报警", 5000);
                }));
            }
        }
        
        // 显示自动关闭的消息框
        private void ShowAutoClosingMessageBox(string message, string caption, int timeout)
        {
            AutoClosingMessageBox.Show(message, caption, timeout);
        }
        
        // 从数据库加载阈值配置
        private void LoadThresholdsFromDatabase()
        {
            string sql = "SELECT cpu_usage_warn, cpu_usage_danger, avail_memory_warn, avail_memory_danger, cpu_temp_warn, cpu_temp_danger FROM t_sys_metadata WHERE device_id = @deviceId";
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@deviceId", "local-pc-01");
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                _cpuUsageWarn = reader.GetDouble(0);
                                _cpuUsageDanger = reader.GetDouble(1);
                                _availMemoryWarn = reader.GetDouble(2);
                                _availMemoryDanger = reader.GetDouble(3);
                                _cpuTempWarn = reader.GetDouble(4);
                                _cpuTempDanger = reader.GetDouble(5);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(String.Format("加载阈值配置失败: {0}", ex.Message));
                }
            }
        }
        
        private void Log(string message)
        {
            try
            {
                File.AppendAllText(_logPath, message + Environment.NewLine);
            }
            //catch { }
            catch (Exception ex)
            {
                //弹出对话框
                MessageBox.Show(String.Format("日志写入失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //_timer.Start();
            //Log("数据采集已启动");
            //这里为了在数据采集过程中在 WinForm 界面的 txtLog 文本框中，追加显示 "数据采集已启动" 这条信息
            _timer.Start();
            string log = "数据采集已启动";
            Log(log);
            txtLog.AppendText(log + Environment.NewLine);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //_timer.Stop();
            //Log("数据采集已停止");
            _timer.Stop();
            string log = "数据采集已停止";
            Log(log);
            txtLog.AppendText(log + Environment.NewLine);
        }

        private void btnSetInterval_Click(object sender, EventArgs e)
        {
            int seconds;
            if (int.TryParse(txtInterval.Text, out seconds) && seconds >= 1)
            {
                _intervalSeconds = seconds;
                _timer.Interval = _intervalSeconds * 1000;
                string log = String.Format("采集周期已修改为 {0} 秒", seconds);
                Log(log);
                txtLog.AppendText(log + Environment.NewLine);
            }
            else
            {
                MessageBox.Show("请输入大于等于1的整数秒", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        // 设置阈值按钮点击事件
        private void btnSetThresholds_Click(object sender, EventArgs e)
        {
            ThresholdForm thresholdForm = new ThresholdForm(_cpuUsageWarn, _cpuUsageDanger, 
                                                          _availMemoryWarn, _availMemoryDanger, 
                                                          _cpuTempWarn, _cpuTempDanger, 
                                                          _volatilityThreshold);
            
            if (thresholdForm.ShowDialog() == DialogResult.OK)
            {
                // 更新阈值
                _cpuUsageWarn = thresholdForm.CpuUsageWarn;
                _cpuUsageDanger = thresholdForm.CpuUsageDanger;
                _availMemoryWarn = thresholdForm.AvailMemoryWarn;
                _availMemoryDanger = thresholdForm.AvailMemoryDanger;
                _cpuTempWarn = thresholdForm.CpuTempWarn;
                _cpuTempDanger = thresholdForm.CpuTempDanger;
                _volatilityThreshold = thresholdForm.VolatilityThreshold;
                
                // 保存阈值到数据库
                SaveThresholdsToDatabase();
                
                string log = "阈值配置已更新";
                Log(log);
                txtLog.AppendText(log + Environment.NewLine);
            }
        }
        
        // 实时监控刷新间隔设置按钮点击事件
        private void btnRefreshInterval_Click(object sender, EventArgs e)
        {
            int seconds;
            if (int.TryParse(txtRefreshInterval.Text, out seconds) && seconds >= 1)
            {
                // 更新定时器间隔
                _intervalSeconds = seconds;
                _timer.Interval = _intervalSeconds * 1000;
                
                string log = String.Format("实时监控刷新间隔已修改为 {0} 秒", seconds);
                Log(log);
                txtLog.AppendText(log + Environment.NewLine);
                
                // 如果当前在实时监控标签页，立即更新一次数据显示
                if (tabControl.SelectedTab == tabScreen)
                {
                    // 获取最新的系统指标数据
                    float cpuUsage = cpuCounter.NextValue();
                    var memCounter = new PerformanceCounter("Memory", "Available MBytes");
                    float availMemory = memCounter.NextValue();
                    
                    float cpuTemp = 0;
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.Cpu);
                    if (cpu != null)
                    {
                        cpu.Update();
                        var tempSensor = cpu.Sensors.FirstOrDefault(s => s.SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature);
                        if (tempSensor != null)
                        {
                            cpuTemp = (float)tempSensor.Value;
                        }
                    }
                    
                    // 更新实时数据显示
                    UpdateRealTimeDisplay(cpuUsage, availMemory, cpuTemp);
                }
            }
            else
            {
                MessageBox.Show("请输入大于等于1的整数秒", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        // 预警查询按钮点击事件
        private void btnQueryWarnings_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startTime = dtpWarningStart.Value;
                DateTime endTime = dtpWarningEnd.Value;
                
                if (startTime > endTime)
                {
                    MessageBox.Show("开始时间不能大于结束时间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 查询预警数据
                string sql = "SELECT warning_time, warning_content, metric_type, metric_value, threshold_type, threshold_value " +
                             "FROM t_warning " +
                             "WHERE warning_time BETWEEN @startTime AND @endTime " +
                             "ORDER BY warning_time DESC";
                
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@startTime", startTime);
                        command.Parameters.AddWithValue("@endTime", endTime);
                        
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            
                            // 绑定到DataGridView
                            dataGridViewWarnings.DataSource = dt;
                            
                            // 修改列标题为中文
                            if (dataGridViewWarnings.Columns.Count > 0)
                            {
                                dataGridViewWarnings.Columns["warning_time"].HeaderText = "预警时间";
                                dataGridViewWarnings.Columns["warning_content"].HeaderText = "预警内容";
                                dataGridViewWarnings.Columns["metric_type"].HeaderText = "指标类型";
                                dataGridViewWarnings.Columns["metric_value"].HeaderText = "当前值";
                                dataGridViewWarnings.Columns["threshold_type"].HeaderText = "预警级别";
                                dataGridViewWarnings.Columns["threshold_value"].HeaderText = "阈值";
                            }
                            
                            string log = String.Format("查询到 {0} 条预警记录", dt.Rows.Count);
                            Log(log);
                            txtLog.AppendText(log + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("查询预警数据失败: {0}", ex.Message));
                MessageBox.Show(String.Format("查询预警数据失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // 历史数据查询按钮点击事件
        private void btnQueryHistory_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startTime = dtpStart.Value;
                DateTime endTime = dtpEnd.Value;
                
                if (startTime > endTime)
                {
                    MessageBox.Show("开始时间不能大于结束时间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 查询历史数据
                string sql = "SELECT collect_time, cpu_usage, avail_memory, cpu_temp " +
                             "FROM t_metrics " +
                             "WHERE collect_time BETWEEN @startTime AND @endTime " +
                             "ORDER BY collect_time DESC";
                
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@startTime", startTime);
                        command.Parameters.AddWithValue("@endTime", endTime);
                        
                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            
                            // 绑定到DataGridView
                            dataGridViewHistory.DataSource = dt;
                            
                            // 修改列标题为中文
                            if (dataGridViewHistory.Columns.Count > 0)
                            {
                                dataGridViewHistory.Columns["collect_time"].HeaderText = "采集时间";
                                dataGridViewHistory.Columns["cpu_usage"].HeaderText = "CPU使用率(%)";
                                dataGridViewHistory.Columns["avail_memory"].HeaderText = "可用内存(MB)";
                                dataGridViewHistory.Columns["cpu_temp"].HeaderText = "CPU温度(°C)";
                            }
                            
                            string log = String.Format("查询到 {0} 条历史记录", dt.Rows.Count);
                            Log(log);
                            txtLog.AppendText(log + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("查询历史数据失败: {0}", ex.Message));
                MessageBox.Show(String.Format("查询历史数据失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // 饼图查询按钮点击事件
        private void btnQueryPieChart_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startTime = dtpPieChartStart.Value;
                DateTime endTime = dtpPieChartEnd.Value;
                string metricType = cmbMetricType.SelectedItem?.ToString() ?? "CPU使用率";
                
                if (startTime > endTime)
                {
                    MessageBox.Show("开始时间不能大于结束时间", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 根据指标类型查询数据
                string sql = "";
                string valueColumn = "";
                
                switch (metricType)
                {
                    case "CPU使用率":
                        valueColumn = "cpu_usage";
                        break;
                    case "可用内存":
                        valueColumn = "avail_memory";
                        break;
                    case "CPU温度":
                        valueColumn = "cpu_temp";
                        break;
                    default:
                        valueColumn = "cpu_usage";
                        break;
                }
                
                sql = String.Format("SELECT {0} FROM t_metrics WHERE collect_time BETWEEN @startTime AND @endTime ORDER BY collect_time DESC", valueColumn);
                
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@startTime", startTime);
                        command.Parameters.AddWithValue("@endTime", endTime);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            // 收集所有数据值
                            List<double> values = new List<double>();
                            double sum = 0;
                            double max = double.MinValue;
                            double min = double.MaxValue;
                            int count = 0;
                            
                            while (reader.Read())
                            {
                                double value = reader.GetDouble(0);
                                values.Add(value);
                                sum += value;
                                max = Math.Max(max, value);
                                min = Math.Min(min, value);
                                count++;
                            }
                            
                            if (count > 0)
                            {
                                double avg = sum / count;
                                
                                // 更新统计信息标签
                                lblPieChartStats.Text = String.Format("{0} 统计信息 (时间范围: {1} 至 {2})\n平均值: {3:F2}\n最大值: {4:F2}\n最小值: {5:F2}\n数据点数: {6}", 
                                    metricType, startTime.ToString("yyyy-MM-dd HH:mm"), endTime.ToString("yyyy-MM-dd HH:mm"), 
                                    avg, max, min, count);
                                
                                // 更新饼图 - 传递所有数据值和指标类型
                                UpdateTimeRangePieChart(values, metricType);
                                
                                string log = String.Format("生成{0}饼图，基于{1}个数据点", metricType, count);
                                Log(log);
                                txtLog.AppendText(log + Environment.NewLine);
                            }
                            else
                            {
                                MessageBox.Show("指定时间范围内没有数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("生成饼图失败: {0}", ex.Message));
                MessageBox.Show(String.Format("生成饼图失败: {0}", ex.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // 更新时间范围饼图
        private void UpdateTimeRangePieChart(List<double> values, string metricType)
        {
            try
            {
                // 确保在UI线程上更新控件
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<List<double>, string>(UpdateTimeRangePieChart), values, metricType);
                    return;
                }
                
                // 如果图表控件不存在，则返回
                if (chartTimeRangePie == null) return;
                
                // 清空现有数据
                chartTimeRangePie.Series["数据分布"].Points.Clear();
                
                // 根据指标类型进行数据分段
                Dictionary<string, int> segments = new Dictionary<string, int>();
                double total = values.Count;
                
                if (metricType == "CPU使用率")
                {
                    // CPU使用率分段：0-30%, 30-60%, 60-90%, 90%以上
                    segments.Add("低使用率 (0-30%)", values.Count(v => v >= 0 && v < 30));
                    segments.Add("中等使用率 (30-60%)", values.Count(v => v >= 30 && v < 60));
                    segments.Add("高使用率 (60-90%)", values.Count(v => v >= 60 && v < 90));
                    segments.Add("极高使用率 (90%+)", values.Count(v => v >= 90));
                }
                else if (metricType == "可用内存")
                {
                    // 可用内存分段：根据数据范围动态分段
                    double max = values.Max();
                    double min = values.Min();
                    double range = max - min;
                    double segmentSize = range / 4;
                    
                    segments.Add("低可用内存", values.Count(v => v >= min && v < min + segmentSize));
                    segments.Add("中低可用内存", values.Count(v => v >= min + segmentSize && v < min + 2 * segmentSize));
                    segments.Add("中高可用内存", values.Count(v => v >= min + 2 * segmentSize && v < min + 3 * segmentSize));
                    segments.Add("高可用内存", values.Count(v => v >= min + 3 * segmentSize && v <= max));
                }
                else if (metricType == "CPU温度")
                {
                    // CPU温度分段：低温、正常、高温、过热
                    segments.Add("低温 (<40°C)", values.Count(v => v < 40));
                    segments.Add("正常 (40-60°C)", values.Count(v => v >= 40 && v < 60));
                    segments.Add("高温 (60-80°C)", values.Count(v => v >= 60 && v < 80));
                    segments.Add("过热 (80°C+)", values.Count(v => v >= 80));
                }
                
                // 添加数据点到饼图
                foreach (var segment in segments)
                {
                    if (segment.Value > 0)  // 只添加有数据的分段
                    {
                        double percentage = (segment.Value / total) * 100;
                        chartTimeRangePie.Series["数据分布"].Points.AddXY(segment.Key, segment.Value);
                        
                        // 设置标签显示百分比
                        int pointIndex = chartTimeRangePie.Series["数据分布"].Points.Count - 1;
                        chartTimeRangePie.Series["数据分布"].Points[pointIndex].Label = string.Format("{0:F1}%", percentage);
                        chartTimeRangePie.Series["数据分布"].Points[pointIndex].LegendText = segment.Key;
                    }
                }
                
                // 设置图表标题
                chartTimeRangePie.Titles.Clear();
                chartTimeRangePie.Titles.Add(string.Format("{0}分布占比", metricType));
                
                // 设置图例显示
                chartTimeRangePie.Legends[0].Enabled = true;
                chartTimeRangePie.Legends[0].Title = metricType + "分段";
            }
            catch (Exception ex)
            {
                Log(String.Format("更新时间范围饼图失败: {0}", ex.Message));
            }
        }
        
        // 保存阈值到数据库
        private void SaveThresholdsToDatabase()
        {
            string sql = "UPDATE t_sys_metadata SET cpu_usage_warn = @cpuUsageWarn, cpu_usage_danger = @cpuUsageDanger, " +
                         "avail_memory_warn = @availMemoryWarn, avail_memory_danger = @availMemoryDanger, " +
                         "cpu_temp_warn = @cpuTempWarn, cpu_temp_danger = @cpuTempDanger WHERE device_id = @deviceId";
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@cpuUsageWarn", _cpuUsageWarn);
                        command.Parameters.AddWithValue("@cpuUsageDanger", _cpuUsageDanger);
                        command.Parameters.AddWithValue("@availMemoryWarn", _availMemoryWarn);
                        command.Parameters.AddWithValue("@availMemoryDanger", _availMemoryDanger);
                        command.Parameters.AddWithValue("@cpuTempWarn", _cpuTempWarn);
                        command.Parameters.AddWithValue("@cpuTempDanger", _cpuTempDanger);
                        command.Parameters.AddWithValue("@deviceId", "local-pc-01");
                        
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log(String.Format("保存阈值配置失败: {0}", ex.Message));
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (computer != null)
                computer.Close();
        }

        
        
        
        
        

        private void btnTestAlert_Click(object sender, EventArgs e)
        {
            // 模拟CPU使用率过高报警
            LogWarning("CPU使用率过高(95.00%), 达到危险阈值(90%)");
        }

        

        

        

        

        

        

        
        
        
        

        

        

        

        

        

        

        
        
        

        

        

        


        // 更新实时数据显示
        private void UpdateRealTimeDisplay(float cpuUsage, float availMemory, float cpuTemp)
        {
            try
            {
                // 确保在UI线程上更新控件
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<float, float, float>(UpdateRealTimeDisplay), cpuUsage, availMemory, cpuTemp);
                    return;
                }
                
                // 更新实时监控标签页的数据
                if (lblScreenCpu != null) lblScreenCpu.Text = String.Format("{0:F1}%", cpuUsage);
                if (lblScreenMemory != null) lblScreenMemory.Text = String.Format("{0:F1} MB", availMemory);
                if (lblScreenTemp != null) lblScreenTemp.Text = String.Format("{0:F1} °C", cpuTemp);
                if (lblScreenTime != null) lblScreenTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                // 更新饼图
                UpdatePieChart(cpuUsage, availMemory, cpuTemp);
            }
            catch (Exception ex)
            {
                Log(String.Format("更新实时显示失败: {0}", ex.Message));
            }
        }
        
        // 更新折线图
        private void UpdatePieChart(float cpuUsage, float availMemory, float cpuTemp)
        {
            try
            {
                // 确保在UI线程上更新控件
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<float, float, float>(UpdatePieChart), cpuUsage, availMemory, cpuTemp);
                    return;
                }
                
                // 如果图表控件不存在，则返回
                if (chartPie == null) return;
                
                // 清空现有数据
                chartPie.Series["CPU使用率"].Points.Clear();
                chartPie.Series["内存使用率"].Points.Clear();
                chartPie.Series["CPU温度"].Points.Clear();
                
                // 添加数据点到折线图
                if (_timeHistory.Count > 0 && _cpuUsageLineData.Count > 0)
                {
                    // 将队列转换为数组以便遍历
                    DateTime[] timeArray = _timeHistory.ToArray();
                    double[] cpuUsageArray = _cpuUsageLineData.ToArray();
                    double[] memoryUsageArray = _memoryUsageLineData.ToArray();
                    double[] cpuTempArray = _cpuTempLineData.ToArray();
                    
                    // 添加CPU使用率数据点
                    for (int i = 0; i < timeArray.Length; i++)
                    {
                        chartPie.Series["CPU使用率"].Points.AddXY(timeArray[i], cpuUsageArray[i]);
                    }
                    
                    // 添加内存使用率数据点
                    for (int i = 0; i < timeArray.Length; i++)
                    {
                        chartPie.Series["内存使用率"].Points.AddXY(timeArray[i], memoryUsageArray[i]);
                    }
                    
                    // 添加CPU温度数据点
                    for (int i = 0; i < timeArray.Length; i++)
                    {
                        chartPie.Series["CPU温度"].Points.AddXY(timeArray[i], cpuTempArray[i]);
                    }
                    
                    // 设置图表区域
                    chartPie.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm:ss";
                    chartPie.ChartAreas["ChartArea1"].AxisX.Title = "时间";
                    chartPie.ChartAreas["ChartArea1"].AxisY.Title = "数值";
                    
                    // 设置图表标题
                    chartPie.Titles.Clear();
                    chartPie.Titles.Add("系统资源使用率实时监控趋势图");
                    
                    // 设置图例位置
                    chartPie.Legends["Legend1"].Docking = Docking.Top;
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("更新折线图失败: {0}", ex.Message));
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 初始化数据库表
            InitializeDatabase();
            
            // 初始化饼图查询日期时间控件
            dtpPieChartEnd.Value = DateTime.Now;
            dtpPieChartStart.Value = DateTime.Now.AddDays(-1);
            
            // 初始化历史数据查询日期时间控件
            dtpEnd.Value = DateTime.Now;
            dtpStart.Value = DateTime.Now.AddDays(-1);
            
            // 初始化预警查询日期时间控件
            dtpWarningEnd.Value = DateTime.Now;
            dtpWarningStart.Value = DateTime.Now.AddDays(-1);
            
            // 设置指标类型默认值
            cmbMetricType.SelectedIndex = 0; // 默认选择"CPU使用率"
            
            // 窗体加载时启动定时器
            _timer.Start();
        }
        
        // 初始化数据库表
        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // 检查t_warning表是否存在，不存在则创建
                    string checkTableSql = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 't_warning'";
                    using (var command = new MySqlCommand(checkTableSql, connection))
                    {
                        int tableCount = Convert.ToInt32(command.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            // 创建t_warning表
                            string createTableSql = @"
                                CREATE TABLE t_warning (
                                    id INT AUTO_INCREMENT PRIMARY KEY,
                                    device_id VARCHAR(50) NOT NULL,
                                    warning_time DATETIME NOT NULL,
                                    warning_content TEXT,
                                    metric_type VARCHAR(50),
                                    metric_value FLOAT,
                                    threshold_type VARCHAR(50),
                                    threshold_value FLOAT,
                                    processing_status VARCHAR(20) DEFAULT 'UNHANDLED',
                                    INDEX idx_device_id (device_id),
                                    INDEX idx_warning_time (warning_time),
                                    INDEX idx_processing_status (processing_status)
                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
                            ";
                            
                            using (var createCommand = new MySqlCommand(createTableSql, connection))
                            {
                                createCommand.ExecuteNonQuery();
                                Log("成功创建t_warning表");
                            }
                        }
                    }
                    
                    // 检查t_metrics表是否存在，不存在则创建
                    checkTableSql = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 't_metrics'";
                    using (var command = new MySqlCommand(checkTableSql, connection))
                    {
                        int tableCount = Convert.ToInt32(command.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            // 创建t_metrics表
                            string createTableSql = @"
                                CREATE TABLE t_metrics (
                                    id INT AUTO_INCREMENT PRIMARY KEY,
                                    device_id VARCHAR(50) NOT NULL,
                                    collect_time DATETIME NOT NULL,
                                    cpu_usage FLOAT,
                                    avail_memory FLOAT,
                                    cpu_temp FLOAT,
                                    INDEX idx_device_id (device_id),
                                    INDEX idx_collect_time (collect_time)
                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
                            ";
                            
                            using (var createCommand = new MySqlCommand(createTableSql, connection))
                            {
                                createCommand.ExecuteNonQuery();
                                Log("成功创建t_metrics表");
                            }
                        }
                    }
                    
                    // 检查t_sys_metadata表是否存在，不存在则创建
                    checkTableSql = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 't_sys_metadata'";
                    using (var command = new MySqlCommand(checkTableSql, connection))
                    {
                        int tableCount = Convert.ToInt32(command.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            // 创建t_sys_metadata表
                            string createTableSql = @"
                                CREATE TABLE t_sys_metadata (
                                    id INT AUTO_INCREMENT PRIMARY KEY,
                                    device_id VARCHAR(50) NOT NULL,
                                    cpu_usage_warn FLOAT DEFAULT 80.0,
                                    cpu_usage_danger FLOAT DEFAULT 90.0,
                                    avail_memory_warn FLOAT DEFAULT 500.0,
                                    avail_memory_danger FLOAT DEFAULT 100.0,
                                    cpu_temp_warn FLOAT DEFAULT 85.0,
                                    cpu_temp_danger FLOAT DEFAULT 95.0,
                                    UNIQUE KEY uk_device_id (device_id)
                                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
                            ";
                            
                            using (var createCommand = new MySqlCommand(createTableSql, connection))
                            {
                                createCommand.ExecuteNonQuery();
                                Log("成功创建t_sys_metadata表");
                                
                                // 插入默认阈值配置
                                string insertSql = @"
                                    INSERT INTO t_sys_metadata 
                                    (device_id, cpu_usage_warn, cpu_usage_danger, avail_memory_warn, avail_memory_danger, cpu_temp_warn, cpu_temp_danger)
                                    VALUES 
                                    ('local-pc-01', 80.0, 90.0, 500.0, 100.0, 85.0, 95.0);
                                ";
                                
                                using (var insertCommand = new MySqlCommand(insertSql, connection))
                                {
                                    insertCommand.ExecuteNonQuery();
                                    Log("成功插入默认阈值配置");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("初始化数据库失败: {0}", ex.Message));
            }
        }
        
        // 标签页切换事件处理
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 如果切换到实时监控标签页，立即更新一次数据显示
                if (tabControl.SelectedTab == tabScreen)
                {
                    // 获取最新的系统指标数据
                    float cpuUsage = cpuCounter.NextValue();
                    var memCounter = new PerformanceCounter("Memory", "Available MBytes");
                    float availMemory = memCounter.NextValue();
                    
                    float cpuTemp = 0;
                    var cpu = computer.Hardware.FirstOrDefault(h => h.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.Cpu);
                    if (cpu != null)
                    {
                        cpu.Update();
                        var tempSensor = cpu.Sensors.FirstOrDefault(s => s.SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature);
                        if (tempSensor != null)
                        {
                            cpuTemp = (float)tempSensor.Value;
                        }
                    }
                    
                    // 更新实时数据显示
                    UpdateRealTimeDisplay(cpuUsage, availMemory, cpuTemp);
                }
                // 如果切换到饼图分析标签页，自动查询并显示默认指标的饼图
                else if (tabControl.SelectedTab == tabPieChart)
                {
                    // 自动触发饼图查询按钮点击事件
                    btnQueryPieChart_Click(null, null);
                }
                // 如果切换到历史数据标签页，自动查询并显示历史数据
                else if (tabControl.SelectedTab == tabHistory)
                {
                    // 自动触发历史数据查询按钮点击事件
                    btnQueryHistory_Click(null, null);
                }
                // 如果切换到预警查询标签页，自动查询并显示预警数据
                else if (tabControl.SelectedTab == tabWarnings)
                {
                    // 自动触发预警查询按钮点击事件
                    btnQueryWarnings_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("标签页切换处理失败: {0}", ex.Message));
            }
        }

        
    }
}
