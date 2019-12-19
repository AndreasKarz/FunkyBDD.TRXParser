# TRXParser
A little standalone Console App to merge different TRX files and generate a Teams message over WebHooks.

> In automated E2E tests with **different environments** you often have the problem of generating **multiple TRX files**. These are deleted after transmission to Azure DevOps.
> If you want to send a notification to teams, you need **a tool that consolidates and analyzes** these TRX files and sends them to **teams** with a **WebHook**. This is exactly what the **TRXParser** does.



## Start parameters

The app expects the following parameters. These can be given as command line parameters or as environment variables. The command line parameter wins against the environment variable!

| cmd          | env                  | description                                                  |
| ------------ | -------------------- | ------------------------------------------------------------ |
| --title      | TRXPARSER_TITLE      | The title of the test run                                    |
| --repourl    | TRXPARSER_REPOURL    | Link to the file repository                                  |
| --resulturl  | TRXPARSER_RESULTURL  | Link to the page with the test results                       |
| --searchpath | TRXPARSER_SEARCHPATH | File path to search the TRX files                            |
| --oklimit    | TRXPARSER_OKLIMIT    | Minimum percentage as Int for a test to be marked as OK      |
| --webhook    | TRXPARSER_WEBHOOK    | WebHook URL from the Teams channel                           |
| --deltrx     | TRXPARSER_DELTRX     | Set (cmd) or true (env) if the TRX files should be deleted when the result is send to Teams |
| --debug      | TRXPARSER_DEBUG      | Set (cmd) or true (env) will display debug information into the console |

## Design of the application

### Team Card template

The basic structure of the teams Card is stored in the folder `__DEV__`. This folder must be included as a resource, so that the EXE can be started completely standalone.
Therefore the icons are also defined as Base64. But the templates are also located in the folder `__DEV__`.

### Implementation of the start parameters

To simple handle the start parameters for a console app you will need the Dragon Fruit package. You can install this prerelease feature with the Nuget console.

```nuget
Install-Package System.CommandLine.DragonFruit -Version 0.3.0-alpha.19577.1
```

ATTENTION: Dragon Fruit will not work with namespaces they have `.` inside! You will find more information about Dragon Fruit at [Scott Hanselmans Blog](https://www.hanselman.com/blog/DragonFruitAndSystemCommandLineIsANewWayToThinkAboutNETConsoleApps.aspx) or on [GitHub](https://github.com/dotnet/command-line-api/wiki/DragonFruit-overview). For the latest release please visit the [Nuget Page](https://www.nuget.org/packages/System.CommandLine.DragonFruit).

### Build a single file app

To build a single file app, publish the solution with the command below. 

```powershell
dotnet publish -r win-x64 -c Release -o "c:\temp" /p:PublishSingleFile=true
```

Since all required resources are stored in the project, no images or templates are needed. The app works self-contained. You will find more information in the [DotNet Core tutorials](https://dotnetcoretutorials.com/2019/06/20/publishing-a-single-exe-file-in-net-core-3-0/). 

For more arguments read this [documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore21#arguments).

