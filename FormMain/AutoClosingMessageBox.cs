using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FormMain
{
    public static class AutoClosingMessageBox
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        const UInt32 WM_CLOSE = 0x0010;

        public static void Show(string message, string caption, int timeout)
        {
            // 创建并启动一个新线程来显示消息框
            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                // 声明定时器变量
                System.Threading.Timer timer = null;
                
                // 创建定时器，在指定时间后关闭消息框
                timer = new System.Threading.Timer((state) =>
                {
                    // 查找消息框窗口
                    IntPtr hWnd = FindWindow(null, caption);
                    if (hWnd != IntPtr.Zero)
                    {
                        // 关闭消息框
                        SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    }
                    timer.Dispose();
                }, null, timeout, System.Threading.Timeout.Infinite);

                // 显示消息框
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // 如果消息框被手动关闭，也释放定时器
                timer.Dispose();
            });
            
            // 设置线程为STA模式，这是显示消息框所必需的
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }
    }
}