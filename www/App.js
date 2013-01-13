var App = {
    Launch: function (name) {
        Keyboard.Tap(Keyboard.VK.LWIN);
        window.setTimeout(function () { Keyboard.Type(name + '\n') }, 100);
    },
    Exit: function(){
        Keyboard.Down(Keyboard.VK.MENU);
        Keyboard.Tap(Keyboard.VK.F4)
        Keyboard.Up(Keyboard.VK.MENU);
    }
};