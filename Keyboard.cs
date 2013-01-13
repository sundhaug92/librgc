﻿using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace librgc
{
    class Keyboard
    {
        const uint MAPVK_VK_TO_VSC = 0x00;
        const uint MAPVK_VSC_TO_VK = 0x01;
        const uint MAPVK_VK_TO_CHAR = 0x02;
        const uint MAPVK_VSC_TO_VK_EX = 0x03;
        const uint MAPVK_VK_TO_VSC_EX = 0x04;

        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public static void KeyDown(int VK, byte scan = 0, int ExtraInfo = 0)
        {
            byte key = (byte)VK;
            var v = MapVirtualKey((uint)VK, MAPVK_VK_TO_CHAR);
            bool b = v != 0;
            int flags = b ? 1 : 0;
            Console.WriteLine("D {0}:{1}:{2}", key, scan,flags, ExtraInfo);
            keybd_event((byte)key, scan, flags, ExtraInfo);
        }

        public static void KeyUp(int VK, byte scan = 0, int ExtraInfo = 0)
        {
            byte key = (byte)VK;
            var v = MapVirtualKey((uint)VK, MAPVK_VK_TO_CHAR);
            bool b = v != 0;
            int flags = b ? 3 : 2;
            Console.WriteLine("U {0}:{1}:{2}", key, scan, flags, ExtraInfo);
            keybd_event((byte)key, scan, flags, ExtraInfo);
        }
        public static void KeyTap(int VK, byte scan = 0, int ExtraInfo = 0)
        {
            KeyDown(VK, (byte)MapVirtualKey((uint)VK, MAPVK_VK_TO_VSC), ExtraInfo);
            KeyUp(VK, (byte)MapVirtualKey((uint)VK, MAPVK_VK_TO_VSC), ExtraInfo);
        }
    }
}
