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
            this.Connection = new WebSocket("ws://192.168.1.14:23091/keyboard");
            this.Connection.onopen(callback);
        }
    },
    Tap: function (VK) {
        this.Connection.send("tap/" + VK);
    },
    Up: function (VK) {
        this.Connection.send("up/" + VK);
    },
    Down: function (VK) {
        this.Connection.send("down/" + VK);
    }
};

