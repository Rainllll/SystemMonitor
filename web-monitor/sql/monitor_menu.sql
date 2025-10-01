-- 监控模块菜单权限SQL

-- 监控管理主菜单
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('监控管理', 0, 6, 'monitor', NULL, 1, 0, 'M', '0', '0', NULL, 'monitor', 'admin', sysdate(), '', NULL, '系统监控管理菜单');

-- 获取监控管理菜单ID
SET @monitorMenuId = LAST_INSERT_ID();

-- 监控大屏
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('监控大屏', @monitorMenuId, 1, 'dashboard', 'monitor/dashboard/index', 1, 0, 'C', '0', '0', 'monitor:dashboard:view', 'dashboard', 'admin', sysdate(), '', NULL, '监控大屏菜单');

-- 实时监控
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('实时监控', @monitorMenuId, 2, 'realtime', 'monitor/realtime/index', 1, 0, 'C', '0', '0', 'monitor:realtime:view', 'chart', 'admin', sysdate(), '', NULL, '实时监控菜单');

-- 历史数据
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('历史数据', @monitorMenuId, 3, 'history', 'monitor/history/index', 1, 0, 'C', '0', '0', 'monitor:history:view', 'date-range', 'admin', sysdate(), '', NULL, '历史数据菜单');

-- 预警管理
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('预警管理', @monitorMenuId, 4, 'warning', 'monitor/warning/index', 1, 0, 'C', '0', '0', 'monitor:warning:view', 'warning', 'admin', sysdate(), '', NULL, '预警管理菜单');

-- 设备管理
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('设备管理', @monitorMenuId, 5, 'device', 'monitor/device/index', 1, 0, 'C', '0', '0', 'monitor:device:view', 'server', 'admin', sysdate(), '', NULL, '设备管理菜单');

-- 阈值配置
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('阈值配置', @monitorMenuId, 6, 'threshold', 'monitor/threshold/index', 1, 0, 'C', '0', '0', 'monitor:threshold:view', 'slider', 'admin', sysdate(), '', NULL, '阈值配置菜单');

-- 获取各菜单ID用于添加按钮权限
SET @realtimeMenuId = (SELECT menu_id FROM sys_menu WHERE path = 'realtime' AND parent_id = @monitorMenuId);
SET @historyMenuId = (SELECT menu_id FROM sys_menu WHERE path = 'history' AND parent_id = @monitorMenuId);
SET @warningMenuId = (SELECT menu_id FROM sys_menu WHERE path = 'warning' AND parent_id = @monitorMenuId);
SET @deviceMenuId = (SELECT menu_id FROM sys_menu WHERE path = 'device' AND parent_id = @monitorMenuId);
SET @thresholdMenuId = (SELECT menu_id FROM sys_menu WHERE path = 'threshold' AND parent_id = @monitorMenuId);

-- 历史数据按钮权限
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('历史数据查询', @historyMenuId, 1, '', '', 1, 0, 'F', '0', '0', 'monitor:history:query', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('历史数据导出', @historyMenuId, 2, '', '', 1, 0, 'F', '0', '0', 'monitor:history:export', '#', 'admin', sysdate(), '', NULL, '');

-- 预警管理按钮权限
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('预警查询', @warningMenuId, 1, '', '', 1, 0, 'F', '0', '0', 'monitor:warning:query', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('预警处理', @warningMenuId, 2, '', '', 1, 0, 'F', '0', '0', 'monitor:warning:handle', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('预警导出', @warningMenuId, 3, '', '', 1, 0, 'F', '0', '0', 'monitor:warning:export', '#', 'admin', sysdate(), '', NULL, '');

-- 设备管理按钮权限
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('设备查询', @deviceMenuId, 1, '', '', 1, 0, 'F', '0', '0', 'monitor:device:query', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('设备新增', @deviceMenuId, 2, '', '', 1, 0, 'F', '0', '0', 'monitor:device:add', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('设备修改', @deviceMenuId, 3, '', '', 1, 0, 'F', '0', '0', 'monitor:device:edit', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('设备删除', @deviceMenuId, 4, '', '', 1, 0, 'F', '0', '0', 'monitor:device:remove', '#', 'admin', sysdate(), '', NULL, '');

-- 阈值配置按钮权限
INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('阈值查询', @thresholdMenuId, 1, '', '', 1, 0, 'F', '0', '0', 'monitor:threshold:query', '#', 'admin', sysdate(), '', NULL, '');

INSERT INTO sys_menu (menu_name, parent_id, order_num, path, component, is_frame, is_cache, menu_type, visible, status, perms, icon, create_by, create_time, update_by, update_time, remark)
VALUES ('阈值修改', @thresholdMenuId, 2, '', '', 1, 0, 'F', '0', '0', 'monitor:threshold:edit', '#', 'admin', sysdate(), '', NULL, '');

-- 为管理员角色分配监控模块权限
INSERT INTO sys_role_menu (role_id, menu_id) 
SELECT 1, menu_id FROM sys_menu WHERE menu_name LIKE '%监控%' OR parent_id = @monitorMenuId;