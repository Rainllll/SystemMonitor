# SystemMonitor

一个基于C# WinForms的实时系统监控应用程序，用于监控计算机的CPU使用率、内存使用情况和CPU温度，并提供预警功能和数据库存储。

## 🚀 功能特性

### 核心监控功能
- **CPU使用率监控**: 实时监控CPU使用率百分比
- **内存监控**: 监控系统可用内存（MB）
- **CPU温度监控**: 通过LibreHardwareMonitor库获取CPU温度
- **可配置采集周期**: 支持自定义数据采集间隔（默认1秒）

### 预警系统
- **多级阈值预警**: 支持警告和危险两级阈值设置
- **波动率监控**: 检测系统指标的异常波动
- **实时预警显示**: 在界面上高亮显示预警信息
- **预警数据存储**: 将预警信息保存到数据库

### 数据存储
- **MySQL数据库支持**: 将监控数据实时保存到MySQL数据库
- **本地日志记录**: 同时保存到本地文本文件
- **历史数据缓存**: 维护最近10次的监控数据用于波动率计算

### 用户界面
- **实时数据显示**: 在文本框中实时显示监控数据和日志
- **阈值配置界面**: 提供专门的窗口用于配置各种预警阈值
- **操作控制**: 支持启动/停止监控，修改采集周期

## 🛠️ 技术栈

- **开发框架**: .NET Framework 4.7.2
- **UI框架**: Windows Forms
- **数据库**: MySQL (使用MySqlConnector)
- **硬件监控**: LibreHardwareMonitor
- **系统监控**: Windows Performance Counters

## 📋 系统要求

- Windows 操作系统
- .NET Framework 4.7.2 或更高版本
- MySQL 数据库服务器
- 管理员权限（用于硬件监控）

## 🔧 安装配置

### 1. 环境准备

#### 安装.NET Framework
确保系统已安装.NET Framework 4.7.2或更高版本。

#### 安装MySQL数据库
1. 下载并安装MySQL Server
2. 创建数据库和相关表结构

### 2. 数据库配置

#### 创建数据库
```sql
CREATE DATABASE system_monitor;
USE system_monitor;
```

#### 创建数据表

**监控数据表**
```sql
CREATE TABLE t_metrics (
    id INT AUTO_INCREMENT PRIMARY KEY,
    device_id VARCHAR(50) NOT NULL,
    collect_time DATETIME NOT NULL,
    cpu_usage DOUBLE NOT NULL,
    avail_memory DOUBLE NOT NULL,
    cpu_temp DOUBLE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_device_time (device_id, collect_time)
);
```

**预警信息表**
```sql
CREATE TABLE t_warning (
    id INT AUTO_INCREMENT PRIMARY KEY,
    device_id VARCHAR(50) NOT NULL,
    warning_time DATETIME NOT NULL,
    warning_content TEXT NOT NULL,
    metric_type VARCHAR(50) NOT NULL,
    metric_value DOUBLE NOT NULL,
    threshold_type VARCHAR(20) NOT NULL,
    threshold_value DOUBLE NOT NULL,
    processing_status VARCHAR(20) DEFAULT 'UNHANDLED',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_device_time (device_id, warning_time)
);
```

**系统配置表**
```sql
CREATE TABLE t_sys_metadata (
    device_id VARCHAR(50) PRIMARY KEY,
    cpu_usage_warn DOUBLE DEFAULT 80.0,
    cpu_usage_danger DOUBLE DEFAULT 90.0,
    avail_memory_warn DOUBLE DEFAULT 500.0,
    avail_memory_danger DOUBLE DEFAULT 100.0,
    cpu_temp_warn DOUBLE DEFAULT 85.0,
    cpu_temp_danger DOUBLE DEFAULT 95.0,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 插入默认配置
INSERT INTO t_sys_metadata (device_id) VALUES ('local-pc-01');
```

### 3. 应用程序配置

#### 修改数据库连接字符串
在`MainForm.cs`文件中修改数据库连接配置：

```csharp
private string _connectionString = "server=localhost;database=system_monitor;user=root;password=your_password;port=3306;";
```

#### 配置LibreHardwareMonitor
确保`LibreHardwareMonitorLib.dll`文件位于正确的路径，或更新项目引用路径。

### 4. 编译运行

1. 使用Visual Studio打开`FormMain.sln`解决方案文件
2. 还原NuGet包依赖
3. 编译项目
4. 以管理员权限运行生成的可执行文件

## 📖 使用说明

