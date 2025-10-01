package com.ruoyi.monitor.service;

import java.util.Map;

/**
 * 监控大屏Service接口
 * 
 * @author ruoyi
 * @date 2024-01-01
 */
public interface IMonitorDashboardService 
{
    /**
     * 获取监控大屏概览数据
     * 
     * @return 概览数据
     */
    Map<String, Object> getOverviewData();

    /**
     * 获取实时监控数据
     * 
     * @return 实时数据
     */
    Map<String, Object> getRealtimeData();

    /**
     * 获取设备状态统计
     * 
     * @return 设备状态统计
     */
    Map<String, Object> getDeviceStatusStatistics();

    /**
     * 获取预警统计数据
     * 
     * @return 预警统计
     */
    Map<String, Object> getWarningStatistics();

    /**
     * 获取系统资源使用趋势
     * 
     * @param deviceId 设备ID
     * @return 趋势数据
     */
    Map<String, Object> getResourceTrend(String deviceId);

    /**
     * 获取TOP设备资源使用排行
     * 
     * @return TOP设备数据
     */
    Map<String, Object> getTopDevicesUsage();

    /**
     * 获取系统健康度评分
     * 
     * @return 健康度评分
     */
    Map<String, Object> getSystemHealthScore();
}