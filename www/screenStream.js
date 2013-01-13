var screenStream = {
    Connection: new WebSocket("ws://" + window.location.host + "/screenstream"),
    RequireConnectionUp: function (callback) {
        if (this.Connection.readyState == this.Connection.OPEN) {
            callback();
        }
        else {
            screenStream.Connection = new WebSocket(screenStream.Connection.url);
            init();
            screenStream.Connection.onopen = callback;
        }
    },
    Init: function () {
        screenStream.Connection.binaryType = "arraybuffer";
        screenStream.Connection.onmessage = function () {
            var destinationCanvas = document.getElementById('screenCanvas');
            var destinationContext = destinationCanvas.getContext('2d');
            var image = new Image();
            image.onload = function () {
                destinationContext.clearRect(0, 0,
                   destinationCanvas.width, destinationCanvas.height);
                destinationContext.drawImage(image, 0, 0);
            }
            image.src = URL.createObjectURL(messageEvent.data);
        };
    }
};