### 启动监控
1. 运行应用程序
2. 点击"启动"按钮开始数据采集
3. 监控数据将实时显示在文本框中

### 配置采集周期
1. 在"采集周期"文本框中输入秒数（≥1）
2. 点击"设置周期"按钮应用新的采集间隔

### 配置预警阈值
1. 点击"设置阈值"按钮打开阈值配置窗口
2. 设置各项指标的警告和危险阈值：
   - CPU使用率警告/危险阈值（%）
   - 可用内存警告/危险阈值（MB）
   - CPU温度警告/危险阈值（°C）
   - 波动率阈值（%）
3. 点击"保存"按钮保存配置

### 停止监控
点击"停止"按钮停止数据采集。

## 📊 监控指标说明

### CPU使用率
- **单位**: 百分比(%)
- **数据源**: Windows Performance Counter
- **更新频率**: 根据设置的采集周期

### 可用内存
- **单位**: 兆字节(MB)
- **数据源**: Windows Performance Counter
- **说明**: 系统当前可用的物理内存大小

### CPU温度
- **单位**: 摄氏度(°C)
- **数据源**: LibreHardwareMonitor
- **说明**: CPU核心温度，需要硬件支持

### 波动率
- **单位**: 百分比(%)
- **计算方式**: |当前值 - 前一次值| / 前一次值 × 100%
- **用途**: 检测系统指标的异常波动

## ⚠️ 预警机制

### 阈值类型
- **WARN**: 警告级别，指标接近异常范围
- **DANGER**: 危险级别，指标已达到危险范围

### 预警触发条件
1. **CPU使用率**: 超过设定的警告或危险阈值
2. **可用内存**: 低于设定的警告或危险阈值
3. **CPU温度**: 超过设定的警告或危险阈值
4. **波动率**: 任一指标的波动率超过设定阈值

### 预警处理
- 在界面上以红色高亮显示预警信息
- 将预警信息保存到数据库的`t_warning`表
- 记录到本地日志文件

## 📁 项目结构

```
SystemMonitor/
├── FormMain.sln              # Visual Studio解决方案文件
├── README.md                 # 项目说明文档
├── FormMain/                 # 主项目目录
│   ├── Program.cs            # 程序入口点
│   ├── MainForm.cs           # 主窗体逻辑
│   ├── MainForm.Designer.cs  # 主窗体设计器代码
│   ├── MainForm.resx         # 主窗体资源文件
│   ├── ThresholdForm.cs      # 阈值配置窗体
│   ├── FormMain.csproj       # 项目文件
│   ├── packages.config       # NuGet包配置
│   ├── App.config            # 应用程序配置
│   ├── app.manifest          # 应用程序清单
│   └── Properties/           # 项目属性文件
└── packages/                 # NuGet包目录
```

## 🔍 故障排除

### 常见问题

#### 1. 无法获取CPU温度
- **原因**: 硬件不支持或缺少传感器驱动
- **解决**: 确保硬件支持温度监控，安装最新的主板驱动

#### 2. 数据库连接失败
- **原因**: 连接字符串配置错误或MySQL服务未启动
- **解决**: 检查数据库连接配置，确保MySQL服务正常运行

#### 3. 权限不足错误
- **原因**: 硬件监控需要管理员权限
- **解决**: 以管理员身份运行应用程序

#### 4. LibreHardwareMonitor相关错误
- **原因**: 缺少LibreHardwareMonitorLib.dll文件
- **解决**: 确保dll文件在正确位置，或重新下载LibreHardwareMonitor

### 日志文件
- **位置**: `D:\data_log.txt`（可在代码中修改路径）
- **内容**: 包含所有监控数据和错误信息
- **用途**: 用于问题诊断和数据分析

## 🤝 贡献指南

欢迎提交Issue和Pull Request来改进这个项目。

### 开发环境设置
1. 安装Visual Studio 2017或更高版本
2. 安装.NET Framework 4.7.2 SDK
3. 克隆项目到本地
4. 还原NuGet包依赖

### 代码规范
- 遵循C#编码规范
- 添加适当的注释
- 确保代码的可读性和可维护性

## 📄 许可证

本项目采用MIT许可证，详情请参阅LICENSE文件。

## 📞 联系方式

如有问题或建议，请通过以下方式联系：
- 提交GitHub Issue
- 发送邮件至项目维护者

---

**注意**: 本应用程序需要管理员权限才能正常获取硬件信息，请确保以管理员身份运行。