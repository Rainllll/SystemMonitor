<template>
  <div class="realtime-monitor">
    <!-- 设备选择器 -->
    <el-card class="device-selector">
      <el-row :gutter="20">
        <el-col :span="6">
          <el-select v-model="selectedDevice" placeholder="选择设备" @change="onDeviceChange">
            <el-option
              v-for="device in deviceList"
              :key="device.deviceId"
              :label="device.deviceName"
              :value="device.deviceId">
            </el-option>
          </el-select>
        </el-col>
        <el-col :span="6">
          <el-select v-model="refreshInterval" placeholder="刷新间隔" @change="onIntervalChange">
            <el-option label="5秒" :value="5000"></el-option>
            <el-option label="10秒" :value="10000"></el-option>
            <el-option label="30秒" :value="30000"></el-option>
            <el-option label="1分钟" :value="60000"></el-option>
          </el-select>
        </el-col>
        <el-col :span="12">
          <el-button type="primary" @click="startMonitor" :disabled="isMonitoring">
            <i class="el-icon-video-play"></i> 开始监控
          </el-button>
          <el-button type="danger" @click="stopMonitor" :disabled="!isMonitoring">
            <i class="el-icon-video-pause"></i> 停止监控
          </el-button>
          <el-button type="success" @click="refreshData">
            <i class="el-icon-refresh"></i> 刷新
          </el-button>
        </el-col>
      </el-row>
    </el-card>

    <!-- 实时数据卡片 -->
    <el-row :gutter="20" class="realtime-cards">
      <el-col :span="6">
        <el-card class="metric-card cpu">
          <div class="metric-content">
            <div class="metric-icon">
              <i class="el-icon-cpu"></i>
            </div>
            <div class="metric-info">
              <div class="metric-title">CPU使用率</div>
              <div class="metric-value">{{ currentData.cpuUsage }}%</div>
              <div class="metric-status" :class="getCpuStatus()">
                {{ getCpuStatusText() }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="metric-card memory">
          <div class="metric-content">
            <div class="metric-icon">
              <i class="el-icon-s-data"></i>
            </div>
            <div class="metric-info">
              <div class="metric-title">内存使用率</div>
              <div class="metric-value">{{ currentData.memoryUsage }}%</div>
              <div class="metric-status" :class="getMemoryStatus()">
                {{ getMemoryStatusText() }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="metric-card temperature">
          <div class="metric-content">
            <div class="metric-icon">
              <i class="el-icon-sunny"></i>
            </div>
            <div class="metric-info">
              <div class="metric-title">CPU温度</div>
              <div class="metric-value">{{ currentData.cpuTemp }}°C</div>
              <div class="metric-status" :class="getTempStatus()">
                {{ getTempStatusText() }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card class="metric-card disk">
          <div class="metric-content">
            <div class="metric-icon">
              <i class="el-icon-folder"></i>
            </div>
            <div class="metric-info">
              <div class="metric-title">磁盘使用率</div>
              <div class="metric-value">{{ currentData.diskUsage }}%</div>
              <div class="metric-status" :class="getDiskStatus()">
                {{ getDiskStatusText() }}
              </div>
            </div>
          </div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 实时图表 -->
    <el-row :gutter="20" class="chart-section">
      <el-col :span="24">
        <el-card class="chart-card">
          <div slot="header" class="card-header">
            <span>实时监控图表</span>
            <div class="chart-controls">
              <el-radio-group v-model="chartTimeRange" @change="onTimeRangeChange">
                <el-radio-button label="5m">5分钟</el-radio-button>
                <el-radio-button label="15m">15分钟</el-radio-button>
                <el-radio-button label="30m">30分钟</el-radio-button>
                <el-radio-button label="1h">1小时</el-radio-button>
              </el-radio-group>
            </div>
          </div>
          <div id="realtimeChart" style="height: 400px;"></div>
        </el-card>
      </el-col>
    </el-row>

    <!-- 网络流量图表 -->
    <el-row :gutter="20" class="chart-section">
      <el-col :span="12">
        <el-card class="chart-card">
          <div slot="header">
            <span>网络流量监控</span>
          </div>
          <div id="networkChart" style="height: 300px;"></div>
        </el-card>
      </el-col>
      <el-col :span="12">
        <el-card class="chart-card">
          <div slot="header">
            <span>系统资源分布</span>
          </div>
          <div id="resourcePieChart" style="height: 300px;"></div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import * as echarts from 'echarts'
import { getDeviceList, getRealtimeMetrics } from '@/api/monitor/realtime'

export default {
  name: 'RealtimeMonitor',
  data() {
    return {
      selectedDevice: '',
      deviceList: [],
      refreshInterval: 10000,
      chartTimeRange: '15m',
      isMonitoring: false,
      refreshTimer: null,
      currentData: {
        cpuUsage: 0,
        memoryUsage: 0,
        cpuTemp: 0,
        diskUsage: 0,
        networkIn: 0,
        networkOut: 0
      },
      charts: {},
      chartData: {
        timeLabels: [],
        cpuUsage: [],
        memoryUsage: [],
        cpuTemp: [],
        networkIn: [],
        networkOut: []
      }
    }
  },
  mounted() {
    this.initCharts()
    this.loadDeviceList()
  },
  beforeDestroy() {
    this.stopMonitor()
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
      
      // 初始化网络流量图表
      this.charts.network = echarts.init(document.getElementById('networkChart'))
      
      // 初始化资源分布饼图
      this.charts.resourcePie = echarts.init(document.getElementById('resourcePieChart'))
      
      // 设置图表自适应
      window.addEventListener('resize', () => {
        Object.values(this.charts).forEach(chart => {
          if (chart) {
            chart.resize()
          }
        })
      })
    },
    
    async loadDeviceList() {
      try {
        const response = await getDeviceList()
        if (response.code === 200) {
          this.deviceList = response.rows
          if (this.deviceList.length > 0) {
            this.selectedDevice = this.deviceList[0].deviceId
          }
        }
      } catch (error) {
        console.error('加载设备列表失败:', error)
      }
    },
    
    async loadRealtimeData() {
      if (!this.selectedDevice) return
      
      try {
        const response = await getRealtimeMetrics({
          deviceId: this.selectedDevice,
          timeRange: this.chartTimeRange
        })
        
        if (response.code === 200) {
          const data = response.data
          
          // 更新当前数据
          this.currentData = {
            cpuUsage: data.current.cpuUsage || 0,
            memoryUsage: data.current.memoryUsage || 0,
            cpuTemp: data.current.cpuTemp || 0,
            diskUsage: data.current.diskUsage || 0,
            networkIn: data.current.networkIn || 0,
            networkOut: data.current.networkOut || 0
          }
          
          // 更新图表数据
          this.updateChartData(data.history)
          this.updateCharts()
        }
      } catch (error) {
        console.error('加载实时数据失败:', error)
      }
    },
    
    updateChartData(historyData) {
      this.chartData = {
        timeLabels: historyData.timeLabels || [],
        cpuUsage: historyData.cpuUsage || [],
        memoryUsage: historyData.memoryUsage || [],
        cpuTemp: historyData.cpuTemp || [],
        networkIn: historyData.networkIn || [],
        networkOut: historyData.networkOut || []
      }
    },
    
    updateCharts() {
      this.updateRealtimeChart()
      this.updateNetworkChart()
      this.updateResourcePieChart()
    },
    
    updateRealtimeChart() {
      const option = {
        title: {
          text: '系统资源实时监控',
          textStyle: { color: '#333', fontSize: 16 }
        },
        tooltip: {
          trigger: 'axis',
          axisPointer: { type: 'cross' }
        },
        legend: {
          data: ['CPU使用率', '内存使用率', 'CPU温度']
        },
        grid: {
          left: '3%',
          right: '4%',
          bottom: '3%',
          containLabel: true
        },
        xAxis: {
          type: 'category',
          boundaryGap: false,
          data: this.chartData.timeLabels
        },
        yAxis: [
          {
            type: 'value',
            name: '使用率(%)',
            min: 0,
            max: 100,
            axisLabel: {
              formatter: '{value}%'
            }
          },
          {
            type: 'value',
            name: '温度(°C)',
            min: 0,
            max: 100,
            position: 'right',
            axisLabel: {
              formatter: '{value}°C'
            }
          }
        ],
        series: [
          {
            name: 'CPU使用率',
            type: 'line',
            smooth: true,
            data: this.chartData.cpuUsage,
            itemStyle: { color: '#409EFF' },
            areaStyle: { opacity: 0.3 }
          },
          {
            name: '内存使用率',
            type: 'line',
            smooth: true,
            data: this.chartData.memoryUsage,
            itemStyle: { color: '#67C23A' },
            areaStyle: { opacity: 0.3 }
          },
          {
            name: 'CPU温度',
            type: 'line',
            smooth: true,
            yAxisIndex: 1,
            data: this.chartData.cpuTemp,
            itemStyle: { color: '#E6A23C' }
          }
        ]
      }
      this.charts.realtime.setOption(option)
    },
    
    updateResourcePieChart() {
      const option = {
        title: {
          text: '系统资源使用情况',
          left: 'center',
          textStyle: { fontSize: 14 }
        },
        tooltip: {
          trigger: 'item',
          formatter: '{a} <br/>{b}: {c}% ({d}%)'
        },
        legend: {
          orient: 'vertical',
          left: 'left'
        },
        series: [
          {
            name: '资源使用率',
            type: 'pie',
            radius: ['40%', '70%'],
            avoidLabelOverlap: false,
            label: {
              show: false,
              position: 'center'
            },
            emphasis: {
              label: {
                show: true,
                fontSize: '18',
                fontWeight: 'bold'
              }
            },
            labelLine: {
              show: false
            },
            data: [
              { value: this.currentData.cpuUsage, name: 'CPU使用率', itemStyle: { color: '#409EFF' } },
              { value: this.currentData.memoryUsage, name: '内存使用率', itemStyle: { color: '#67C23A' } },
              { value: this.currentData.diskUsage, name: '磁盘使用率', itemStyle: { color: '#E6A23C' } },
              { value: 100 - Math.max(this.currentData.cpuUsage, this.currentData.memoryUsage, this.currentData.diskUsage), name: '空闲资源', itemStyle: { color: '#F56C6C' } }
            ]
          }
        ]
      }
      this.charts.resourcePie.setOption(option)
    },
    
    startMonitor() {
      this.isMonitoring = true
      this.loadRealtimeData()
      this.refreshTimer = setInterval(() => {
        this.loadRealtimeData()
      }, this.refreshInterval)
      this.$message.success('开始实时监控')
    },
    
    stopMonitor() {
      this.isMonitoring = false
      if (this.refreshTimer) {
        clearInterval(this.refreshTimer)
        this.refreshTimer = null
      }
      this.$message.info('停止实时监控')
    },
    
    refreshData() {
      this.loadRealtimeData()
      this.$message.success('数据已刷新')
    },
    
    onDeviceChange() {
      if (this.isMonitoring) {
        this.loadRealtimeData()
      }
    },
    
    onIntervalChange() {
      if (this.isMonitoring) {
        this.stopMonitor()
        this.startMonitor()
      }
    },
    
    onTimeRangeChange() {
      this.loadRealtimeData()
    },
    
    // 状态判断方法
    getCpuStatus() {
      const usage = this.currentData.cpuUsage
      if (usage >= 90) return 'danger'
      if (usage >= 80) return 'warning'
      return 'normal'
    },
    
    getCpuStatusText() {
      const usage = this.currentData.cpuUsage
      if (usage >= 90) return '危险'
      if (usage >= 80) return '警告'
      return '正常'
    }
  }
}
</script>

<style scoped>
.realtime-monitor {
  padding: 20px;
}

.device-selector {
  margin-bottom: 20px;
}

.realtime-cards {
  margin-bottom: 20px;
}

.metric-card {
  border-radius: 8px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.metric-card.cpu {
  border-left: 4px solid #409EFF;
}

.metric-card.memory {
  border-left: 4px solid #67C23A;
}

.metric-card.temperature {
  border-left: 4px solid #E6A23C;
}

.metric-card.disk {
  border-left: 4px solid #F56C6C;
}

.metric-content {
  display: flex;
  align-items: center;
}

.metric-icon {
  font-size: 36px;
  margin-right: 15px;
  color: #409EFF;
}

.metric-info {
  flex: 1;
}

.metric-title {
  font-size: 14px;
  color: #666;
  margin-bottom: 8px;
}

.metric-value {
  font-size: 24px;
  font-weight: bold;
  color: #333;
  margin-bottom: 4px;
}

.metric-status {
  font-size: 12px;
  padding: 2px 8px;
  border-radius: 4px;
}

.metric-status.normal {
  background: #f0f9ff;
  color: #67C23A;
}

.metric-status.warning {
  background: #fdf6ec;
  color: #E6A23C;
}

.metric-status.danger {
  background: #fef0f0;
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

.chart-controls {
  display: flex;
  align-items: center;
}
</style>