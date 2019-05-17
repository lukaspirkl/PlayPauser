chrome.runtime.onInstalled.addListener(function () {
    chrome.storage.sync.set({ url: 'ws://127.0.0.1:8181' });
});