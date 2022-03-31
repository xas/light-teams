# Light Teams

Light Teams is an dotnet console application used to manage your Teams status and be able to display it.

## Hardware requirement

The application was tested on a Raspberry Pi Zero 2 with a Pimoroni [Blinkt!](https://shop.pimoroni.com/products/blinkt)

## Use

At this time, Microsoft does not provide a easy way for a user to create a token with its permission, and use it automatically in any application.  
The process is still a request from Microsoft, validating a code, and receive a token to use for a short period of time. And refresh it.  

Because of this, the start of the application is a little painful.  

* You launch the application, and a message to authorize the application is displayed (an url + a code)
* You launch your browser, login with your account, use the provided url, and validate the access with the code
* The application will receive a token to be able to query your Teams status and update the _Blinkt!_ display in consequence
* Red color if you're not available
* Green color if you're available
* Blu color for any unknown status (offline, unknown, ...)

The application will loop to refresh the token when its lifetime expire.

## Display errors

If the _Blinkt!_ is switched off, the application is not running.  

If the _Blinkt!_ show a white animation, this will indicate that the application is waiting for the user to authorize (again) the application to access your Teams status (step 2 in the use case)  

## Setup

To use the application, you will need to create an application registration on Azure.

* create an account on Azure
* go to the `App registrations` service
* register a public app, with a _single tenant_ settings
* in the menu _Manage/Authentication_ enable the setting _Allow public client flows_
* in the menu _Manage/API permissions_ add the `Presence.Read`, `offline_access` permissions
* in the menu _Overview_ note your `Application (client) ID` and your `Directory (tenant) ID`

Back to the _Light Teams_ application, in the `appsettings.json` file, update with the IDs you wrote down.  

You can launch the application, and see the message to authorize the application.
