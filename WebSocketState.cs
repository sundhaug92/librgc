namespace librgc
{
    internal class WebSocketState
    {
        private int _MouseX = 0, _MouseY;

        public bool MouseMode
        {
            get
            {
                return true;
            }
            set { }
        }
        public bool KeyboardMode
        {
            get
            {
                return false;
            }
            set { }
        }

        public int MouseX
        {
            get
            {
                return _MouseX;
            }
            set
            {
                _MouseX = value;
            }
        }

        public int MouseY
        {
            get
            {
                return _MouseY;
            }
            set
            {
                _MouseY = value;
            }
        }
    }
}