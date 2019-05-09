# Task 1

The commandline expects following arguments:

- `-sid`:  the subscription's ID 

- `-rsGroup` the resource group's name



# Task 2

### Setup 

The `libman.json` contains all required web dependencies of this project. These can be installed via the libman tool.

The `appsettings.json` must be configured before running the application. `ApiKey`, `ApiBaseUrl` and `ApiViewId` refer to the task's first part.



### Part 1:

The web app for browsing the Warsaw API results can be accessed via the web app server's base URL. First you have to push the button in order to buffer the data and then you can browse it in the populated table.



### Part 2:

The web form for user input of template variables can be accessed via `<server>/Deployment/`.

The JSON templates for ARM Deployment are located at `<root>/ArmConfig/`. They are based on an auto-generated template created by the Azure web UI. The respective entries are modified at run-time based on entries in `appconfig.json ` and user input from the web form.