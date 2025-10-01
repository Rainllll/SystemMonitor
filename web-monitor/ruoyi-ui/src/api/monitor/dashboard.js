import request from '@/utils/request'

// 获取监控大屏概览数据
export function getOverview() {
  return request({
    url: '/monitor/dashboard/overview',
    method: 'get'
  })
}

// 获取实时监控数据
export function getRealtimeData() {
  return request({
    url: '/monitor/dashboard/realtime',
    method: 'get'
  })
}

// 获取设备状态统计
export function getDeviceStatus() {
  return request({
    url: '/monitor/dashboard/deviceStatus',
    method: 'get'
  })
}

// 获取预警统计数据
export function getWarningStats() {
  return request({
    url: '/monitor/dashboard/warningStats',
    method: 'get'
  })
}

// 获取系统资源使用趋势
export function getResourceTrend(deviceId) {
  return request({
    url: '/monitor/dashboard/trend/' + deviceId,
    method: 'get'
  })
}

// 获取TOP设备资源使用排行
export function getTopDevices() {
  return request({
    url: '/monitor/dashboard/topDevices',
    method: 'get'
  })
}

// 获取系统健康度评分
export function getHealthScore() {
  return request({
    url: '/monitor/dashboard/healthScore',
    method: 'get'
  })
}