importScripts('./ngsw-worker.js');

var basePath = 'https://cranium.azurewebsites.net/';

function getUrl(data) {
    let result = basePath;
    if(data.url) {        
        return data.url;
    }
    return result;
}

self.addEventListener('notificationclick', (event) => {
    console.log('[Service Worker] Notification click received. event', event);
    var data = {};
    if(event.notification && event.notification.data)
    {
        data =  event.notification.data;
    }
    var url = getUrl(data);    
    event.waitUntil(clients.openWindow(url));
});