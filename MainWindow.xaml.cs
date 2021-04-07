using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Threading;

using Forms = System.Windows.Forms;

namespace BF1_AutoOpenBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClassKeysManager main_ClassKeysManager;     // 定义按键管理变量

        private bool isRun = false;
        private DispatcherTimer Time = null;

        public MainWindow()
        {
            InitializeComponent();

            Time = new DispatcherTimer();
            Time.Interval = TimeSpan.FromMilliseconds(10);
            Time.Tick += Time_Tick;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "[BFBAN] 战地1自动开战斗包小工具 v" + Application.ResourceAssembly.GetName().Version.ToString();

            main_ClassKeysManager = new ClassKeysManager();
            main_ClassKeysManager.AddKey(Forms.Keys.F9);
            main_ClassKeysManager.AddKey(Forms.Keys.F10);
            main_ClassKeysManager.KeyDownEvent += new ClassKeysManager.KeyHandler(MyKeyDownEvent);

            if (Time != null)
            {
                Time.Start();
            }
        }

        private void MyKeyDownEvent(int keyId, string keyName)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (CheckBoxIsRun.IsChecked == true)
                {
                    switch ((Forms.Keys)keyId)
                    {
                        case Forms.Keys.F9:
                            isRun = !isRun; ;
                            break;
                    }
                }
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isRun = false;

            if (Time != null)
            {
                Time.Stop();
            }

            if (ClassKeysManager.thread != null)
            {
                ClassKeysManager.thread.Abort();
            }

            Application.Current.Shutdown();
        }

        void Time_Tick(object sender, EventArgs e)
        {
            if (isRun)
            {
                Delay(Convert.ToInt32(SliderSleep.Value));
                ClassManaged.mouse_event(ClassManaged.MOUSEEVENTF_LEFTDOWN | ClassManaged.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

                Delay(Convert.ToInt32(SliderSleep.Value));
                ClassManaged.Keybd_event(Forms.Keys.Enter, ClassManaged.MapVirtualKey(Forms.Keys.Enter, 0), 0, 0);
                Delay(Convert.ToInt32(SliderSleep.Value));
                ClassManaged.Keybd_event(Forms.Keys.Enter, ClassManaged.MapVirtualKey(Forms.Keys.Enter, 0), 2, 0);
            }
        }

        public static class DispatcherHelper
        {
            [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            public static void DoEvents()
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
                try { Dispatcher.PushFrame(frame); }
                catch (InvalidOperationException) { }
            }
            private static object ExitFrames(object frame)
            {
                ((DispatcherFrame)frame).Continue = false;
                return null;
            }
        }
        public static void Delay(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                DispatcherHelper.DoEvents();
            }
            return;
        }
    }
}
