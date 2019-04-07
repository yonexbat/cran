importScripts('./ngsw-worker.js');

var basePath = 'https://cranium.azurewebsites.net/';

function getUrl(action, data) {
    let result = basePath;
    if(action && data) {
        
        switch(action)
        {
            case "gotoquestion":
                result = result + "jsclient/viewquestion/" + data.questionid;
        }
    }
    return result;
}

self.addEventListener('notificationclick', (event) => {
    console.log('[Service Worker] Notification click Received. event', event);
    var url = getUrl(event.action, event.notification.data);
    event.notification.close();
    event.waitUntil(clients.openWindow(url));
});