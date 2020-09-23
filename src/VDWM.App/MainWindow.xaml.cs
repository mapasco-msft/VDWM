using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;

using VDWM.Core;
using System.Printing;
using System.ComponentModel;

namespace VDWM.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", EntryPoint = "RegisterHotKey")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private DesktopManager dwm;

        private const int SWITCH_HOT_KEY = 6; // control + shift
        private const int MOVE_HOT_KEY = 3; // control + alt
        private IntPtr windowHndl;

        private int[] keys = { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39 };

        public MainWindow()
        {
            InitializeComponent();
            dwm = new DesktopManager();
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            windowHndl = new WindowInteropHelper(this).Handle;
            for (int i = 0; i < keys.Length; i++)
            {
                RegisterHotKey(windowHndl, i, SWITCH_HOT_KEY, keys[i]);
                RegisterHotKey(windowHndl, i+keys.Length, MOVE_HOT_KEY, keys[i]);
            }
            var source = HwndSource.FromHwnd(windowHndl);
            source.AddHook(WndProc);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                UnregisterHotKey(windowHndl, i);
                UnregisterHotKey(windowHndl, i+keys.Length);
            }
            base.OnClosing(e);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            if (msg == 0x0312)
            {

                int actionId = lParam.ToInt32();
                int keyId = wParam.ToInt32();
                int vdId = keyId > keys.Length ? keyId - keys.Length : keyId;
                int action = (actionId & 0xF);
                if (action == SWITCH_HOT_KEY)
                {
                    dwm.ChangeToVD(vdId);
                } else if (action == MOVE_HOT_KEY)
                {
                    dwm.MoveWindowToVD(vdId);
                    dwm.ChangeToVD(vdId);
                }
                handled = true;
            }

            return IntPtr.Zero;
        }


    }
}
