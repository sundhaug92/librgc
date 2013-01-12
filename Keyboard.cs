using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace librgc
{
    class Keyboard
    {
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public static void KeyDown(int VK, byte scan = 0, int ExtraInfo = 0)
        {
            byte key = (byte)VK;
            keybd_event((byte)key, scan, 0, ExtraInfo);
        }

        public static void KeyUp(int VK, byte scan = 0, int ExtraInfo = 0)
        {
            byte key = (byte)VK;
            keybd_event((byte)key, scan, 0x7F, ExtraInfo);
        }
        public static void KeyTap(int VK, byte scan = 0, int ExtraInfo = 0)
        {
            KeyDown(VK,scan, ExtraInfo);
            KeyUp(VK, scan, ExtraInfo);
        }
    }
}
