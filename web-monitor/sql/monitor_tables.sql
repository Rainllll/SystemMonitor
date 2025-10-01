-- 监控模块数据库表结构

-- 设备信息表
CREATE TABLE `monitor_device` (
  `device_id` varchar(50) NOT NULL COMMENT '设备ID',
  `device_name` varchar(100) NOT NULL COMMENT '设备名称',
  `device_type` varchar(50) DEFAULT 'PC' COMMENT '设备类型',
  `ip_address` varchar(50) DEFAULT NULL COMMENT 'IP地址',
  `location` varchar(200) DEFAULT NULL COMMENT '设备位置',
  `status` char(1) DEFAULT '1' COMMENT '状态(0=离线 1=在线)',
  `last_heartbeat` datetime DEFAULT NULL COMMENT '最后心跳时间',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  `remark` varchar(500) DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`device_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='监控设备信息表';

-- 监控指标数据表
CREATE TABLE `monitor_metrics` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `device_id` varchar(50) NOT NULL COMMENT '设备ID',
  `collect_time` datetime NOT NULL COMMENT '采集时间',
  `cpu_usage` decimal(5,2) NOT NULL COMMENT 'CPU使用率(%)',
  `memory_total` decimal(10,2) DEFAULT NULL COMMENT '总内存(MB)',
  `memory_used` decimal(10,2) DEFAULT NULL COMMENT '已用内存(MB)',
  `memory_available` decimal(10,2) NOT NULL COMMENT '可用内存(MB)',
  `cpu_temp` decimal(5,2) DEFAULT NULL COMMENT 'CPU温度(°C)',
  `disk_usage` decimal(5,2) DEFAULT NULL COMMENT '磁盘使用率(%)',
  `network_in` decimal(10,2) DEFAULT NULL COMMENT '网络入流量(KB/s)',
  `network_out` decimal(10,2) DEFAULT NULL COMMENT '网络出流量(KB/s)',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  KEY `idx_device_time` (`device_id`,`collect_time`),
  KEY `idx_collect_time` (`collect_time`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='监控指标数据表';

-- 预警信息表
CREATE TABLE `monitor_warning` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `device_id` varchar(50) NOT NULL COMMENT '设备ID',
  `warning_time` datetime NOT NULL COMMENT '预警时间',
  `warning_type` varchar(50) NOT NULL COMMENT '预警类型',
  `warning_level` varchar(20) NOT NULL COMMENT '预警级别(WARN/DANGER)',
  `warning_content` text NOT NULL COMMENT '预警内容',
  `metric_type` varchar(50) NOT NULL COMMENT '指标类型',
  `metric_value` decimal(10,2) NOT NULL COMMENT '指标值',
  `threshold_value` decimal(10,2) NOT NULL COMMENT '阈值',
  `status` char(1) DEFAULT '0' COMMENT '处理状态(0=未处理 1=已处理 2=已忽略)',
  `handle_by` varchar(64) DEFAULT NULL COMMENT '处理人',
  `handle_time` datetime DEFAULT NULL COMMENT '处理时间',
  `handle_remark` varchar(500) DEFAULT NULL COMMENT '处理备注',
  `create_time` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`),
  KEY `idx_device_time` (`device_id`,`warning_time`),
  KEY `idx_status` (`status`),
  KEY `idx_warning_level` (`warning_level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='监控预警信息表';

-- 阈值配置表
CREATE TABLE `monitor_threshold` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '主键ID',
  `device_id` varchar(50) NOT NULL COMMENT '设备ID',
  `metric_type` varchar(50) NOT NULL COMMENT '指标类型',
  `warn_threshold` decimal(10,2) NOT NULL COMMENT '警告阈值',
  `danger_threshold` decimal(10,2) NOT NULL COMMENT '危险阈值',
  `comparison_type` varchar(10) NOT NULL COMMENT '比较类型(GT=大于 LT=小于)',
  `enabled` char(1) DEFAULT '1' COMMENT '是否启用(0=否 1=是)',
  `create_by` varchar(64) DEFAULT '' COMMENT '创建者',
  `create_time` datetime DEFAULT NULL COMMENT '创建时间',
  `update_by` varchar(64) DEFAULT '' COMMENT '更新者',
  `update_time` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_device_metric` (`device_id`,`metric_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='监控阈值配置表';

-- 插入默认设备数据
INSERT INTO `monitor_device` (`device_id`, `device_name`, `device_type`, `ip_address`, `location`, `status`, `last_heartbeat`, `create_by`, `create_time`, `remark`) 
VALUES ('local-pc-01', '本地计算机-01', 'PC', '192.168.1.100', '办公室A区', '1', NOW(), 'admin', NOW(), '主要开发机器');

-- 插入默认阈值配置
INSERT INTO `monitor_threshold` (`device_id`, `metric_type`, `warn_threshold`, `danger_threshold`, `comparison_type`, `enabled`, `create_by`, `create_time`) VALUES
('local-pc-01', 'CPU_USAGE', 80.00, 90.00, 'GT', '1', 'admin', NOW()),
('local-pc-01', 'MEMORY_USAGE', 80.00, 90.00, 'GT', '1', 'admin', NOW()),
('local-pc-01', 'CPU_TEMP', 85.00, 95.00, 'GT', '1', 'admin', NOW()),
('local-pc-01', 'DISK_USAGE', 85.00, 95.00, 'GT', '1', 'admin', NOW()),
('local-pc-01', 'MEMORY_AVAILABLE', 500.00, 100.00, 'LT', '1', 'admin', NOW());