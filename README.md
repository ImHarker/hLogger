# hLogger

`hLogger` is a logging library for C# that provides easy-to-use logging functionalities for your applications. It supports various log levels, console output, and file logging.


## Features

- Log levels: Info, Log, Debug, Warning, Error, Exception, Critical
- Console output for log messages
- Logging with customizable log levels
- Configuration for including source location in the log file
- Configuration for the log output (None, Terminal only, File only, Both)
- Lightweight and easy to integrate

## Installation

Download the `hLogger.dll` and `hLogger.xml` files from the [Releases](https://github.com/ImHarker/hLogger/releases) page.

OR

Download the source code and build it yourself.

## Usage

1. **Include hLogger in Your Project:**
    - Copy the `hLogger.dll` and `hLogger.xml` file into your project directory.
    - Reference the DLL in your project.

2. **Initialization:**
    - In your code, include the HLogger namespace at the start of your application.

    ```csharp
    using HLogger;
    ```

3. **Logging Messages:**
    - Log messages at different levels.

    ```csharp
    //Log Assembly Information
    hLogger.AssemblyInfo();
    
    //Log an informational message
    hLogger.Info("Starting hChat Client...");
    
    //Log an "Log" message
    hLogger.Log("Log Test");
    
    //Log a dump of an object
    var test = new Message("Hello, World!"); //Example object
    hLogger.DebugObject(test);
    
    //Log a warning message
    hLogger.Warning("Warning Test");
    
    //Log an error message
    hLogger.Error("Error Test");

    //Log an exception
    //code that throws an exception
    try {
    	var y = 1;
    	var x = 1 / (y - 1);
    }
    catch (Exception e) {
    	hLogger.Exception(e);
    }

    //Log a critical error message
    hLogger.Critical("Critical Test");
    ```

5. **Configuration:**
    - Customize logging configuration.
  
    ```csharp
    //Set the log output
    hLogger.SetLogOutput(hLogger.LogOutput.Both);
    
    // Set the log level
    hLogger.SetLogLevel(hLogger.LogLevel.All);

    // Include source location in log files
    hLogger.SetIncludeSourceLocation(hLogger.IncludeSourceLocation.Yes);
    ```


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.txt) file for details.
