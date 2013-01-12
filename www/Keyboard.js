function keyboardTap(VK){
    keyboardConnection.send("tap/" + VK);
}
function keyboardUp(VK){
    keyboardConnection.send("up/" + VK);
}
function keyboardDown(VK){
    keyboardConnection.send("down/", VK);
}

var keyboardConnection = new WebSocket("ws://192.168.1.14:23091/keyboard");