# 系统监控软件View层总结

## 1. 概述

系统监控软件的View层采用Windows Forms技术构建，作为用户与系统交互的直接接口，提供了直观、友好的操作界面。View层设计遵循MVC模式思想，将视图与业务逻辑分离，通过事件处理机制实现用户交互，支持实时监控、数据查询、图表分析等功能。

## 2. 主要窗体与控件

### 2.1 MainForm（主窗体）

MainForm是系统的主窗体，采用多标签页设计，包含以下五个主要功能模块：

#### 2.1.1 实时监控标签页（tabControlPanel）
- **功能**：展示系统资源的实时使用情况
- **主要控件**：
  - RichTextBox（txtLog）：显示系统日志信息
  - Button（btnStart/btnStop）：控制监控的启动和停止
  - TextBox（txtInterval）：设置数据采集间隔
  - Button（btnSetInterval）：应用采集间隔设置
  - Button（btnSetThresholds）：打开阈值设置窗体
  - Button（btnTestAlert）：测试预警功能

#### 2.1.2 预警查询标签页（tabWarnings）
- **功能**：查询系统产生的预警记录
- **主要控件**：
  - GroupBox（gbWarningQuery）：预警查询条件设置区
    - DateTimePicker（dtpWarningStart/dtpWarningEnd）：设置查询时间范围
    - Button（btnQueryWarnings）：执行预警查询
  - GroupBox（gbWarningData）：预警数据展示区
    - DataGridView（dataGridViewWarnings）：显示预警记录

#### 2.1.3 历史数据标签页（tabHistory）
- **功能**：查询历史监控数据
- **主要控件**：
  - GroupBox（gbHistoryQuery）：历史数据查询条件设置区
    - DateTimePicker（dtpStart/dtpEnd）：设置查询时间范围
    - Button（btnQueryHistory）：执行历史数据查询
  - GroupBox（gbHistoryData）：历史数据展示区
    - DataGridView（dataGridViewHistory）：显示历史数据记录

#### 2.1.4 大屏显示标签页（tabScreen）
- **功能**：以大屏形式展示系统实时状态
- **主要控件**：
  - GroupBox（gbScreenSettings）：大屏设置区
    - TextBox（txtRefreshInterval）：设置刷新间隔
    - Button（btnRefreshInterval）：应用刷新间隔设置
  - GroupBox（gbScreenDisplay）：大屏显示区
    - Chart（chartPie）：显示系统资源使用率图表
    - Label（lblScreenCpu/lblScreenMemory/lblScreenTemp）：显示CPU、内存、温度实时数据
    - Label（lblScreenTime）：显示当前时间

#### 2.1.5 饼图分析标签页（tabPieChart）
- **功能**：提供对历史数据的可视化分析
- **主要控件**：
  - GroupBox（gbPieChartQuery）：饼图查询条件设置区
    - ComboBox（cmbMetricType）：选择指标类型（CPU使用率、可用内存、CPU温度）
    - DateTimePicker（dtpPieChartStart/dtpPieChartEnd）：设置查询时间范围
    - Button（btnQueryPieChart）：执行饼图数据查询
  - GroupBox（gbPieChartDisplay）：饼图显示区
    - Chart（chartTimeRangePie）：显示指标分布饼图
    - Label（lblPieChartStats）：显示饼图统计信息

### 2.2 ThresholdForm（阈值设置窗体）

ThresholdForm是用于设置系统预警阈值的窗体，包含以下控件：
- **输入控件**：
  - TextBox（txtCpuUsageWarn/txtCpuUsageDanger）：CPU使用率警告/危险阈值
  - TextBox（txtAvailMemoryWarn/txtAvailMemoryDanger）：可用内存警告/危险阈值
  - TextBox（txtCpuTempWarn/txtCpuTempDanger）：CPU温度警告/危险阈值
  - TextBox（txtVolatilityThreshold）：波动率阈值
- **按钮控件**：
  - Button（btnSave）：保存阈值设置
  - Button（btnCancel）：取消设置

### 2.3 AutoClosingMessageBox（自动关闭消息框）

