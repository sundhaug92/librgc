function keyboardTap(key) {
    Console.log("WARNING: Old API in use: keyboardTap");
    Keyboard.Tap(key);
}
function keyboardUp(key) {
    Console.log("WARNING: Old API in use: keyboardUp");
    Keyboard.Up(key);
}
function keyboardDown(key) {
    Console.log("WARNING: Old API in use: keyboardDown");
    Keyboard.Down(key);
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
    Tap: function (key) {
        this.Send("tap/" + key);
    },
    Up: function (key) {
        this.Send("up/" + key);
    },
    Down: function (key) {
        this.Send("down/" + key);
    },
    VK: {
        VOLUME_MUTE: 'AD',
        VOLUME_DOWN: 'AE',
        VOLUME_UP: 'AF',
        MEDIA_NEXT_TRACK: 'B0',
        MEDIA_PREV_TRACK: 'B1',
        MEDIA_STOP: 'B2',
        MEDIA_PLAY_PAUSE: 'B3'
    },

};

