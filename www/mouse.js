var mouseConnection = new WebSocket("ws://192.168.1.14:23091/mouse");

function LeftClick() {
    mouseConnection.send("click/0");
}
function RightClick() {
    mouseConnection.send("click/1");
}
function MiddleClick() {
    mouseConnection.send("click/2");
}
var Mouse = {
    Connection: new WebSocket("ws://192.168.1.14:23091/mouse"),
    RequireConnectionUp: function (callback) {
        if (this.Connection.readyState == this.Connection.OPEN) {
            callback();
        }
        else {
            this.Connection = new WebSocket("ws://192.168.1.14:23091/keyboard");
            this.Connection.onopen(callback);
        }
    },
    Send: function (string) {
        this.RequireConnectionUp(function () {
            this.Connection.send(string);
        });
    },
    MiddleClick: function () {
        this.Click(2);
    },
    RightClick: function () {
        this.Click(1);
    },
    LeftClick:function(){
        this.Click(0);
    },
    Click:function(key){
        this.Send("click/" + key);
    },
    Up: function (key) {
        this.Send("up/" + key);
    },
    Down: function (key) {
        this.Send("down/" + key);
    },
    Reset: function () {
        this.Send("reset/");
    }
};