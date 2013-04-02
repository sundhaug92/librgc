function LeftClick() {
    Console.log("WARNING: Old API in use: LeftClick");
    Mouse.LeftClick();
}
function RightClick() {
    Console.log("WARNING: Old API in use: RightClick");
    Mouse.RightClick();
}
function MiddleClick() {
    Console.log("WARNING: Old API in use: MiddleClick");
    Mouse.MiddleClick();
}
var Mouse = {
    Connection: new WebSocket((isSecure() ? "wss://" : "ws://") + window.location.host + "/keyboard"),
    RequireConnectionUp: function (callback) {
        if (this.Connection.readyState == this.Connection.OPEN) {
            callback();
        }
        else {
            Mouse.Connection = new WebSocket(Mouse.Connection.url);
            Mouse.Connection.onopen = callback;
        }
    },
    Send: function (string) {
        this.RequireConnectionUp(function () {
            Mouse.Connection.send(string);
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
    },
    SetPosition: function (X, Y) {
        this.Send("position/" + X + "/" + Y);
    }
};