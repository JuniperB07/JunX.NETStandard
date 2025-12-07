using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.Utility
{
    /// <summary>
    /// Defines a contract for logging operations, including log entry creation and log file management.
    /// </summary>
    /// <remarks>
    /// This interface supports structured logging by allowing implementations to add timestamped log entries,
    /// generate log files, and verify their existence. It is suitable for diagnostic, audit, or operational tracing scenarios
    /// in modular or metadata-driven systems.
    /// </remarks>
    public interface ILoggable
    {
        void AddLog(DateTime LogDT, object Category, string Details);
        void CreateLogFile();
        bool LogFileExists();
    }
}
