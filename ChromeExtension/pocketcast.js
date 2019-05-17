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
                document.getElementsByClassName("play_pause_button")[0].click();
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
