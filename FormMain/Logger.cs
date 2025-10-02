using System;
using System.IO;

namespace FormMain
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\debug.log");

        static Logger()
        {
            // 确保日志文件存在
            if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath).Dispose();
            }
        }

        public static void Log(string message)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}{Environment.NewLine}";
                File.AppendAllText(LogFilePath, logMessage);
            }
            catch (Exception ex)
            {
                // 如果日志记录失败，至少在控制台输出
                Console.WriteLine($"日志记录失败: {ex.Message}");
                Console.WriteLine($"原始消息: {message}");
            }
        }

        public static void LogError(string errorMessage, Exception ex = null)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] 错误: {errorMessage}{Environment.NewLine}";
                
                if (ex != null)
                {
                    logMessage += $"异常信息: {ex.Message}{Environment.NewLine}";
                    logMessage += $"堆栈跟踪: {ex.StackTrace}{Environment.NewLine}";
                }
                
                File.AppendAllText(LogFilePath, logMessage);
            }
            catch (Exception logEx)
            {
                // 如果日志记录失败，至少在控制台输出
                Console.WriteLine($"错误日志记录失败: {logEx.Message}");
                Console.WriteLine($"原始错误消息: {errorMessage}");
                if (ex != null)
                {
                    Console.WriteLine($"原始异常: {ex.Message}");
                }
            }
        }
    }
}