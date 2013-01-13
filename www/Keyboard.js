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
    Send: function (string) {
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
        BACKSPACE: '08',
        TAB: '09',

        ENTER: '0D',
        RETURN: '0D',

        SHIFT: '10',
        CONTROL: '11',
        MENU: '12',
        PAUSE: '13',
        CAPITAL: '14',
        CAPSLOCK: '14',

        ESCAPE: '1B',

        SPACE: '20',
        PRIOR: '21',
        PAGE_UP: '21',
        NEXT: '22',
        PAGE_DOWN: '22',
        END: '23',
        HOME: '24',
        LEFT: '25',
        UP: '26',
        RIGHT: '27',
        DOWN: '28',

        AT: '40',
        A: '41',
        B: '42',
        C: '43',
        D: '44',
        E: '45',
        F: '46',
        G: '47',
        H: '48',
        I: '49',
        J: '4A',
        K: '4B',
        L: '4C',
        M: '4D',
        N: '4E',
        O: '4F',
        P: '50',
        Q: '51',
        R: '52',
        S: '53',
        T: '54',
        U: '55',
        V: '56',
        W: '57',
        X: '58',
        Y: '59',
        Z: '5A',

        ADD: '6B',
        SEPARATOR: '6C',
        SUBTRACT: '6D',
        DECIMAL: '6E',
        DIVIDE: '6F',

        NUMLOCK: '90',
        SCROLLOCK: '91',

        VOLUME_MUTE: 'AD',
        VOLUME_DOWN: 'AE',
        VOLUME_UP: 'AF',
        MEDIA_NEXT_TRACK: 'B0',
        MEDIA_PREV_TRACK: 'B1',
        MEDIA_STOP: 'B2',
        MEDIA_PLAY_PAUSE: 'B3',
        LAUNCH_MAIL: 'B4',
        LAUNCH_MEDIA_SELECT: 'B5',
        LAUNCH_APP1: 'B6',
        LAUNCH_APP2: 'B7',

        OEM_PERIOD: 'BE'
    },
    Type: function (string) {
        var arr = string.split('');
        arr.forEach(function (element, index, array) {
            if ((element >= 'a') && (element <= 'z')) {
                Keyboard.Tap(element.toUpperCase().charCodeAt(0).toString(16));
            }
            else if ((element >= 'A') && (element <= 'Z')) {
                Keyboard.Tap(Keyboard.VK.CAPSLOCK);
                Keyboard.Type(element.toLowerCase());
                Keyboard.Tap(Keyboard.VK.CAPSLOCK);
            }
            else if (element == '.') {
                Keyboard.Tap(Keyboard.VK.OEM_PERIOD);
            }
            else if (element == ' ') {
                Keyboard.Tap(Keyboard.VK.SPACE);
            }
            else if (element == '\n') {
                Keyboard.Tap(Keyboard.VK.RETURN);
            }
        });
    }
};