AutoClosingMessageBox是一个静态类，实现了自动关闭的消息框功能，用于显示预警信息。主要特点：
- 使用Windows API实现消息框的自动关闭
- 支持设置显示时间超时后自动关闭
- 通过多线程技术避免阻塞主线程

## 3. 关键功能实现

### 3.1 实时数据采集与显示

系统通过定时器（System.Timers.Timer）实现定时数据采集：
```csharp
private void InitTimer()
{
    _timer = new System.Timers.Timer(_intervalSeconds * 1000);
    _timer.Elapsed += Timer_Elapsed;
    _timer.AutoReset = true;
}

private void Timer_Elapsed(object sender, ElapsedEventArgs e)
{
    CollectAndSaveData();
}
```

数据采集包括CPU使用率、可用内存和CPU温度：
```csharp
// CPU 使用率
float cpuUsage = cpuCounter.NextValue();

// 可用内存
var memCounter = new PerformanceCounter("Memory", "Available MBytes");
float availableMemory = memCounter.NextValue();

// CPU 温度
double cpuTemp = -1;
foreach (IHardware hardware in computer.Hardware)
{
    if (hardware.HardwareType == LibreHardwareMonitor.Hardware.HardwareType.Cpu)
    {
        hardware.Update();
        foreach (ISensor sensor in hardware.Sensors)
        {
            if (sensor.SensorType == LibreHardwareMonitor.Hardware.SensorType.Temperature)
            {
                cpuTemp = sensor.Value.GetValueOrDefault();
                break;
            }
        }
    }
    if (cpuTemp >= 0) break;
}
```

为确保线程安全，使用Invoke方法在UI线程上更新控件：
```csharp
if (!this.IsDisposed && this.IsHandleCreated)
{
    this.Invoke(new Action(() =>
    {
        txtLog.AppendText(log + Environment.NewLine);
        UpdateRealTimeDisplay((float)cpuUsage, (float)availableMemory, (float)cpuTemp);
    }));
}
```

### 3.2 数据查询与展示

数据查询功能通过SQL语句从数据库获取数据，使用MySqlDataAdapter填充DataTable，并将结果绑定到DataGridView控件：
```csharp
string sql = "SELECT collect_time, cpu_usage, avail_memory, cpu_temp FROM t_metrics WHERE collect_time BETWEEN @startTime AND @endTime";
using (var connection = new MySqlConnection(_connectionString))
{
    connection.Open();
    using (var command = new MySqlCommand(sql, connection))
    {
        command.Parameters.AddWithValue("@startTime", startTime);
        command.Parameters.AddWithValue("@endTime", endTime);
        
        using (var adapter = new MySqlDataAdapter(command))
        {
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridViewHistory.DataSource = dt;
        }
    }
}
```

### 3.3 图表数据更新

图表数据更新功能实现了系统资源使用率的可视化展示：
```csharp
// 清空现有数据点
chartPie.Series["CPU使用率"].Points.Clear();
chartPie.Series["内存使用率"].Points.Clear();
chartPie.Series["CPU温度"].Points.Clear();

// 添加新数据点到图表
for (int i = 0; i < timeArray.Length; i++)
{
    chartPie.Series["CPU使用率"].Points.AddXY(timeArray[i], cpuUsageArray[i]);
    chartPie.Series["内存使用率"].Points.AddXY(timeArray[i], memoryUsageArray[i]);
    chartPie.Series["CPU温度"].Points.AddXY(timeArray[i], cpuTempArray[i]);
}
```

### 3.4 阈值设置与预警检测

阈值设置通过ThresholdForm窗体实现：
```csharp
private void btnSetThresholds_Click(object sender, EventArgs e)
{
    var thresholdForm = new ThresholdForm(_cpuUsageWarn, _cpuUsageDanger, 
                                         _availMemoryWarn, _availMemoryDanger, 
                                         _cpuTempWarn, _cpuTempDanger, 
                                         _volatilityThreshold);
    
    if (thresholdForm.ShowDialog() == DialogResult.OK)
    {
        // 更新阈值设置
        _cpuUsageWarn = thresholdForm.CpuUsageWarn;
        _cpuUsageDanger = thresholdForm.CpuUsageDanger;
        _availMemoryWarn = thresholdForm.AvailMemoryWarn;
        _availMemoryDanger = thresholdForm.AvailMemoryDanger;
        _cpuTempWarn = thresholdForm.CpuTempWarn;
        _cpuTempDanger = thresholdForm.CpuTempDanger;
        _volatilityThreshold = thresholdForm.VolatilityThreshold;
    }
}
```

