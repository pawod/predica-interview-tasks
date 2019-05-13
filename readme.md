# Task 1

The commandline expects following arguments:

- `-sid`:  the subscription's ID 

- `-rsGroup` the resource group's name



# Task 2

### WebApp:

The web app for browsing the Warsaw API results can be accessed via the web app server's base URL. First you have to push the button in order to buffer some data and then you can browse it in the populated table.

- **Install required web dependencies** - The `libman.json` contains all required dependencies for the web app. These can be installed via the libman tool (manually or via Visual Studio).
- **appsettings.json** - If you run the app locally, you must set the Warsaw API config parameters.
- **App URL** - The app's default route brings you to the main and only controller, which provides the Warsaw data browser.



### Deployment Project:

The project is designed to be executed via Visual Studio, which will prompt you to enter the missing deployment parameters. It is configured to deploy the solution's `WarsawBrowser` project as web application to azure using the provided deployment parameters.
