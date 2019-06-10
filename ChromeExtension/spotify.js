(function () {

    'use strict';

    let ws = null;

    function start() {
        chrome.storage.sync.get('url', function (data) {
            ws = new WebSocket(data.url);

            ws.onopen = function () {
                console.log('PlayPauser - Connected');
            };

            ws.onmessage = function (e) {
                let play = document.getElementsByClassName("spoticon-play-16")[0];
                let pause = document.getElementsByClassName("spoticon-pause-16")[0];
                if (play) {
                    play.click();
                }
                else {
                    pause.click();
                }
            };

            ws.onclose = function () {
                console.log('PlayPauser - Disconnected');
            };
        });
    }

    function check() {
        if (!ws || ws.readyState == WebSocket.CLOSED) start();
    }

    start();

    setInterval(check, 5000);

    (function keepAlive() {
        var timeout = 20000;
        if (!!ws && ws.readyState == ws.OPEN) {
            ws.send('keepAlive');
        }
        setTimeout(keepAlive, timeout);
    })();

    chrome.storage.onChanged.addListener(function(changes) {
        if(!!changes.url)
        {
            console.log("PlayPauser - Url changed, disconnecting...");
            ws.close();
        }
    });

})();
