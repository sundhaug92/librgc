using Alchemy.Classes;

namespace librgc
{
    public class WebSocketServer
    {
        private Alchemy.WebSocketServer wss = new Alchemy.WebSocketServer(8181)
        {
            OnReceive = OnReceive,
            OnSend = OnSend
        };

        public void Start()
        {
            wss.Start();
        }

        private static void OnReceive(UserContext context)
        {
            string s = context.DataFrame.ToString();
            if (context.Data == null)
            {
                context.Data = (object)new WebSocketState();
                ((WebSocketState)context.Data).MouseX = 1280 / 2;
                ((WebSocketState)context.Data).MouseY = 1024 / 2;
                Mouse.SetCursorPos(((WebSocketState)context.Data).MouseX, ((WebSocketState)context.Data).MouseY);
            }
            WebSocketState State = (WebSocketState)context.Data;
            foreach (char c in s)
            {
                if (State.MouseMode)
                {
                    if (c == 'M') State.MouseMode = true;
                    else if (c == 'K') State.KeyboardMode = true;
                    switch (c)
                    {
                        case 'Q':
                            Mouse.LeftClick();
                            break;
                        case 'E':
                            Mouse.RightClick();
                            break;
                        case 'W':
                            if (State.MouseY > 10) State.MouseY -= 1;
                            Mouse.SetCursorPos(State.MouseX, State.MouseY);
                            break;
                        case 'A':
                            if (State.MouseX > 10) State.MouseX -= 1;
                            Mouse.SetCursorPos(State.MouseX, State.MouseY);
                            break;
                        case 'S':
                            if (State.MouseY < (1024-10)) State.MouseY += 1;
                            Mouse.SetCursorPos(State.MouseX, State.MouseY);
                            break;
                        case 'D':
                            if (State.MouseX < (1280 - 10)) State.MouseX += 1;
                            Mouse.SetCursorPos(State.MouseX, State.MouseY);
                            break;
                        case 'X':
                            Mouse.Scroll(1);
                            break;
                        case 'Z':
                            Mouse.Scroll(-1);
                            break;
                        case 'Æ':
                            while (true)
                            {
                                while (State.MouseX > 0)
                                {
                                    State.MouseX--;
                                    Mouse.SetCursorPos(State.MouseX, State.MouseY);
                                    System.Threading.Thread.Sleep(1);
                                }
                                while (State.MouseX < 1280)
                                {
                                    State.MouseX++;
                                    Mouse.SetCursorPos(State.MouseX, State.MouseY);
                                    System.Threading.Thread.Sleep(1);
                                }
                            }
                        case 'R':
                            State.MouseX = 1280 / 2;
                            State.MouseY = 1024 / 2;
                            Mouse.SetCursorPos(State.MouseX, State.MouseY);
                            break;
                    }
                }
            }
            context.Data = (object)State;
        }

        private static void OnSend(UserContext context)
    {
        }
    }
}