importScripts('./ngsw-worker.js');

var basePath = 'https://cranium.azurewebsites.net/';

function getUrl(action, data) {
    let result = basePath;
    if(data && data.url) {        
        return data.url;
    }
    return result;
}

self.addEventListener('notificationclick', (event) => {
    console.log('[Service Worker] Notification click received. event', event);
    var url = getUrl(event.action, event.notification.data);
    event.notification.close();
    event.waitUntil(clients.openWindow(url));
});