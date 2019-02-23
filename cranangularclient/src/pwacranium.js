importScripts('./ngsw-worker.js');
self.addEventListener('notificationclick', (event) => {
    console.log('[Service Worker] Notification click Received. event', event);
    event.notification.close();
    event.waitUntil(clients.openWindow('https://cranium.azurewebsites.net/'));    
});