# CranAngularClient

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.2.4. 

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).
Before running the tests make sure you are serving the app via `ng serve`.

## Upgrading ##
Upgrading: `ng update @angular/cli @angular/core`

## Building client ##
`npm run installclientprod`

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

## Playing with webpush ##
web-push send-notification --endpoint=https://fcm.googleapis.com/fcm/send/e5kXWPqXaRI:APA91bGWa1Pd_Nxnj_nMhoK_kZaHhfyXAvGo3Ty1eI6SLsXG_eCiQ7pmNC-ymJ1k3gflJWLj0GZSx2JxuWHqHJdpqSMgGRPzMtuXGJVtn_2B4c198f-EuS6Z0qvU0_pixfD4q48DXz2O  --key=BLFquItY-oc5HtYhjLkeGmWNjmlMR8RO8wtK7XImvvroFUPnGPLxevu5yu16ADmh1uDNjZ_NPkONdCbRorGOt24 --auth=pZmugFrCbvTPHpgDCeyY8Q --payload="{'notification': {'title': 'Angular News','body': 'Newsletter Available!', 'vibrate': [100, 50, 100],  'data': {  'primaryKey': 1   },   'actions': [{'action': 'explore', 'title': 'Go to the site'  }]}}"  --vapid-subject=mailto:public@claude-glauser.ch --vapid-pubkey=BBexMQInwvBFQtqWi9Px9FrhnzEmp0drOs4nkYGcopy_0TQjJ5jUKn7dBDTor_Ma5--Oq8rsseRl2m-dN9iyazU --vapid-pvtkey=TgPuP3hErzuIjTYg_bcYCkOa0GvfGNNUbeiuQpipX3o

## .net core secrets ##
dotnet user-secrets list