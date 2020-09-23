using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using WindowsDesktop;

namespace VDWM.Core
{
    public class DesktopManager
    {
        private ICollection<VirtualDesktop> desktops
        {
            get => VirtualDesktop.GetDesktops();
        }

        private VirtualDesktop currentVirtualDesktop;
        public VirtualDesktop GetCurrentVirtualDesktop()
        {
            return currentVirtualDesktop = VirtualDesktop.FromHwnd(GetForegroundWindow());
        }

        public bool MoveWindowToVD(int desktopIdx)
        {
            var targetDesktop = desktops.ElementAt(desktopIdx - 1);
            VirtualDesktopHelper.MoveToDesktop(GetForegroundWindow(), targetDesktop);
            return true;
            
        }

        public bool ChangeToVD(int desktopIdx)
        {
            if (desktopIdx > desktops.Count)
            {

            }
            else
            {
                var targetDesktop = desktops.ElementAt(desktopIdx - 1);

                targetDesktop.Switch();
            }
            return true;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}
