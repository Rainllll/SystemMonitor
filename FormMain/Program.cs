using System;
using System.Windows.Forms;

namespace FormMain
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.Log("程序开始启动");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 注释掉不兼容的代码
            // Application.SetHighDpiMode(HighDpiMode.SystemAware);
            
            try
            {
                Logger.Log("即将创建MainForm");
                // 创建并显示MainForm
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Logger.LogError("程序启动失败", ex);
            }
        }
    }
}
