using System.Reflection;
using System.Runtime.CompilerServices;

namespace HLogger {

	/// <summary>
	/// The `hLogger` class is a static utility class providing a logging tool for C# applications. 
	/// It offers various log levels, console output, file logging, and configuration options.
	/// </summary>
	/// <remarks>
	/// This class simplifies logging in your application by providing easy-to-use methods for logging messages 
	/// at different levels such as Info, Log, Debug, Warning, Error, Exception, and Critical.
	/// Users can customize logging behavior, including source location details in log files and setting log levels.
	/// </remarks>
	public static class hLogger {
		/// <summary>
		/// Represents the severity levels for logging messages.
		/// </summary>
		public enum LogLevel {
			/// <summary>
			/// Include all log levels.
			/// </summary>
			All,

			/// <summary>
			/// Informational messages.
			/// </summary>
			Info,

			/// <summary>
			/// General log messages.
			/// </summary>
			Log,

			/// <summary>
			/// Messages useful for debugging.
			/// </summary>
			Debug,

			/// <summary>
			/// Warning messages.
			/// </summary>
			Warning,

			/// <summary>
			/// Error messages.
			/// </summary>
			Error,

			/// <summary>
			/// Exception messages.
			/// </summary>
			Exception,

			/// <summary>
			/// Critical error messages.
			/// </summary>
			Critical,

			/// <summary>
			/// Exclude all log levels.
			/// </summary>
			None
		}


		/// <summary>
		/// Represents options for including or excluding source location information in the log file.
		/// </summary>
		public enum IncludeSourceLocation {
			/// <summary>
			/// Do not include source location information in the log file.
			/// </summary>
			No,

			/// <summary>
			/// Include source location information in the log file.
			/// </summary>
			Yes
		}

		/// <summary>
		/// Enumerates options for the location where log messages should be output.
		/// </summary>
		public enum LogOutput {
			/// <summary>
			/// Do not output log messages to any location.
			/// </summary>
			None,

			/// <summary>
			/// Output log messages to the terminal/console only.
			/// </summary>
			TerminalOnly,

			/// <summary>
			/// Output log messages to a file only.
			/// </summary>
			FileOnly,

			/// <summary>
			/// Output log messages to both the terminal/console and a file.
			/// </summary>
			Both
		}


		private static LogLevel _logLevel = LogLevel.All;
		private static IncludeSourceLocation _includeSourceLocation = IncludeSourceLocation.Yes;
		private static LogOutput _logOutput = LogOutput.Both;

		/// <summary>
		/// Sets the global logging level, specifying the severity threshold for displayed log messages.
		/// </summary>
		/// <param name="level">The log level to set.</param>
		/// /// <remarks>
		/// Default Value: LogLevel.All
		/// </remarks>
		public static void SetLogLevel(LogLevel level) {
			_logLevel = level;
		}

		/// <summary>
		/// Sets the configuration for including or excluding source location information in the log file.
		/// </summary>
		/// <param name="includeSourceLocation">The option to include or exclude source location information.</param>
		/// <remarks>
		/// Default Value: IncludeSourceLocation.Yes
		/// </remarks>
		public static void SetIncludeSourceLocation(IncludeSourceLocation includeSourceLocation) {
			_includeSourceLocation = includeSourceLocation;
		}

		/// <summary>
		/// Sets the destination for log messages..
		/// </summary>
		/// <param name="logOutput">The target location(s) for log messages.</param>
		/// /// <remarks>
		/// Default Value: LogOutput.Both
		/// </remarks>
		public static void SetLogOutput(LogOutput logOutput) {
			_logOutput = logOutput;
		}


		/// <summary>
		/// Logs an informational message using the INFO log level.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'message' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>
		public static void Info(string message) {
			/*,
				[CallerMemberName] string memberName = "",
				[CallerFilePath] string sourceFilePath = "",
				[CallerLineNumber] int sourceLineNumber = 0
			*/
			if (_logOutput == LogOutput.None) return;
			if (_logLevel > LogLevel.Info) return;
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {
				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("INFO".PadRight(10));
				Console.ResetColor();
				Console.Write($"{message}\n");
			}

			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				message = $"{message,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
				//message = $"{message,-50} | at {Assembly.GetCallingAssembly().GetName().Name}{sourceFilePath.Split(Assembly.GetCallingAssembly().GetName().Name).Last().Split('.').First().Replace(Path.DirectorySeparatorChar, '.')}.{memberName}() in {sourceFilePath}:{sourceLineNumber}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Info",-10} | {message}");
		}

		/// <summary>
		/// Logs a general log message using the LOG log level.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'message' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>
		public static void Log(string message) {
			if (_logOutput == LogOutput.None) return;
			if (_logLevel > LogLevel.Log) return;
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {
				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.Write("LOG".PadRight(10));
				Console.ResetColor();
				Console.Write($"{message}\n");
			}
			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				message = $"{message,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Log",-10} | {message}");
		}


