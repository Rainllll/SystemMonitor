# SystemMonitor Web 监控平台部署指南

## 📋 项目概述

基于若依(RuoYi-Vue)框架开发的系统监控Web平台，提供实时监控、历史数据查询、预警管理和大屏展示功能。

## 🚀 快速开始

### 1. 环境准备

#### 必需环境
- **JDK**: 1.8+
- **MySQL**: 5.7+ 或 8.0+
- **Redis**: 3.0+
- **Maven**: 3.6+
- **Node.js**: 14.0+
- **npm**: 6.0+

#### 开发工具推荐
- **IDE**: IntelliJ IDEA 或 Eclipse
- **数据库工具**: Navicat 或 DataGrip
- **前端工具**: VS Code

### 2. 数据库初始化

#### 2.1 创建数据库
```sql
CREATE DATABASE ry CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
```

#### 2.2 导入基础数据
按顺序执行以下SQL文件：
```bash
# 1. 导入若依基础数据
mysql -u root -p ry < sql/ry_20240101.sql

# 2. 导入监控模块表结构
mysql -u root -p ry < sql/monitor_tables.sql

# 3. 导入监控模块菜单权限
mysql -u root -p ry < sql/monitor_menu.sql
```

### 3. 后端配置

#### 3.1 修改数据库配置
编辑 `ruoyi-admin/src/main/resources/application-druid.yml`：

```yaml
spring:
  datasource:
    druid:
      master:
        url: jdbc:mysql://localhost:3306/ry?useUnicode=true&characterEncoding=utf8&zeroDateTimeBehavior=convertToNull&useSSL=true&serverTimezone=GMT%2B8
        username: root
        password: your_password
```

#### 3.2 修改Redis配置
编辑 `ruoyi-admin/src/main/resources/application.yml`：

```yaml
redis:
  host: localhost
  port: 6379
  password: your_redis_password
  database: 0
```

#### 3.3 启动后端服务
```bash
# 进入项目根目录
cd web-monitor

# 编译项目
mvn clean install

# 启动服务
cd ruoyi-admin
mvn spring-boot:run
```

或者在IDE中直接运行 `RuoYiApplication.java`

### 4. 前端配置

#### 4.1 安装依赖
```bash
cd ruoyi-ui
npm install
```

#### 4.2 修改API地址
编辑 `ruoyi-ui/.env.development`：

```env
# 开发环境配置
ENV = 'development'

# 若依管理系统/开发环境
VUE_APP_BASE_API = 'http://localhost:8080'
```

#### 4.3 启动前端服务
```bash
npm run dev
```

### 5. 访问系统

- **前端地址**: http://localhost:80
- **后端地址**: http://localhost:8080
- **默认账号**: admin / admin123

## 📊 功能模块说明

### 5.1 监控大屏
- **路径**: `/monitor/dashboard`
- **功能**: 
  - 系统概览数据展示
  - 实时监控图表
  - 设备状态分布饼图
  - 预警统计
  - 资源使用趋势

### 5.2 实时监控
- **路径**: `/monitor/realtime`
- **功能**:
  - 实时系统指标监控
  - 多设备切换
  - 自定义刷新间隔
  - 折线图趋势展示
  - 网络流量监控

### 5.3 历史数据
- **路径**: `/monitor/history`
- **功能**:
  - 历史数据查询
  - 时间范围筛选
  - 数据导出
  - 图表分析

### 5.4 预警管理
- **路径**: `/monitor/warning`
- **功能**:
  - 预警信息查询
  - 预警处理
  - 预警统计
  - 预警规则配置

### 5.5 设备管理
- **路径**: `/monitor/device`
- **功能**:
  - 设备信息管理
  - 设备状态监控
  - 设备配置

### 5.6 阈值配置
- **路径**: `/monitor/threshold`
- **功能**:
  - 监控阈值设置
  - 预警规则配置
  - 阈值模板管理

## 🔧 开发指南

### 6.1 添加新的监控指标

1. **数据库层面**:
   - 在 `monitor_metrics` 表中添加新字段
   - 更新相关视图和索引

2. **后端层面**:
   - 更新 `MonitorMetrics` 实体类
   - 修改 Mapper 文件
   - 更新 Service 业务逻辑

3. **前端层面**:
   - 更新 API 接口
   - 修改页面组件
   - 添加图表展示

### 6.2 自定义图表

使用 ECharts 库进行图表开发：

```javascript
// 示例：创建自定义折线图
const option = {
  title: { text: '自定义监控图表' },
  tooltip: { trigger: 'axis' },
  xAxis: { type: 'category', data: timeLabels },
  yAxis: { type: 'value' },
  series: [{
    name: '监控指标',
    type: 'line',
    data: metricData
  }]
}
chart.setOption(option)
```

### 6.3 添加预警规则

1. 在 `monitor_threshold` 表中配置阈值
2. 在后端实现预警检测逻辑
3. 配置预警通知方式（邮件、短信等）

## 🐛 常见问题

### Q1: 后端启动失败
**A**: 检查以下配置：
- 数据库连接配置是否正确
- Redis 服务是否启动
- 端口是否被占用

### Q2: 前端页面空白
**A**: 检查以下问题：
- 后端服务是否正常启动
- API 地址配置是否正确
- 浏览器控制台是否有错误信息

### Q3: 图表不显示
**A**: 可能的原因：
- ECharts 库未正确加载
- 图表容器尺寸问题
- 数据格式不正确

### Q4: 实时数据不更新
**A**: 检查：
- WebSocket 连接是否正常
- 定时器是否正确设置
- 后端数据采集是否正常

## 📞 技术支持

如遇到问题，请：
1. 查看日志文件获取详细错误信息
2. 检查网络连接和防火墙设置
3. 确认所有依赖服务正常运行
4. 参考若依官方文档：http://doc.ruoyi.vip

## 📝 更新日志

### v1.0.0 (2024-01-01)
- 初始版本发布
- 实现基础监控功能
- 支持实时数据展示
- 提供预警管理功能