using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Timers;
using LibreHardwareMonitor.Hardware;
using MySqlConnector;//右键项目 → 管理 NuGet 包 → 搜索并安装 MySqlConnector

namespace FormMain
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer _timer;
        private int _intervalSeconds = 1; // 默认采集周期 1s
        private string _logPath = @"D:\data_log.txt"; // 日志文件路径
        private PerformanceCounter cpuCounter;
        private Computer computer;

        // 数据库连接字符串（我使用的端口3307）
        private string _connectionString = "server=localhost;database=system_monitor;user=root;password=123456;port=3306;";
        
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


        public MainForm()
        {
            InitializeComponent();

            // 初始化 CPU 使用率计数器
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(500); // 预热

            // 初始化 LibreHardwareMonitor
            computer = new Computer() { IsCpuEnabled = true }; // 注意：IsCpuEnabled
            computer.Open();

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
                    if (hardware.HardwareType == HardwareType.Cpu) // 注意：Cpu
                    {
                        hardware.Update();
                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                cpuTemp = sensor.Value.GetValueOrDefault();
                                break; // 只取第一个温度传感器
                            }
                        }
                    }
                    if (cpuTemp >= 0) break;
                }

                string log = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}: CPU={cpuUsage:F2}%, 可用内存={availableMemory}MB, CPU温度={cpuTemp:F1}°C";
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
                        command.Parameters.AddWithValue("@cpuUsage", cpuUsage);
                        command.Parameters.AddWithValue("@availableMemory", availableMemory);
                        command.Parameters.AddWithValue("@cpuTemp", cpuTemp);


                        // 执行插入并返回是否成功，如果 rowsAffected > 0 为 true（例如等于 1），说明有至少一行数据被成功插入，方法返回 true
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Log($"数据库错误: {ex.Message}");
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
                        command.Parameters.AddWithValue("@metricValue", metricValue);
                        command.Parameters.AddWithValue("@thresholdType", thresholdType);
                        command.Parameters.AddWithValue("@thresholdValue", thresholdValue);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Log($"预警信息保存失败: {ex.Message}");
                    return false;
                }
            }
        }
        
        // 更新历史数据
        private void UpdateHistoryData(double cpuUsage, double availableMemory, double cpuTemp)
        {
            const int historySize = 10;
            
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
                string content = $"CPU使用率过高({cpuUsage:F2}%), 达到危险阈值({_cpuUsageDanger}%)";
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU使用率", cpuUsage, "DANGER", _cpuUsageDanger);
                LogWarning(content);
            }
            else if (cpuUsage >= _cpuUsageWarn)
            {
                string content = $"CPU使用率较高({cpuUsage:F2}%), 达到警告阈值({_cpuUsageWarn}%)";
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU使用率", cpuUsage, "WARN", _cpuUsageWarn);
                LogWarning(content);
            }
            
            // 检查可用内存
            if (availableMemory <= _availMemoryDanger)
            {
                string content = $"可用内存过低({availableMemory:F2}MB), 达到危险阈值({_availMemoryDanger}MB)";
                SaveAlertToDatabase(deviceId, collectTime, content, "可用内存", availableMemory, "DANGER", _availMemoryDanger);
                LogWarning(content);
            }
            else if (availableMemory <= _availMemoryWarn)
            {
                string content = $"可用内存较低({availableMemory:F2}MB), 达到警告阈值({_availMemoryWarn}MB)";
                SaveAlertToDatabase(deviceId, collectTime, content, "可用内存", availableMemory, "WARN", _availMemoryWarn);
                LogWarning(content);
            }
            
            // 检查CPU温度
            if (cpuTemp >= _cpuTempDanger)
            {
                string content = $"CPU温度过高({cpuTemp:F2}°C), 达到危险阈值({_cpuTempDanger}°C)";
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU温度", cpuTemp, "DANGER", _cpuTempDanger);
                LogWarning(content);
            }
            else if (cpuTemp >= _cpuTempWarn)
            {
                string content = $"CPU温度较高({cpuTemp:F2}°C), 达到警告阈值({_cpuTempWarn}°C)";
                SaveAlertToDatabase(deviceId, collectTime, content, "CPU温度", cpuTemp, "WARN", _cpuTempWarn);
                LogWarning(content);
            }
            
            // 检查波动率预警
            if (_cpuUsageHistory.Count >= 2)
            {
                double cpuVolatility = CalculateVolatility(_cpuUsageHistory, cpuUsage);
                if (cpuVolatility >= _volatilityThreshold)
                {
                    string content = $"CPU使用率波动较大({cpuVolatility:F2}%), 超过阈值({_volatilityThreshold}%)";
                    SaveAlertToDatabase(deviceId, collectTime, content, "CPU使用率波动率", cpuVolatility, "WARN", _volatilityThreshold);
                    LogWarning(content);
                }
            }
        }
        
        // 记录警告信息
        private void LogWarning(string message)
        {
            string warningMsg = $"[{DateTime.Now:HH:mm:ss}] 警告: {message}";
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
                }));
            }
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
                    Log($"加载阈值配置失败: {ex.Message}");
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
                MessageBox.Show($"日志写入失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string log = $"采集周期已修改为 {seconds} 秒";
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
                    Log($"保存阈值配置失败: {ex.Message}");
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (computer != null)
                computer.Close();
        }
    }
}
