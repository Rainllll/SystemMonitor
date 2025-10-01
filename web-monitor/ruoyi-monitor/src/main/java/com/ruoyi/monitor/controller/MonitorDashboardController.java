package com.ruoyi.monitor.controller;

import java.util.List;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import com.ruoyi.common.annotation.Log;
import com.ruoyi.common.core.controller.BaseController;
import com.ruoyi.common.core.domain.AjaxResult;
import com.ruoyi.common.enums.BusinessType;
import com.ruoyi.monitor.domain.MonitorMetrics;
import com.ruoyi.monitor.service.IMonitorDashboardService;
import com.ruoyi.common.utils.poi.ExcelUtil;
import com.ruoyi.common.core.page.TableDataInfo;

/**
 * 监控大屏Controller
 * 
 * @author ruoyi
 * @date 2024-01-01
 */
@RestController
@RequestMapping("/monitor/dashboard")
public class MonitorDashboardController extends BaseController
{
    @Autowired
    private IMonitorDashboardService monitorDashboardService;

    /**
     * 获取监控大屏概览数据
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/overview")
    public AjaxResult getOverview()
    {
        Map<String, Object> overview = monitorDashboardService.getOverviewData();
        return success(overview);
    }

    /**
     * 获取实时监控数据
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/realtime")
    public AjaxResult getRealtimeData()
    {
        Map<String, Object> realtimeData = monitorDashboardService.getRealtimeData();
        return success(realtimeData);
    }

    /**
     * 获取设备状态统计
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/deviceStatus")
    public AjaxResult getDeviceStatus()
    {
        Map<String, Object> deviceStatus = monitorDashboardService.getDeviceStatusStatistics();
        return success(deviceStatus);
    }

    /**
     * 获取预警统计数据
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/warningStats")
    public AjaxResult getWarningStats()
    {
        Map<String, Object> warningStats = monitorDashboardService.getWarningStatistics();
        return success(warningStats);
    }

    /**
     * 获取系统资源使用趋势
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/trend/{deviceId}")
    public AjaxResult getResourceTrend(@PathVariable("deviceId") String deviceId)
    {
        Map<String, Object> trendData = monitorDashboardService.getResourceTrend(deviceId);
        return success(trendData);
    }

    /**
     * 获取TOP设备资源使用排行
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/topDevices")
    public AjaxResult getTopDevices()
    {
        Map<String, Object> topDevices = monitorDashboardService.getTopDevicesUsage();
        return success(topDevices);
    }

    /**
     * 获取系统健康度评分
     */
    @PreAuthorize("@ss.hasPermi('monitor:dashboard:view')")
    @GetMapping("/healthScore")
    public AjaxResult getHealthScore()
    {
        Map<String, Object> healthScore = monitorDashboardService.getSystemHealthScore();
        return success(healthScore);
    }
}