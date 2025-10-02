using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormMain
{
    public class DataManagementStrategy
    {
        private string _connectionString;
        private string _logPath;
        private int _retentionDays = 30; // 默认保留30天的数据
        private int _aggregationInterval = 60; // 默认聚合间隔为60分钟
        private bool _enableDataArchiving = true; // 是否启用数据归档
        private bool _enableDataAggregation = true; // 是否启用数据聚合

        public DataManagementStrategy(string connectionString)
        {
            _connectionString = connectionString;
            _logPath = @"D:\data_management_log.txt"; // 默认日志路径
        }

        public DataManagementStrategy(string connectionString, string logPath)
        {
            _connectionString = connectionString;
            _logPath = logPath;
        }

        // 设置数据保留天数
        public void SetRetentionDays(int days)
        {
            _retentionDays = days;
        }

        // 设置数据聚合间隔（分钟）
        public void SetAggregationInterval(int minutes)
        {
            _aggregationInterval = minutes;
        }

        // 启用/禁用数据归档
        public void SetDataArchiving(bool enabled)
        {
            _enableDataArchiving = enabled;
        }

        // 启用/禁用数据聚合
        public void SetDataAggregation(bool enabled)
        {
            _enableDataAggregation = enabled;
        }

        // 执行数据管理策略
        public void ExecuteDataManagement()
        {
            try
            {
                Log("开始执行数据管理策略...");

                // 1. 清理过期数据
                CleanExpiredData();

                // 2. 数据聚合
                if (_enableDataAggregation)
                {
                    AggregateData();
                }

                // 3. 数据归档
                if (_enableDataArchiving)
                {
                    ArchiveData();
                }

                Log("数据管理策略执行完成");
            }
            catch (Exception ex)
            {
                Log(String.Format("数据管理策略执行失败: {0}", ex.Message));
            }
        }

        // 清理过期数据
        private void CleanExpiredData()
        {
            try
            {
                Log(String.Format("开始清理 {0} 天前的数据...", _retentionDays));

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 清理指标数据
                    string cleanMetricsSql = String.Format("DELETE FROM t_metrics WHERE collect_time < DATE_SUB(NOW(), INTERVAL {0} DAY)", _retentionDays);
                    using (var command = new MySqlCommand(cleanMetricsSql, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Log(String.Format("已清理 {0} 条指标数据", rowsAffected));
                    }

                    // 清理已处理的预警数据
                    string cleanWarningsSql = String.Format("DELETE FROM t_warning WHERE warning_time < DATE_SUB(NOW(), INTERVAL {0} DAY) AND processing_status = 'HANDLED'", _retentionDays);
                    using (var command = new MySqlCommand(cleanWarningsSql, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Log(String.Format("已清理 {0} 条已处理的预警数据", rowsAffected));
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("清理过期数据失败: {0}", ex.Message));
            }
        }

        // 公开方法：清理指定天数前的数据
        public int CleanOldData(int days)
        {
            try
            {
                Log(String.Format("开始清理 {0} 天前的数据...", days));

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 清理指标数据
                    string cleanMetricsSql = String.Format("DELETE FROM t_metrics WHERE collect_time < DATE_SUB(NOW(), INTERVAL {0} DAY)", days);
                    using (var command = new MySqlCommand(cleanMetricsSql, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Log(String.Format("已清理 {0} 条指标数据", rowsAffected));
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("清理过期数据失败: {0}", ex.Message));
                return 0;
            }
        }

        // 数据聚合
        private void AggregateData()
        {
            try
            {
                Log(String.Format("开始数据聚合，聚合间隔: {0} 分钟...", _aggregationInterval));

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 检查聚合表是否存在，不存在则创建
                    string createAggregatedTableSql = @"
                        CREATE TABLE IF NOT EXISTS t_metrics_aggregated (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            device_id VARCHAR(50) NOT NULL,
                            aggregation_time DATETIME NOT NULL,
                            avg_cpu_usage FLOAT,
                            max_cpu_usage FLOAT,
                            min_cpu_usage FLOAT,
                            avg_avail_memory FLOAT,
                            max_avail_memory FLOAT,
                            min_avail_memory FLOAT,
                            avg_cpu_temp FLOAT,
                            max_cpu_temp FLOAT,
                            min_cpu_temp FLOAT,
                            record_count INT,
                            UNIQUE KEY (device_id, aggregation_time)
                        )";
                    using (var command = new MySqlCommand(createAggregatedTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // 聚合数据
                    string aggregateSql = @"
                        INSERT INTO t_metrics_aggregated 
                        (device_id, aggregation_time, avg_cpu_usage, max_cpu_usage, min_cpu_usage, 
                         avg_avail_memory, max_avail_memory, min_avail_memory, 
                         avg_cpu_temp, max_cpu_temp, min_cpu_temp, record_count)
                        SELECT 
                            device_id,
                            DATE_FORMAT(collect_time, '%Y-%m-%d %H:%i:00') AS aggregation_time,
                            AVG(cpu_usage) AS avg_cpu_usage,
                            MAX(cpu_usage) AS max_cpu_usage,
                            MIN(cpu_usage) AS min_cpu_usage,
                            AVG(avail_memory) AS avg_avail_memory,
                            MAX(avail_memory) AS max_avail_memory,
                            MIN(avail_memory) AS min_avail_memory,
                            AVG(cpu_temp) AS avg_cpu_temp,
                            MAX(cpu_temp) AS max_cpu_temp,
                            MIN(cpu_temp) AS min_cpu_temp,
                            COUNT(*) AS record_count
                        FROM 
                            t_metrics
                        WHERE 
                            collect_time >= DATE_SUB(NOW(), INTERVAL 7 DAY)
                            AND collect_time < DATE_SUB(NOW(), INTERVAL 1 DAY)
                            AND NOT EXISTS (
                                SELECT 1 FROM t_metrics_aggregated 
                                WHERE device_id = t_metrics.device_id 
                                AND aggregation_time = DATE_FORMAT(t_metrics.collect_time, '%Y-%m-%d %H:%i:00')
                            )
                        GROUP BY 
                            device_id, DATE_FORMAT(collect_time, '%Y-%m-%d %H:%i:00')
                        ON DUPLICATE KEY UPDATE
                            avg_cpu_usage = VALUES(avg_cpu_usage),
                            max_cpu_usage = VALUES(max_cpu_usage),
                            min_cpu_usage = VALUES(min_cpu_usage),
                            avg_avail_memory = VALUES(avg_avail_memory),
                            max_avail_memory = VALUES(max_avail_memory),
                            min_avail_memory = VALUES(min_avail_memory),
                            avg_cpu_temp = VALUES(avg_cpu_temp),
                            max_cpu_temp = VALUES(max_cpu_temp),
                            min_cpu_temp = VALUES(min_cpu_temp),
                            record_count = VALUES(record_count)";

                    using (var command = new MySqlCommand(aggregateSql, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Log(String.Format("已聚合 {0} 组数据", rowsAffected));
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("数据聚合失败: {0}", ex.Message));
            }
        }

        // 公开方法：按指定间隔聚合数据
        public int AggregateData(string intervalType)
        {
            try
            {
                Log(String.Format("开始数据聚合，聚合类型: {0}...", intervalType));

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 检查聚合表是否存在，不存在则创建
                    string createAggregatedTableSql = @"
                        CREATE TABLE IF NOT EXISTS t_metrics_aggregated (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            device_id VARCHAR(50) NOT NULL,
                            aggregation_time DATETIME NOT NULL,
                            avg_cpu_usage FLOAT,
                            max_cpu_usage FLOAT,
                            min_cpu_usage FLOAT,
                            avg_avail_memory FLOAT,
                            max_avail_memory FLOAT,
                            min_avail_memory FLOAT,
                            avg_cpu_temp FLOAT,
                            max_cpu_temp FLOAT,
                            min_cpu_temp FLOAT,
                            record_count INT,
                            UNIQUE KEY (device_id, aggregation_time)
                        )";
                    using (var command = new MySqlCommand(createAggregatedTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // 根据聚合类型设置时间格式
                    string timeFormat = "%Y-%m-%d %H:%i:00"; // 默认每小时
                    if (intervalType.ToLower() == "daily")
                    {
                        timeFormat = "%Y-%m-%d 00:00:00"; // 每天
                    }
                    else if (intervalType.ToLower() == "weekly")
                    {
                        timeFormat = "%Y-%u-1 00:00:00"; // 每周
                    }
                    else if (intervalType.ToLower() == "monthly")
                    {
                        timeFormat = "%Y-%m-01 00:00:00"; // 每月
                    }

                    // 聚合数据
                    string aggregateSql = String.Format(@"
                        INSERT INTO t_metrics_aggregated 
                        (device_id, aggregation_time, avg_cpu_usage, max_cpu_usage, min_cpu_usage, 
                         avg_avail_memory, max_avail_memory, min_avail_memory, 
                         avg_cpu_temp, max_cpu_temp, min_cpu_temp, record_count)
                        SELECT 
                            device_id,
                            DATE_FORMAT(collect_time, '{0}') AS aggregation_time,
                            AVG(cpu_usage) AS avg_cpu_usage,
                            MAX(cpu_usage) AS max_cpu_usage,
                            MIN(cpu_usage) AS min_cpu_usage,
                            AVG(avail_memory) AS avg_avail_memory,
                            MAX(avail_memory) AS max_avail_memory,
                            MIN(avail_memory) AS min_avail_memory,
                            AVG(cpu_temp) AS avg_cpu_temp,
                            MAX(cpu_temp) AS max_cpu_temp,
                            MIN(cpu_temp) AS min_cpu_temp,
                            COUNT(*) AS record_count
                        FROM 
                            t_metrics
                        WHERE 
                            collect_time >= DATE_SUB(NOW(), INTERVAL 30 DAY)
                            AND collect_time < DATE_SUB(NOW(), INTERVAL 1 DAY)
                            AND NOT EXISTS (
                                SELECT 1 FROM t_metrics_aggregated 
                                WHERE device_id = t_metrics.device_id 
                                AND aggregation_time = DATE_FORMAT(t_metrics.collect_time, '{0}')
                            )
                        GROUP BY 
                            device_id, DATE_FORMAT(collect_time, '{0}')
                        ON DUPLICATE KEY UPDATE
                            avg_cpu_usage = VALUES(avg_cpu_usage),
                            max_cpu_usage = VALUES(max_cpu_usage),
                            min_cpu_usage = VALUES(min_cpu_usage),
                            avg_avail_memory = VALUES(avg_avail_memory),
                            max_avail_memory = VALUES(max_avail_memory),
                            min_avail_memory = VALUES(min_avail_memory),
                            avg_cpu_temp = VALUES(avg_cpu_temp),
                            max_cpu_temp = VALUES(max_cpu_temp),
                            min_cpu_temp = VALUES(min_cpu_temp),
                            record_count = VALUES(record_count)", timeFormat);

                    using (var command = new MySqlCommand(aggregateSql, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        Log(String.Format("已聚合 {0} 组数据", rowsAffected));
                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("数据聚合失败: {0}", ex.Message));
                return 0;
            }
        }

        // 数据归档
        private void ArchiveData()
        {
            try
            {
                Log("开始数据归档...");

                // 创建归档目录
                string archiveDir = Path.Combine(Path.GetDirectoryName(_logPath), "archive");
                if (!Directory.Exists(archiveDir))
                {
                    Directory.CreateDirectory(archiveDir);
                }

                // 生成归档文件名
                string archiveFileName = String.Format("metrics_archive_{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);
                string archiveFilePath = Path.Combine(archiveDir, archiveFileName);

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 导出数据到CSV
                    string exportSql = "SELECT * FROM t_metrics WHERE collect_time >= DATE_SUB(NOW(), INTERVAL 7 DAY) AND collect_time < DATE_SUB(NOW(), INTERVAL 1 DAY)";
                    using (var command = new MySqlCommand(exportSql, connection))
                    using (var reader = command.ExecuteReader())
                    using (var writer = new StreamWriter(archiveFilePath, false, Encoding.UTF8))
                    {
                        // 写入CSV头
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i > 0) writer.Write(",");
                            writer.Write(reader.GetName(i));
                        }
                        writer.WriteLine();

                        // 写入数据
                        int recordCount = 0;
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (i > 0) writer.Write(",");
                                writer.Write(reader[i].ToString());
                            }
                            writer.WriteLine();
                            recordCount++;
                        }

                        Log(String.Format("已归档 {0} 条数据到 {1}", recordCount, archiveFileName));
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("数据归档失败: {0}", ex.Message));
            }
        }

        // 公开方法：归档指定天数前的数据
        public string ArchiveOldData(int days)
        {
            try
            {
                Log(String.Format("开始归档 {0} 天前的数据...", days));

                // 创建归档目录
                string archiveDir = Path.Combine(Path.GetDirectoryName(_logPath), "archive");
                if (!Directory.Exists(archiveDir))
                {
                    Directory.CreateDirectory(archiveDir);
                }

                // 生成归档文件名
                string archiveFileName = String.Format("metrics_archive_{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);
                string archiveFilePath = Path.Combine(archiveDir, archiveFileName);

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 导出数据到CSV
                    string exportSql = String.Format("SELECT * FROM t_metrics WHERE collect_time >= DATE_SUB(NOW(), INTERVAL {0} DAY) AND collect_time < DATE_SUB(NOW(), INTERVAL {1} DAY)", days + 7, days);
                    using (var command = new MySqlCommand(exportSql, connection))
                    using (var reader = command.ExecuteReader())
                    using (var writer = new StreamWriter(archiveFilePath, false, Encoding.UTF8))
                    {
                        // 写入CSV头
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i > 0) writer.Write(",");
                            writer.Write(reader.GetName(i));
                        }
                        writer.WriteLine();

                        // 写入数据
                        int recordCount = 0;
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (i > 0) writer.Write(",");
                                writer.Write(reader[i].ToString());
                            }
                            writer.WriteLine();
                            recordCount++;
                        }

                        Log(String.Format("已归档 {0} 条数据到 {1}", recordCount, archiveFileName));
                        return archiveFilePath;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("数据归档失败: {0}", ex.Message));
                return "";
            }
        }

        // 记录日志
        private void Log(string message)
        {
            try
            {
                string logEntry = String.Format("[{0:yyyy-MM-dd HH:mm:ss}] DataManagement: {1}", DateTime.Now, message);
                File.AppendAllText(_logPath, logEntry + Environment.NewLine);
            }
            catch
            {
                // 忽略日志写入错误
            }
        }

        // 获取数据库统计信息
        public DatabaseStatistics GetDatabaseStatistics()
        {
            var stats = new DatabaseStatistics();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // 获取指标数据表记录数
                    string metricsCountSql = "SELECT COUNT(*) FROM t_metrics";
                    using (var command = new MySqlCommand(metricsCountSql, connection))
                    {
                        stats.TotalRecords = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // 获取预警数据表记录数
                    string warningsCountSql = "SELECT COUNT(*) FROM t_warning";
                    using (var command = new MySqlCommand(warningsCountSql, connection))
                    {
                        stats.WarningRecordCount = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // 获取聚合数据表记录数
                    string aggregatedCountSql = "SELECT COUNT(*) FROM t_metrics_aggregated";
                    using (var command = new MySqlCommand(aggregatedCountSql, connection))
                    {
                        stats.AggregatedRecordCount = Convert.ToInt32(command.ExecuteScalar());
                    }

                    // 获取数据库大小
                    string dbSizeSql = "SELECT ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS db_size FROM information_schema.tables WHERE table_schema = 'system_monitor'";
                    using (var command = new MySqlCommand(dbSizeSql, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            stats.DatabaseSizeMB = Convert.ToDouble(result);
                        }
                    }

                    // 获取最早记录时间
                    string earliestTimeSql = "SELECT MIN(collect_time) FROM t_metrics";
                    using (var command = new MySqlCommand(earliestTimeSql, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            stats.EarliestRecordTime = Convert.ToDateTime(result).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }

                    // 获取最晚记录时间
                    string latestTimeSql = "SELECT MAX(collect_time) FROM t_metrics";
                    using (var command = new MySqlCommand(latestTimeSql, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            stats.LatestRecordTime = Convert.ToDateTime(result).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(String.Format("获取数据库统计信息失败: {0}", ex.Message));
            }

            return stats;
        }
    }

    // 数据库统计信息类
    public class DatabaseStatistics
    {
        public int TotalRecords { get; set; }
        public int WarningRecordCount { get; set; }
        public int AggregatedRecordCount { get; set; }
        public double DatabaseSizeMB { get; set; }
        public string EarliestRecordTime { get; set; }
        public string LatestRecordTime { get; set; }
    }
}