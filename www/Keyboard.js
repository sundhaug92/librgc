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
    Tap: function (VK) {
        keyboardConnection.send("tap/" + VK);
    },
    Up: function (VK) {
        keyboardConnection.send("up/" + VK);
    },
    Down: function (VK) {
        keyboardConnection.send("down/" + VK);
    }
};

