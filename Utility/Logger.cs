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
            }
            catch
            {
                return;
            }
        }
    }
}