预警检测在数据采集过程中进行：
```csharp
private void CheckAlerts(string deviceId, DateTime collectTime, double cpuUsage, double availableMemory, double cpuTemp)
{
    // CPU使用率预警
    if (cpuUsage >= _cpuUsageDanger)
    {
        string alertMsg = $"CPU使用率达到危险级别: {cpuUsage:F2}% (阈值: {_cpuUsageDanger}%)";
        Log(alertMsg);
        AutoClosingMessageBox.Show(alertMsg, "CPU使用率危险预警", 5000);
        SaveWarningToDatabase(deviceId, collectTime, "CPU使用率", cpuUsage, _cpuUsageDanger, "危险");
    }
    else if (cpuUsage >= _cpuUsageWarn)
    {
        string alertMsg = $"CPU使用率达到警告级别: {cpuUsage:F2}% (阈值: {_cpuUsageWarn}%)";
        Log(alertMsg);
        AutoClosingMessageBox.Show(alertMsg, "CPU使用率警告", 5000);
        SaveWarningToDatabase(deviceId, collectTime, "CPU使用率", cpuUsage, _cpuUsageWarn, "警告");
    }
    
    // 其他指标预警检测...
}
```

## 4. 界面设计特点

### 4.1 布局设计

系统界面采用标准Windows应用程序布局：
- 顶部为标题栏和基本控制按钮
- 主体区域为多标签页控件（TabControl），组织不同功能模块
- 每个标签页内部分为功能设置区和数据显示区
- 底部为状态栏，显示系统状态信息

### 4.2 交互设计

- **标签页切换**：通过TabControl实现不同功能模块间的切换
- **按钮操作**：提供明确的按钮操作，如启动/停止、查询、设置等
- **数据输入**：使用TextBox、ComboBox、DateTimePicker等控件实现数据输入
- **数据展示**：使用DataGridView、Chart等控件实现数据可视化展示

### 4.3 视觉设计

- **颜色方案**：采用Windows标准配色方案，界面简洁明了
- **数据可视化**：通过图表控件实现数据的直观展示
- **状态指示**：通过颜色、图标等方式表示不同状态（如警告、危险等）

## 5. 技术亮点

### 5.1 多线程处理

系统使用多线程技术确保UI响应性能：
- 数据采集在后台线程进行，避免阻塞UI线程
- 使用Invoke方法确保UI更新在UI线程执行
- AutoClosingMessageBox使用独立线程显示，避免阻塞主线程

### 5.2 数据绑定

系统利用Windows Forms的数据绑定功能简化数据展示：
- 将DataTable绑定到DataGridView，自动显示查询结果
- 图表数据通过动态添加数据点实现实时更新

### 5.3 异常处理

系统实现了完善的异常处理机制：
- 数据库操作使用try-catch块捕获异常
- 通过Logger类记录异常信息到日志文件
- 友好的错误提示，避免直接显示异常堆栈信息

### 5.4 资源管理

系统注重资源管理，避免资源泄漏：
- 使用using语句确保数据库连接等资源正确释放
- 定时器在窗体关闭时正确停止和释放
- 硬件监控组件在适当时候关闭和释放

## 6. 总结

系统监控软件的View层通过Windows Forms技术实现了功能完善、界面友好的用户界面。View层设计遵循MVC模式，将视图与业务逻辑分离，通过事件处理机制实现用户交互。系统采用多标签页设计组织不同功能模块，提供了实时监控、数据查询、图表分析等功能。通过多线程处理、数据绑定、异常处理和资源管理等技术，确保了系统的稳定性、响应性和用户体验。View层的实现为用户提供了直观、便捷的操作界面，是系统监控软件的重要组成部分。