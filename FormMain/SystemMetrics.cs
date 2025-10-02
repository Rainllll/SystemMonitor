using System;

namespace FormMain
{
    public class SystemMetric
    {
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public float CpuUsage { get; set; }
        public float AvailableMemory { get; set; }
        public float CpuTemperature { get; set; }
    }

    public class WarningRecord
    {
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string WarningType { get; set; }
        public float CurrentValue { get; set; }
        public float ThresholdValue { get; set; }
        public string WarningLevel { get; set; }
        public string Description { get; set; }
        public string ProcessingStatus { get; set; }
    }

    public class ThresholdConfig
    {
        public string DeviceId { get; set; }
        public float CpuUsageWarningThreshold { get; set; }
        public float CpuUsageDangerThreshold { get; set; }
        public float MemoryWarningThreshold { get; set; }
        public float MemoryDangerThreshold { get; set; }
        public float TemperatureWarningThreshold { get; set; }
        public float TemperatureDangerThreshold { get; set; }
    }
}