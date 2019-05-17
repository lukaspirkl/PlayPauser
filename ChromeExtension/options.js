let input = document.getElementById('url');
let button = document.getElementById('save');

chrome.storage.sync.get('url', function (data) {
    input.value = data.url;
});

button.addEventListener('click', function() {
    chrome.storage.sync.set({url: input.value});
});