		/// <summary>
		/// Dumps and logs an object using the DEBUG log level.
		/// </summary>
		/// <param name="obj">The object to be dumped and logged.</param>
		/// <param name="name">This parameter is automatically captured and should not be populated</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'obj' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>
		public static void DebugObject(object obj,
			[CallerArgumentExpression(nameof(obj))] string? name = null) {
			
			if (_logOutput == LogOutput.None) return;
			if (_logLevel > LogLevel.Debug) return;
				var txt = name == null ? obj.GetType().Name : $"{name} ({obj.GetType().Name})";
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {
				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.Write("DEBUG".PadRight(10));
				Console.ResetColor();
				Console.Write($"{txt}\n");
				Console.Write(
					$"{System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }).Replace("\n", "\n\t\t\t").Insert(0, "\t\t\t")}\n");
			}
			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				txt = $"{txt,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Debug",-10} | {txt}");
			LogToFile($"{System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }).Replace("\n", "\n\t\t\t\t\t\t").Insert(0, "\t\t\t\t\t\t")}");
		}

		/// <summary>
		/// Logs a warning message using the WARNING log level.
		/// </summary>
		/// <param name="message">The warning message to be logged.</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'message' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>
		public static void Warning(string message) {
			if (_logOutput == LogOutput.None) return;
			if (_logLevel > LogLevel.Warning) return;
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {

				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.Write("WARNING".PadRight(10));
				Console.ResetColor();
				Console.Write($"{message}\n");
			}
			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				message = $"{message,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Warning",-10} | {message}");
		}

		/// <summary>
		/// Logs an error message using the ERROR log level.
		/// </summary>
		/// <param name="message">The error message to be logged.</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'message' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>
		public static void Error(string message) {
			if (_logOutput == LogOutput.None) return;			
			if (_logLevel > LogLevel.Error) return;
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {

				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write("ERROR".PadRight(10));
				Console.ResetColor();
				Console.Write($"{message}\n");
			}
			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				message = $"{message,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Error",-10} | {message}");
		}

		/// <summary>
		/// Logs an exception using the EXCEPTION log level.
		/// </summary>
		/// <param name="e">The exception to be logged.</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'e' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>
		public static void Exception(Exception e) {
			if (_logOutput == LogOutput.None) return;
			if (_logLevel > LogLevel.Exception) return;
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {

				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write("EXCEPTION".PadRight(10));
				Console.ResetColor();
				Console.Write($"{e.Message}\n");
				if (!String.IsNullOrEmpty(e.StackTrace))
					Console.Write(e.StackTrace.Replace(" at ", "\t  at ") + "\n");
			}
			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			var message = e.Message;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				message = $"{message,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Exception",-10} | {message}");
			if (!String.IsNullOrEmpty(e.StackTrace))
				LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Exception",-10} | {e.StackTrace.Replace(" at ", "\t  at ")}");
		}

		/// <summary>
		/// Logs a critical message using the CRITICAL log level.
		/// </summary>
		/// <param name="message">The critical message to be logged.</param>
		/// <remarks>
		/// Users are encouraged to only populate the 'message' parameter, allowing other details to be automatically captured.
		/// The calling member's name, source file path, and source line number are automatically captured.
		/// </remarks>

		public static void Critical(string message) {
			if (_logOutput == LogOutput.None) return;
			if (_logLevel > LogLevel.Critical) return;
			if (_logOutput is LogOutput.Both or LogOutput.TerminalOnly) {
				Console.Write("[hLogger] ");
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.BackgroundColor = ConsoleColor.DarkRed;
				Console.Write("\u001b[1m");
				Console.Write("CRITICAL");
				Console.ResetColor();
				Console.Write($"  {message}\n");
			}
			if (_logOutput is not (LogOutput.Both or LogOutput.FileOnly)) return;
			if (_includeSourceLocation == IncludeSourceLocation.Yes)
				message = $"{message,-50} | {Environment.StackTrace.Split('\n')[2].TrimEnd()}";
			LogToFile($"[hLogger] {DateTime.Now:yyyy-MM-dd HH:mm:ss} | {"Critical",-10} | {message}");
		}

		/// <summary>
		/// Logs assembly information using the INFO log level.
		/// </summary>
		/// <remarks>
		/// This includes the full name of the calling assembly, providing details about its name, version, and other attributes.
		/// </remarks>

		public static void AssemblyInfo() {
			if (_logOutput == LogOutput.None) return;			
			if (_logLevel > LogLevel.Info) return;
			if (_logOutput is not (LogOutput.Both or LogOutput.TerminalOnly)) return;
			Console.Write("[hLogger] ");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("ASSEMBLY".PadRight(10));
			Console.ResetColor();
			Console.Write($"{Assembly.GetCallingAssembly().FullName}\n");
		}

		/// <summary>
		/// Appends a log message to a specified log file.
		/// </summary>
		/// <param name="message">The log message to be written to the file.</param>
		/// <param name="filename">The name of the log file (default is "hLogger.log").</param>
		/// <remarks>
		/// The method creates a directory for log files in the user's application data folder and appends the log message to the specified file.
		/// If the file does not exist, it will be created.
		/// </remarks>
		private static void LogToFile(string message, string filename = "hLogger.log") {
			if (Assembly.GetEntryAssembly() == null) return;
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + Assembly.GetEntryAssembly()!.GetName().Name + Path.DirectorySeparatorChar + "logs";
			Directory.CreateDirectory(path);
			path = Path.Combine(path, filename);
			using StreamWriter sw = File.AppendText(path);
			sw.WriteLine(message);
		}

	}
}
