package com.ruoyi.monitor.domain;

import java.util.Date;
import com.fasterxml.jackson.annotation.JsonFormat;
import org.apache.commons.lang3.builder.ToStringBuilder;
import org.apache.commons.lang3.builder.ToStringStyle;
import com.ruoyi.common.annotation.Excel;
import com.ruoyi.common.core.domain.BaseEntity;

/**
 * 监控设备信息对象 monitor_device
 * 
 * @author ruoyi
 * @date 2024-01-01
 */
public class MonitorDevice extends BaseEntity
{
    private static final long serialVersionUID = 1L;

    /** 设备ID */
    private String deviceId;

    /** 设备名称 */
    @Excel(name = "设备名称")
    private String deviceName;

    /** 设备类型 */
    @Excel(name = "设备类型")
    private String deviceType;

    /** IP地址 */
    @Excel(name = "IP地址")
    private String ipAddress;

    /** 设备位置 */
    @Excel(name = "设备位置")
    private String location;

    /** 状态(0=离线 1=在线) */
    @Excel(name = "状态", readConverterExp = "0=离线,1=在线")
    private String status;

    /** 最后心跳时间 */
    @JsonFormat(pattern = "yyyy-MM-dd HH:mm:ss")
    @Excel(name = "最后心跳时间", width = 30, dateFormat = "yyyy-MM-dd HH:mm:ss")
    private Date lastHeartbeat;

    public void setDeviceId(String deviceId) 
    {
        this.deviceId = deviceId;
    }

    public String getDeviceId() 
    {
        return deviceId;
    }
    public void setDeviceName(String deviceName) 
    {
        this.deviceName = deviceName;
    }

    public String getDeviceName() 
    {
        return deviceName;
    }
    public void setDeviceType(String deviceType) 
    {
        this.deviceType = deviceType;
    }

    public String getDeviceType() 
    {
        return deviceType;
    }
    public void setIpAddress(String ipAddress) 
    {
        this.ipAddress = ipAddress;
    }

    public String getIpAddress() 
    {
        return ipAddress;
    }
    public void setLocation(String location) 
    {
        this.location = location;
    }

    public String getLocation() 
    {
        return location;
    }
    public void setStatus(String status) 
    {
        this.status = status;
    }

    public String getStatus() 
    {
        return status;
    }
    public void setLastHeartbeat(Date lastHeartbeat) 
    {
        this.lastHeartbeat = lastHeartbeat;
    }

    public Date getLastHeartbeat() 
    {
        return lastHeartbeat;
    }

    @Override
    public String toString() {
        return new ToStringBuilder(this,ToStringStyle.MULTI_LINE_STYLE)
            .append("deviceId", getDeviceId())
            .append("deviceName", getDeviceName())
            .append("deviceType", getDeviceType())
            .append("ipAddress", getIpAddress())
            .append("location", getLocation())
            .append("status", getStatus())
            .append("lastHeartbeat", getLastHeartbeat())
            .append("createBy", getCreateBy())
            .append("createTime", getCreateTime())
            .append("updateBy", getUpdateBy())
            .append("updateTime", getUpdateTime())
            .append("remark", getRemark())
            .toString();
    }
}