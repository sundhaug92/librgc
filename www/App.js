var App = {
    Launch: function (name) {
        Keyboard.Tap(Keyboard.VK.LWIN);
        Keyboard.Type(name + '\n');
    },
    Exit: function(){
        Keyboard.Down(Keyboard.VK.MENU);
        Keyboard.Tap(Keyboard.VK.F4)
        Keyboard.Up(Keyboard.VK.MENU);
    }
};