using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace librgc
{
    public class Mouse
    {
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern long mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 cButtons, Int32 dwExtraInfo);

        public const Int32 MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const Int32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const Int32 MOUSEEVENTF_LEFTUP = 0x0004;
        public const Int32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const Int32 MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const Int32 MOUSEEVENTF_MOVE = 0x0001;
        public const Int32 MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const Int32 MOUSEEVENTF_RIGHTUP = 0x0010;
        public const Int32 MOUSEEVENTF_WHEEL = 0x0800;

        public static void LeftClick()
        {
            LeftDown();
            LeftUp();
        }

        public static void SetCursorPos(Int32 x, Int32 y)
        {
            Position = new Point(x, y);
        }
        public static Point Position
        {
            get
            {
                return Cursor.Position;
            }
            set
            {
                Cursor.Position = value;
            }
        }

        public static void LeftDown()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        public static void LeftUp()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void LeftClick(Int32 x, Int32 y)
        {
            SetCursorPos(x, y);
            LeftClick();
        }

        public static void RightClick()
        {
            RightDown();
            RightUp();
        }

        public static void RightDown()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        }

        public static void RightUp()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void RightClick(Int32 x, Int32 y)
        {
            SetCursorPos(x, y);
            RightClick();
        }

        public static void MiddleDown()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
        }

        public static void MiddleUp()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
        }

        public static void MiddleClick()
        {
            MiddleDown();
            MiddleUp();
        }

        public static void MiddleClick(Int32 x, Int32 y)
        {
            SetCursorPos(x, y);
            MiddleClick();
        }

        public static void Scroll(int p)
        {
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, p, 0);
        }
    }
}