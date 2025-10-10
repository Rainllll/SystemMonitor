# 第3章 系统的详细设计及实现（View层）

## 1、功能简介

本系统采用Windows Forms技术构建用户界面，实现了系统监控软件的View层设计。View层作为用户与系统交互的直接接口，提供了直观、友好的操作界面，支持实时监控、数据查询、图表分析等功能。界面设计遵循简洁明了、操作便捷的原则，通过标签页（TabControl）组织不同功能模块，使用户能够轻松切换和访问各项功能。

### 界面功能描述

系统主界面采用多标签页设计，包含以下五个主要功能模块：

1. **实时监控标签页**：展示系统资源的实时使用情况，包括CPU使用率、可用内存和CPU温度的实时数据，以及这些指标的实时趋势图。用户可设置数据刷新间隔，系统会自动定时更新显示内容。

2. **饼图分析标签页**：提供对历史数据的可视化分析功能。用户可选择指标类型（CPU使用率、可用内存、CPU温度）和时间范围，系统将生成相应指标的分布饼图，直观展示不同区间的数据占比情况。

3. **历史数据标签页**：支持按时间范围查询历史监控数据，查询结果以表格形式展示，包括采集时间、CPU使用率、可用内存和CPU温度等信息，便于用户回顾和分析历史系统状态。

4. **预警查询标签页**：用于查询系统产生的预警记录，用户可设置时间范围筛选预警信息，查询结果包括预警时间、预警内容、指标类型、当前值、预警级别和阈值等详细信息。

### 界面设计图

![系统主界面设计图](界面设计图.png)

系统主界面采用标准Windows应用程序布局，顶部为标题栏，主体区域为多标签页控件，底部为状态栏。每个标签页内部分为功能设置区和数据显示区，布局清晰，操作便捷。实时监控标签页中，数据以数字和图表双重形式展示，既直观又详细；其他查询类标签页采用条件设置区和结果展示区的分离设计，符合用户操作习惯。

## 2、技术实现

View层采用C#语言和Windows Forms框架开发，利用丰富的控件库实现了功能完善的用户界面。界面设计遵循MVC模式思想，将视图与业务逻辑分离，通过事件处理机制实现用户交互。在实现过程中，充分利用了Windows Forms的数据绑定功能，将界面控件与数据源关联，简化了数据展示和更新的代码实现。

### 关键技术实现

1. **界面初始化与控件布局**

界面初始化主要通过MainForm构造函数完成，包括组件初始化、性能计数器设置、硬件监控组件初始化和定时器配置。关键代码如下：

```csharp
public MainForm()
{
    InitializeComponent();
    // 初始化 CPU 使用率计数器
    cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    // 初始化 LibreHardwareMonitor
    computer = new Computer() { IsCpuEnabled = true };
    computer.Open();
    // 初始化定时器
    InitTimer();
}
```

2. **实时数据更新与显示**

实时数据更新通过定时器触发，采集系统指标数据后更新界面显示。为确保线程安全，使用Invoke方法在UI线程上更新控件。关键代码如下：

```csharp
private void UpdateRealTimeDisplay(float cpuUsage, float availMemory, float cpuTemp)
{
    if (this.InvokeRequired)
    {
        this.Invoke(new Action<float, float, float>(UpdateRealTimeDisplay), cpuUsage, availMemory, cpuTemp);
        return;
    }
    // 更新界面控件显示数据
    lblScreenCpu.Text = String.Format("{0:F1}%", cpuUsage);
    lblScreenMemory.Text = String.Format("{0:F1} MB", availMemory);
    lblScreenTemp.Text = String.Format("{0:F1} °C", cpuTemp);
}
```

3. **标签页切换事件处理**

标签页切换事件处理实现了不同功能模块间的切换逻辑，当切换到实时监控标签页时，立即获取最新系统指标数据并更新显示。关键代码如下：

```csharp
private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
{
    if (tabControl.SelectedTab == tabScreen)
    {
        // 获取最新的系统指标数据
        float cpuUsage = cpuCounter.NextValue();
        float availMemory = new PerformanceCounter("Memory", "Available MBytes").NextValue();
        // 更新实时数据显示
        UpdateRealTimeDisplay(cpuUsage, availMemory, cpuTemp);
    }
}
```

4. **数据查询与展示**

数据查询功能通过SQL语句从数据库获取历史数据，使用MySqlDataAdapter填充DataTable，并将结果绑定到DataGridView控件展示。关键代码如下：

```csharp
string sql = "SELECT collect_time, cpu_usage, avail_memory, cpu_temp FROM t_metrics WHERE collect_time BETWEEN @startTime AND @endTime";
using (var adapter = new MySqlDataAdapter(command))
{
    DataTable dt = new DataTable();
    adapter.Fill(dt);
    dataGridViewHistory.DataSource = dt;
}
```

5. **图表数据更新**

图表数据更新功能实现了系统资源使用率的实时趋势图展示，通过清空现有数据点并添加新的数据点来更新图表。关键代码如下：

```csharp
chartPie.Series["CPU使用率"].Points.Clear();
chartPie.Series["内存使用率"].Points.Clear();
chartPie.Series["CPU温度"].Points.Clear();
// 添加新数据点到图表
chartPie.Series["CPU使用率"].Points.AddXY(timeArray[i], cpuUsageArray[i]);
```

通过以上关键技术的实现，View层完成了系统监控软件的用户界面设计，实现了数据展示、用户交互和图表可视化等功能，为用户提供了直观、便捷的操作体验。