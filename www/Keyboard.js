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
    Connection: new WebSocket("ws://" + window.location.host + "/keyboard"),
    RequireConnectionUp: function (callback) {
        if (this.Connection.readyState == this.Connection.OPEN) {
            callback();
        }
        else {
            Keyboard.Connection = new WebSocket(Keyboard.Connection.url);
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
        LWIN: '5B',
        RWIN: '5C',
        APPS: '5D',
        NUMPAD0:'60',
        NUMPAD1:'61',
        NUMPAD2:'62',
        NUMPAD3:'63',
        NUMPAD4:'64',
        NUMPAD5:'65',
        NUMPAD6:'66',
        NUMPAD7:'67',
        NUMPAD8:'68',
        NUMPAD9:'69',
        MULTIPLY: '6A',
        ADD: '6B',
        SEPARATOR: '6C',
        SUBTRACT: '6D',
        DECIMAL: '6E',
        DIVIDE: '6F',
        F1: '70',
        F2: '71',
        F3: '72',
        F4: '73',
        F5: '74',
        F6: '75',
        F7: '76',
        F8: '77',
        F9: '78',
        F10: '79',
        F11: '7A',
        F12: '7B',
        F13: '7C',
        F14: '7D',
        F15: '7E',
        F16: '7F',
        F17: '80',
        F18: '81',
        F19: '82',
        F20: '83',
        F21: '84',
        F22: '85',
        F23: '86',
        F24: '87',

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
    },
    SpeedTest: {
        Type: function () {
            for (var i = 0; i < 10000; i++) {
                Keyboard.Type("nanan\n");
            }
        }
    }
};