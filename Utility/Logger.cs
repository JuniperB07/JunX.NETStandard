using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JunX.NETStandard.Utility
{
    /// <summary>
    /// Provides basic logging functionality for structured CSV-style log files, including file creation and entry appending.
    /// </summary>
    /// <remarks>
    /// This class writes logs in the format <c>Date/Time,Category,Details</c> to a specified file path.
    /// It supports checking for file existence, generating a header row, and appending timestamped log entries.
    /// Intended for use in .NET applications requiring lightweight, file-based logging.
    /// </remarks>
    public class Logger : ILoggable
    {
        private string _logPath = "";

        /// <summary>
        /// Occurs when a new log entry has been added.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="LogAddedEventArgs"/> to provide subscribers 
        /// with details about the log entry, including the message, timestamp, 
        /// category, and additional information. It can be used to trigger 
        /// monitoring, auditing, or custom handling whenever a new log is recorded.
        /// </remarks>
        public event EventHandler<LogAddedEventArgs> LogAdded;
        /// <summary>
        /// Occurs when a new log file has been created.
        /// </summary>
        /// <remarks>
        /// This event uses <see cref="LogFileCreatedEventArgs"/> to provide subscribers 
        /// with the file path of the newly created log file. It can be used to trigger 
        /// monitoring, archiving, or custom handling whenever a new log file is generated.
        /// </remarks>
        public event EventHandler<LogFileCreatedEventArgs> LogFileCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with the specified log file path.
        /// </summary>
        /// <param name="LogPath">The full file path where log entries will be written.</param>
        /// <remarks>
        /// This constructor sets the internal log path used by all logging operations. The path should be valid and writable.
        /// </remarks>
        public Logger(string LogPath)
        {
            _logPath = LogPath;
        }

        /// <summary>
        /// Determines whether the log file exists at the configured path.
        /// </summary>
        /// <returns><c>true</c> if the log file exists; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method checks the presence of the log file using the current internal path. It does not validate file contents.
        /// </remarks>
        public bool LogFileExists()
        {
            return File.Exists(_logPath);
        }
        /// <summary>
        /// Creates a new log file at the configured path with a CSV-style header row.
        /// </summary>
        /// <remarks>
        /// This method writes the header <c>Date/Time,Category,Details</c> to the log file. If the file already exists,
        /// the header will be appended, potentially duplicating it. Use <see cref="LogFileExists"/> to check existence before calling.
        /// </remarks>
        public void CreateLogFile()
        {
            File.AppendAllText(_logPath, "Date/Time,Category,Details" + Environment.NewLine, Encoding.UTF8);
            LogFileCreated?.Invoke(this, new LogFileCreatedEventArgs(_logPath));
        }
        /// <summary>
        /// Appends a new log entry to the log file using the specified timestamp, category, and detail message.
        /// </summary>
        /// <param name="LogDateTime">The date and time of the log entry.</param>
        /// <param name="Category">The category or label describing the log context (e.g., "Info", "Error").</param>
        /// <param name="Details">The detailed message or description of the log event.</param>
        /// <remarks>
        /// The log entry is written in CSV format as <c>MM/dd/yyyy - HH:mm:ss,Category,Details</c>, followed by a newline.
        /// This method assumes the log file path has been initialized and is writable.
        /// </remarks>
        public void AddLog(DateTime LogDateTime, string Category, string Details)
        {
            StringBuilder log = new StringBuilder();

            log.Append(LogDateTime.ToString("MM/dd/yyyy - HH:mm:ss") + ",");
            log.Append(Category + ",");
            log.Append(Details);

            try
            {
                File.AppendAllText(_logPath, log.ToString() + Environment.NewLine, Encoding.UTF8);
                LogAdded?.Invoke(this, new LogAddedEventArgs(log.ToString(), LogDateTime, Category, Details));
            }
            catch
            {
                return;
            }
        }
    }

    /// <summary>
    /// Provides data for events raised when a new log entry has been added.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates information about the log entry, including 
    /// the log message, timestamp, category, and additional details. It allows event 
    /// subscribers to capture, process, or display log information as part of monitoring 
    /// or diagnostic workflows.
    /// </remarks>
    public class LogAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the main log message that was added.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing the text of the log entry.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="LogAddedEventArgs"/>. It allows event subscribers to access 
        /// the primary log message for display, storage, or diagnostic purposes.
        /// </remarks>
        public string Log { get; }
        /// <summary>
        /// Gets the date and time when the log entry was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value representing the timestamp of the log entry.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="LogAddedEventArgs"/>. It allows event subscribers to track 
        /// when the log was recorded, which is useful for ordering, filtering, 
        /// or auditing log entries.
        /// </remarks>
        public DateTime LogDT { get; }
        /// <summary>
        /// Gets the category assigned to the log entry.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the classification of the log, 
        /// such as informational, warning, or error.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="LogAddedEventArgs"/>. It allows event subscribers to 
        /// filter, group, or analyze log entries based on their category.
        /// </remarks>
        public string Category { get; }
        /// <summary>
        /// Gets additional descriptive information associated with the log entry.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> containing extended details that supplement the main log message.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="LogAddedEventArgs"/>. It allows event subscribers to capture 
        /// supporting context or diagnostic information beyond the primary log message 
        /// and category.
        /// </remarks>
        public string Details { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogAddedEventArgs"/> class 
        /// with the specified log information.
        /// </summary>
        /// <param name="log">
        /// The main log message text.
        /// </param>
        /// <param name="logDT">
        /// The date and time when the log entry was created.
        /// </param>
        /// <param name="category">
        /// The category assigned to the log entry, such as informational, warning, or error.
        /// </param>
        /// <param name="details">
        /// Additional descriptive information that supplements the main log message.
        /// </param>
        public LogAddedEventArgs(string log, DateTime logDT, string category, string details)
        {
            Log = log;
            LogDT = logDT;
            Category = category;
            Details = details;
        }
    }

    /// <summary>
    /// Provides data for events raised when a new log file has been created.
    /// </summary>
    /// <remarks>
    /// This event argument class encapsulates the file path of the newly created log file. 
    /// It allows event subscribers to access the location of the log file for purposes such 
    /// as monitoring, archiving, or displaying log information.
    /// </remarks>
    public class LogFileCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the file path of the newly created log file.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the full path where the log file was generated.
        /// </value>
        /// <remarks>
        /// This read‑only property is initialized through the constructor of 
        /// <see cref="LogFileCreatedEventArgs"/>. It allows event subscribers to 
        /// access the location of the log file for monitoring, archiving, or 
        /// displaying purposes.
        /// </remarks>
        public string LogPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFileCreatedEventArgs"/> class 
        /// with the specified log file path.
        /// </summary>
        /// <param name="SetLogPath">
        /// A <see cref="string"/> representing the full path of the newly created log file.
        /// </param>
        public LogFileCreatedEventArgs(string SetLogPath)
        {
            LogPath = SetLogPath;
        }
    }
}
