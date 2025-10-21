using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Provides a centralized container for shared MySQL-related objects and configuration values used across the application.
    /// </summary>
    /// <remarks>
    /// This static class holds reusable instances of connection, command, adapter, reader, dataset, and related metadata.
    /// Designed for internal use in tightly scoped database workflows where controlled initialization and reuse are required.
    /// </remarks>
    internal static class InternalVariables
    {
        internal static MySqlConnection Connection;
        internal static MySqlDataReader Reader;
        internal static MySqlDataAdapter Adapter;
        internal static MySqlCommand Command;
        internal static DataSet Dataset;
        internal static string ConnectionString;
        internal static string CommandText;
        internal static List<string> Values;
        internal static ConnectionStringMetadata ConnectionStringInformation;

        /// <summary>
        /// Initializes all internal MySQL-related objects and resets associated metadata to their default states.
        /// </summary>
        /// <remarks>
        /// This method creates new instances of <see cref="MySqlConnection"/>, <see cref="MySqlDataAdapter"/>, <see cref="MySqlCommand"/>, and <see cref="DataSet"/>.
        /// It also clears the connection string, command text, and value list by assigning them to <c>null</c>.
        /// Intended for use in controlled startup or reset scenarios within internal database workflows.
        /// </remarks>
        internal static void InitializeAll()
        {
            Connection = new MySqlConnection();
            Adapter = new MySqlDataAdapter();
            Command = new MySqlCommand();
            Dataset = new DataSet();
            ConnectionString = null;
            CommandText = null;
            Values = new List<string>();
            ConnectionStringInformation = new ConnectionStringMetadata("", "", "");
        }
    }
}
