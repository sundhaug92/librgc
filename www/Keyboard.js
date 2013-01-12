function keyboardTap(VK) {
    Console.log("WARNING: Old API in use: keyboardTap");
    Keyboard.Tap(VK);
}
function keyboardUp(VK) {
    Console.log("WARNING: Old API in use: keyboardUp");
    Keyboard.Up(VK);
}
function keyboardDown(VK) {
    Console.log("WARNING: Old API in use: keyboardDown");
    Keyboard.Down(VK);
}

var Keyboard = {
    Connection: new WebSocket("ws://192.168.1.14:23091/keyboard"),
    RequireConnectionUp: function (callback) {
        if (this.Connection.readyState == this.Connection.OPEN) {
            callback();
        }
        else {
            Keyboard.Connection = new WebSocket("ws://192.168.1.14:23091/keyboard");
            Keyboard.Connection.onopen = callback;
        }
    },
    Send: function(string){
        this.RequireConnectionUp(function () {
            Keyboard.Connection.send(string);
        });
    },
    Tap: function (VK) {
        this.Send("tap/" + VK);
    },
    Up: function (VK) {
        this.Send("up/" + VK);
    },
    Down: function (VK) {
        this.Send("down/" + VK);
    }
};

