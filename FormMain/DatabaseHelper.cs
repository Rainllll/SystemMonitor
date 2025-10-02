using MySqlConnector;
using System;
using System.Configuration;
using System.Data;

namespace FormMain
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            // 从配置文件中获取连接字符串，如果没有则使用默认值
            var connectionSetting = System.Configuration.ConfigurationManager.ConnectionStrings["MySqlConnection"];
            if (connectionSetting != null)
            {
                connectionString = connectionSetting.ConnectionString;
            }
            else
            {
                connectionString = "server=localhost;user id=root;password=;database=system_monitor;";
            }
        }

        public DatabaseHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public void TestConnection()
        {
            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    // 连接成功，不做任何操作
                }
                catch (Exception ex)
                {
                    throw new Exception("数据库连接失败: " + ex.Message);
                }
            }
        }

        public MySqlDataReader ExecuteQuery(string query)
        {
            MySqlConnection connection = GetConnection();
            MySqlCommand command = new MySqlCommand(query, connection);
            
            try
            {
                connection.Open();
                return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception("查询执行失败: " + ex.Message);
            }
        }

        public int ExecuteNonQuery(string query)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                
                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("执行失败: " + ex.Message);
                }
            }
        }

        public object ExecuteScalar(string query)
        {
            using (MySqlConnection connection = GetConnection())
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                
                try
                {
                    connection.Open();
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception("执行失败: " + ex.Message);
                }
            }
        }
    }
}