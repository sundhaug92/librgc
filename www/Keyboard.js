var keyboardConnection = new WebSocket("ws://192.168.1.14:23091/keyboard");

function keyboardTap(VK) {
    Keyboard.Tap(VK);
}
function keyboardUp(VK) {
    Keyboard.Up(VK);
}
function keyboardDown(VK) {
    Keyboard.Down(VK);
}

var Keyboard = {
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

