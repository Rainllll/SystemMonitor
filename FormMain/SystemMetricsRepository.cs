using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace FormMain
{
    public class SystemMetricsRepository
    {
        private DatabaseHelper dbHelper;

        public SystemMetricsRepository()
        {
            dbHelper = new DatabaseHelper();
        }

        public SystemMetricsRepository(string connectionString)
        {
            dbHelper = new DatabaseHelper(connectionString);
        }

        public List<SystemMetric> GetLatestMetrics(int count = 100)
        {
            List<SystemMetric> metrics = new List<SystemMetric>();
            string query = String.Format("SELECT device_id, collect_time, cpu_usage, avail_memory, cpu_temp FROM t_metrics ORDER BY collect_time DESC LIMIT {0}", count);

            try
            {
                using (MySqlDataReader reader = (MySqlDataReader)dbHelper.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        SystemMetric metric = new SystemMetric
                        {
                            DeviceId = reader.GetString("device_id"),
                            Timestamp = reader.GetDateTime("collect_time"),
                            CpuUsage = reader.GetFloat("cpu_usage"),
                            AvailableMemory = reader.GetFloat("avail_memory"),
                            CpuTemperature = reader.GetFloat("cpu_temp")
                        };
                        metrics.Add(metric);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取最新指标数据失败: " + ex.Message);
            }

            return metrics;
        }

        public List<SystemMetric> GetMetricsByTimeRange(DateTime startTime, DateTime endTime)
        {
            List<SystemMetric> metrics = new List<SystemMetric>();
            string query = String.Format("SELECT device_id, collect_time, cpu_usage, avail_memory, cpu_temp FROM t_metrics WHERE collect_time BETWEEN '{0}' AND '{1}' ORDER BY collect_time DESC", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                using (MySqlDataReader reader = (MySqlDataReader)dbHelper.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        SystemMetric metric = new SystemMetric
                        {
                            DeviceId = reader.GetString("device_id"),
                            Timestamp = reader.GetDateTime("collect_time"),
                            CpuUsage = reader.GetFloat("cpu_usage"),
                            AvailableMemory = reader.GetFloat("avail_memory"),
                            CpuTemperature = reader.GetFloat("cpu_temp")
                        };
                        metrics.Add(metric);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取指定时间范围指标数据失败: " + ex.Message);
            }

            return metrics;
        }

        public List<WarningRecord> GetWarningRecordsByTimeRange(DateTime startTime, DateTime endTime)
        {
            List<WarningRecord> warnings = new List<WarningRecord>();
            string query = String.Format("SELECT device_id, warning_time, warning_content, metric_type, metric_value, threshold_type, threshold_value, processing_status FROM t_warning WHERE warning_time BETWEEN '{0}' AND '{1}' ORDER BY warning_time DESC", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                using (MySqlDataReader reader = (MySqlDataReader)dbHelper.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        WarningRecord warning = new WarningRecord
                        {
                            DeviceId = reader.GetString("device_id"),
                            Timestamp = reader.GetDateTime("warning_time"),
                            WarningType = reader.GetString("metric_type"),
                            CurrentValue = reader.GetFloat("metric_value"),
                            ThresholdValue = reader.GetFloat("threshold_value"),
                            WarningLevel = reader.GetString("threshold_type"),
                            Description = reader.GetString("warning_content"),
                            ProcessingStatus = reader.GetString("processing_status")
                        };
                        warnings.Add(warning);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取预警记录失败: " + ex.Message);
            }

            return warnings;
        }

        public List<ThresholdConfig> GetThresholdConfigs()
        {
            List<ThresholdConfig> configs = new List<ThresholdConfig>();
            string query = "SELECT device_id, cpu_usage_warn, cpu_usage_danger, avail_memory_warn, avail_memory_danger, cpu_temp_warn, cpu_temp_danger FROM t_sys_metadata";

            try
            {
                using (MySqlDataReader reader = (MySqlDataReader)dbHelper.ExecuteQuery(query))
                {
                    while (reader.Read())
                    {
                        ThresholdConfig config = new ThresholdConfig
                        {
                            DeviceId = reader.GetString("device_id"),
                            CpuUsageWarningThreshold = reader.GetFloat("cpu_usage_warn"),
                            CpuUsageDangerThreshold = reader.GetFloat("cpu_usage_danger"),
                            MemoryWarningThreshold = reader.GetFloat("avail_memory_warn"),
                            MemoryDangerThreshold = reader.GetFloat("avail_memory_danger"),
                            TemperatureWarningThreshold = reader.GetFloat("cpu_temp_warn"),
                            TemperatureDangerThreshold = reader.GetFloat("cpu_temp_danger")
                        };
                        configs.Add(config);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取阈值配置失败: " + ex.Message);
            }

            return configs;
        }

        public SystemMetric GetLatestMetric()
        {
            SystemMetric metric = null;
            string query = "SELECT device_id, collect_time, cpu_usage, avail_memory, cpu_temp FROM t_metrics ORDER BY collect_time DESC LIMIT 1";

            try
            {
                using (MySqlDataReader reader = (MySqlDataReader)dbHelper.ExecuteQuery(query))
                {
                    if (reader.Read())
                    {
                        metric = new SystemMetric
                        {
                            DeviceId = reader.GetString("device_id"),
                            Timestamp = reader.GetDateTime("collect_time"),
                            CpuUsage = reader.GetFloat("cpu_usage"),
                            AvailableMemory = reader.GetFloat("avail_memory"),
                            CpuTemperature = reader.GetFloat("cpu_temp")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取最新指标数据失败: " + ex.Message);
            }

            return metric;
        }

        public bool TestConnection()
        {
            try
            {
                dbHelper.TestConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}