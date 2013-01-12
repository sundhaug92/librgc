function LeftClick() {
    mouseConnection.send("click/0");
}
function RightClick() {
    mouseConnection.send("click/1");

}
function MiddleClick() {
    mouseConnection.send("click/2");

}
var mouseConnection = new WebSocket("ws://192.168.1.14:23091/mouse");