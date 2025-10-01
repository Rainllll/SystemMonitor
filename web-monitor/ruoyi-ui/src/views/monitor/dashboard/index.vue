<template>
  <div class="monitor-dashboard">
    <!-- 顶部概览卡片 -->
    <el-row :gutter="20" class="overview-cards">
      <el-col :span="6" v-for="(item, index) in overviewData" :key="index">
        <el-card class="overview-card" :class="item.type">
          <div class="card-content">
            <div class="card-icon">
              <i :class="item.icon"></i>
            </div>
            <div class="card-info">
              <div class="card-title">{{ item.title }}</div>
              <div class="card-value">{{ item.value }}</div>
              <div class="card-trend" :class="item.trend">
                <i :class="item.trendIcon"></i>
                {{ item.trendText }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 中间图表区域 -->
    <el-row :gutter="20" class="chart-section">
      <!-- 实时监控图表 -->
      <el-col :span="12">
        <el-card class="chart-card">
          <div slot="header" class="card-header">
            <span>实时监控</span>
            <el-button type="text" @click="refreshRealtimeData">刷新</el-button>
          </div>
          <div id="realtimeChart" style="height: 300px;"></div>
        </el-card>
      </el-col>

      <!-- 设备状态饼图 -->
      <el-col :span="12">
        <el-card class="chart-card">
          <div slot="header" class="card-header">
            <span>设备状态分布</span>
          </div>
          <div id="deviceStatusChart" style="height: 300px;"></div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 底部图表区域 -->
    <el-row :gutter="20" class="chart-section">
      <!-- 预警统计 -->
      <el-col :span="8">
        <el-card class="chart-card">
          <div slot="header" class="card-header">
            <span>预警统计</span>
          </div>
          <div id="warningChart" style="height: 250px;"></div>
        </el-card>
      </el-col>

      <!-- 资源使用趋势 -->
      <el-col :span="8">
        <el-card class="chart-card">
          <div slot="header" class="card-header">
            <span>资源使用趋势</span>
          </div>
          <div id="trendChart" style="height: 250px;"></div>
        </el-card>
      </el-col>

      <!-- TOP设备排行 -->
      <el-col :span="8">
        <el-card class="chart-card">
          <div slot="header" class="card-header">
            <span>设备资源排行</span>
          </div>
          <div id="topDevicesChart" style="height: 250px;"></div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import * as echarts from 'echarts'
import { getOverview, getRealtimeData, getDeviceStatus, getWarningStats, getResourceTrend, getTopDevices } from '@/api/monitor/dashboard'

export default {
  name: 'MonitorDashboard',
  data() {
    return {
      overviewData: [
        {
          title: '在线设备',
          value: '0',
          icon: 'el-icon-monitor',
          type: 'online',
          trend: 'up',
          trendIcon: 'el-icon-top',
          trendText: '较昨日 +2'
        },
        {
          title: '离线设备',
          value: '0',
          icon: 'el-icon-warning',
          type: 'offline',
          trend: 'down',
          trendIcon: 'el-icon-bottom',
          trendText: '较昨日 -1'
        },
        {
          title: '今日预警',
          value: '0',
          icon: 'el-icon-bell',
          type: 'warning',
          trend: 'up',
          trendIcon: 'el-icon-top',
          trendText: '较昨日 +5'
        },
        {
          title: '系统健康度',
          value: '0%',
          icon: 'el-icon-success',
          type: 'health',
          trend: 'stable',
          trendIcon: 'el-icon-minus',
          trendText: '保持稳定'
        }
      ],
      charts: {},
      refreshTimer: null
    }
  },
  mounted() {
    this.initCharts()
    this.loadData()
    this.startAutoRefresh()
  },
  beforeDestroy() {
    this.stopAutoRefresh()
    Object.values(this.charts).forEach(chart => {
      if (chart) {
        chart.dispose()
      }
    })
  },
  methods: {
    initCharts() {
      // 初始化实时监控图表
      this.charts.realtime = echarts.init(document.getElementById('realtimeChart'))
      
      // 初始化设备状态饼图
      this.charts.deviceStatus = echarts.init(document.getElementById('deviceStatusChart'))
      
      // 初始化预警统计图表
      this.charts.warning = echarts.init(document.getElementById('warningChart'))
      
      // 初始化趋势图表
      this.charts.trend = echarts.init(document.getElementById('trendChart'))
      
      // 初始化TOP设备图表
      this.charts.topDevices = echarts.init(document.getElementById('topDevicesChart'))
    },
    
    async loadData() {
      try {
        // 加载概览数据
        await this.loadOverviewData()
        
        // 加载图表数据
        await this.loadRealtimeData()
        await this.loadDeviceStatusData()
        await this.loadWarningData()
        await this.loadTrendData()
        await this.loadTopDevicesData()
      } catch (error) {
        console.error('加载数据失败:', error)
      }
    },
    
    async loadOverviewData() {
      const response = await getOverview()
      if (response.code === 200) {
        const data = response.data
        this.overviewData[0].value = data.onlineDevices || 0
        this.overviewData[1].value = data.offlineDevices || 0
        this.overviewData[2].value = data.todayWarnings || 0
        this.overviewData[3].value = (data.healthScore || 0) + '%'
      }
    },
    
    async loadRealtimeData() {
      const response = await getRealtimeData()
      if (response.code === 200) {
        this.updateRealtimeChart(response.data)
      }
    },
    
    updateRealtimeChart(data) {
      const option = {
        title: {
          text: '实时系统监控',
          textStyle: { color: '#333', fontSize: 14 }
        },
        tooltip: {
          trigger: 'axis',
          axisPointer: { type: 'cross' }
        },
        legend: {
          data: ['CPU使用率', '内存使用率', 'CPU温度']
        },
        xAxis: {
          type: 'category',
          data: data.timeLabels || []
        },
        yAxis: [
          {
            type: 'value',
            name: '使用率(%)',
            min: 0,
            max: 100
          },
          {
            type: 'value',
            name: '温度(°C)',
            min: 0,
            max: 100
          }
        ],
        series: [
          {
            name: 'CPU使用率',
            type: 'line',
            data: data.cpuUsage || [],
            itemStyle: { color: '#409EFF' }
          },
          {
            name: '内存使用率',
            type: 'line',
            data: data.memoryUsage || [],
            itemStyle: { color: '#67C23A' }
          },
          {
            name: 'CPU温度',
            type: 'line',
            yAxisIndex: 1,
            data: data.cpuTemp || [],
            itemStyle: { color: '#E6A23C' }
          }
        ]
      }
      this.charts.realtime.setOption(option)
    },
    
    async refreshRealtimeData() {
      await this.loadRealtimeData()
      this.$message.success('数据已刷新')
    },
    
    startAutoRefresh() {
      this.refreshTimer = setInterval(() => {
        this.loadData()
      }, 30000) // 30秒刷新一次
    },
    
    stopAutoRefresh() {
      if (this.refreshTimer) {
        clearInterval(this.refreshTimer)
        this.refreshTimer = null
      }
    }
  }
}
</script>

<style scoped>
.monitor-dashboard {
  padding: 20px;
  background: #f5f5f5;
  min-height: calc(100vh - 84px);
}

.overview-cards {
  margin-bottom: 20px;
}

.overview-card {
  border-radius: 8px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.overview-card.online {
  border-left: 4px solid #67C23A;
}

.overview-card.offline {
  border-left: 4px solid #F56C6C;
}

.overview-card.warning {
  border-left: 4px solid #E6A23C;
}

.overview-card.health {
  border-left: 4px solid #409EFF;
}

.card-content {
  display: flex;
  align-items: center;
}

.card-icon {
  font-size: 40px;
  margin-right: 20px;
  color: #409EFF;
}

.card-info {
  flex: 1;
}

.card-title {
  font-size: 14px;
  color: #666;
  margin-bottom: 8px;
}

.card-value {
  font-size: 24px;
  font-weight: bold;
  color: #333;
  margin-bottom: 4px;
}

.card-trend {
  font-size: 12px;
  color: #999;
}

.card-trend.up {
  color: #67C23A;
}

.card-trend.down {
  color: #F56C6C;
}

.chart-section {
  margin-bottom: 20px;
}

.chart-card {
  border-radius: 8px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
</style>