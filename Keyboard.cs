using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace librgc
{
    class Keyboard
    {
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public static void KeyDown(int VK)
        {
            byte key = (byte)VK;
            keybd_event((byte)key, 0, 0, 0);
        }

        public static void KeyUp(int VK)
        {
            byte key = (byte)VK;
            keybd_event((byte)key, 0, 0x7F, 0);
        }
        public static void KeyTap(int VK)
        {
            KeyDown(VK);
            KeyUp(VK);
        }
    }
}
