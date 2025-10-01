package com.ruoyi.monitor.domain;

import java.math.BigDecimal;
import java.util.Date;
import com.fasterxml.jackson.annotation.JsonFormat;
import org.apache.commons.lang3.builder.ToStringBuilder;
import org.apache.commons.lang3.builder.ToStringStyle;
import com.ruoyi.common.annotation.Excel;
import com.ruoyi.common.core.domain.BaseEntity;

/**
 * 监控指标数据对象 monitor_metrics
 * 
 * @author ruoyi
 * @date 2024-01-01
 */
public class MonitorMetrics extends BaseEntity
{
    private static final long serialVersionUID = 1L;

    /** 主键ID */
    private Long id;

    /** 设备ID */
    @Excel(name = "设备ID")
    private String deviceId;

    /** 采集时间 */
    @JsonFormat(pattern = "yyyy-MM-dd HH:mm:ss")
    @Excel(name = "采集时间", width = 30, dateFormat = "yyyy-MM-dd HH:mm:ss")
    private Date collectTime;

    /** CPU使用率(%) */
    @Excel(name = "CPU使用率(%)")
    private BigDecimal cpuUsage;

    /** 总内存(MB) */
    @Excel(name = "总内存(MB)")
    private BigDecimal memoryTotal;

    /** 已用内存(MB) */
    @Excel(name = "已用内存(MB)")
    private BigDecimal memoryUsed;

    /** 可用内存(MB) */
    @Excel(name = "可用内存(MB)")
    private BigDecimal memoryAvailable;

    /** CPU温度(°C) */
    @Excel(name = "CPU温度(°C)")
    private BigDecimal cpuTemp;

    /** 磁盘使用率(%) */
    @Excel(name = "磁盘使用率(%)")
    private BigDecimal diskUsage;

    /** 网络入流量(KB/s) */
    @Excel(name = "网络入流量(KB/s)")
    private BigDecimal networkIn;

    /** 网络出流量(KB/s) */
    @Excel(name = "网络出流量(KB/s)")
    private BigDecimal networkOut;

    public void setId(Long id) 
    {
        this.id = id;
    }

    public Long getId() 
    {
        return id;
    }
    public void setDeviceId(String deviceId) 
    {
        this.deviceId = deviceId;
    }

    public String getDeviceId() 
    {
        return deviceId;
    }
    public void setCollectTime(Date collectTime) 
    {
        this.collectTime = collectTime;
    }

    public Date getCollectTime() 
    {
        return collectTime;
    }
    public void setCpuUsage(BigDecimal cpuUsage) 
    {
        this.cpuUsage = cpuUsage;
    }

    public BigDecimal getCpuUsage() 
    {
        return cpuUsage;
    }
    public void setMemoryTotal(BigDecimal memoryTotal) 
    {
        this.memoryTotal = memoryTotal;
    }

    public BigDecimal getMemoryTotal() 
    {
        return memoryTotal;
    }
    public void setMemoryUsed(BigDecimal memoryUsed) 
    {
        this.memoryUsed = memoryUsed;
    }

    public BigDecimal getMemoryUsed() 
    {
        return memoryUsed;
    }
    public void setMemoryAvailable(BigDecimal memoryAvailable) 
    {
        this.memoryAvailable = memoryAvailable;
    }

    public BigDecimal getMemoryAvailable() 
    {
        return memoryAvailable;
    }
    public void setCpuTemp(BigDecimal cpuTemp) 
    {
        this.cpuTemp = cpuTemp;
    }

    public BigDecimal getCpuTemp() 
    {
        return cpuTemp;
    }
    public void setDiskUsage(BigDecimal diskUsage) 
    {
        this.diskUsage = diskUsage;
    }

    public BigDecimal getDiskUsage() 
    {
        return diskUsage;
    }
    public void setNetworkIn(BigDecimal networkIn) 
    {
        this.networkIn = networkIn;
    }

    public BigDecimal getNetworkIn() 
    {
        return networkIn;
    }
    public void setNetworkOut(BigDecimal networkOut) 
    {
        this.networkOut = networkOut;
    }

    public BigDecimal getNetworkOut() 
    {
        return networkOut;
    }

    @Override
    public String toString() {
        return new ToStringBuilder(this,ToStringStyle.MULTI_LINE_STYLE)
            .append("id", getId())
            .append("deviceId", getDeviceId())
            .append("collectTime", getCollectTime())
            .append("cpuUsage", getCpuUsage())
            .append("memoryTotal", getMemoryTotal())
            .append("memoryUsed", getMemoryUsed())
            .append("memoryAvailable", getMemoryAvailable())
            .append("cpuTemp", getCpuTemp())
            .append("diskUsage", getDiskUsage())
            .append("networkIn", getNetworkIn())
            .append("networkOut", getNetworkOut())
            .append("createTime", getCreateTime())
            .toString();
    }
}