# Squirrel Utilities

[![NuGet](https://img.shields.io/nuget/v/Grynwald.Utilities.Squirrel.svg)](https://www.nuget.org/packages/Grynwald.Utilities.Squirrel)
[![Build Status](https://dev.azure.com/ap0llo/OSS/_apis/build/status/utilities-squirrel?branchName=master)](https://dev.azure.com/ap0llo/OSS/_build/latest?definitionId=9?branchName=master)

*Grynwald.Utilities.Squirrel* aids the use of [Squirrel](https://github.com/Squirrel/Squirrel.Windows)
in applications that want to perform additional actions as part of the
installation process. Squirrel Utilities also provides some helpers useful for
console applications.

## Installation

The Squirrel Utiltiies package is available on [NuGet.org](https://www.nuget.org/packages/Grynwald.Utilities.Squirrel/)

## Building from source

MarkdownGenerator targets .NET Framewoek 4.6.1 and can be built using the .NET SDK (tested with Visual Studio 2017 15.9)

```ps1
  dotnet restore .\src\Utilities.Squirrel.sln

  dotnet build .\src\Utilities.Squirrel.sln
```

## Usage

### Installer

*Note: In order to user the Installer class, your application has to be 
 "Squirrel Aware", see [Squirrel Documentation](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/custom-squirrel-events.md) for details*

The Installer built on top of Squirrel's support for 
"[Custom Squirrel Events](https://github.com/Squirrel/Squirrel.Windows/blob/master/docs/using/custom-squirrel-events.md)"

A installer is composed of one or more "installer steps" that are executed for 
the various events supported by Squirrel. The installer should be called as 
early as possible in your application's main method. In most cases, the 
application will not return from the installer call.

Installer instances are instantiated using the `InstallerBuilder`. 
The installer is executed by calling its `HandleInstallationEvents()` methods:

```csharp
using Grynwald.Utilities.Squirrel.Installation;

static void Main()
{
    var installer = InstallerBuilder.CreateBuilder()
        //TODO: Add installer steps here
        .Build();

    installer.HandleInstallationEvents();
}
```

The `InstallerBuilder` class offers methods to configure built-in steps or
supports adding custom steps:

- ``AddCustomStep``: Adds the specified step to the installer (any type
   that implements ``IInstallerStep``)
- ``AddDirectoryToPath()``: Adds a step that adds the specified directory to
   the user's PATH environment variable to the installer
- ``CreateBatchFile()``: Adds a step to the installer that creates a new
   batch-file at the specified location with the specified command
- ``SaveResourceToFile()``: Adds a step to the installer that saves a embedded
  resource to a file
- ``OnFirstRun()``: Sets the action to be run after the application has been
  launched the first time
- ``OnException()``: Sets the action to be executed when an exception occurs
  during installation

For console applications ``InstallerBuilder.CreateConsoleApplicationBuilder()``
creates a installer builder preconfigured with installer steps
useful for console applications. By default, the installer will:

- Create a batch file in the application root directory that launches the
  application. The file is updated after application updates, so it always
  uses the latest installed version of the application.
- Add the application root directory to the user's PATH environment variable
- On first run, display a console window with a message indicating that the
  application was installed successfully.

### Updater

The updater class updates the application in the background and was written
primarily for console applications. It can be configured by passing in a
instance of `UpdateSettings` and will call the appropriate Squirrel methods
in a background task.
The backgroud task is started by invoking ``Updater.Start()``.
Before the application exits, it should wait for the updater task to complete,
either by explicitly calling ``WaitForCompletion()`` or by disposing the 
updater object.

### Example

A console application using both the ``Installer`` and ``Updater`` could look
like this

```csharp
using Grynwald.Utilities.Squirrel.Installation;

static void Main()
{
    InstallerBuilder.CreateBuilder()
        //TODO: Add installer steps here
        .Build()
        .HandleInstallationEvents();

    //TODO: configure updater (e.g. load from app settings)
    UpdateOptions updateOption = ...

    using (var updater = new Updater(new UpdateOptions()))
    {
        updater.Start();

        //
        // you application logic goes here
        //

        updater.WaitForCompletion();
    }
}
```