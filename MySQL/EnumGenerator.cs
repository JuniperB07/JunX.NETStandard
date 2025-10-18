using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Provides functionality to generate C# <c>enum</c> files from MySQL table and column metadata.
    /// </summary>
    /// <remarks>
    /// This class connects to a MySQL database using the provided connection string,
    /// retrieves table and column names from the specified <see cref="DatabaseName"/>,
    /// and generates corresponding <c>.cs</c> files containing <c>enum</c> definitions for each table.
    /// Each enum includes the table name and its columns as members, with whitespace and hyphens replaced by underscores.
    /// Intended for use in code generation scenarios where database schema needs to be reflected in strongly typed constructs.
    /// </remarks>
    public class EnumGenerator
    {
        private MySqlConnection conn = new MySqlConnection();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumGenerator"/> class and attempts to open a connection to the specified MySQL server.
        /// </summary>
        /// <param name="ServerConnectionString">
        /// The connection string used to establish a connection to the MySQL server. This should include server, user ID, and password—but <b>must not</b> include the database name.
        /// </param>
        /// <param name="ConnectionState">
        /// An output parameter that reflects the final state of the connection after the attempt to open it.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the connection attempt fails due to invalid credentials, unreachable server, or other connection-related issues.
        /// </exception>
        /// <remarks>
        /// This constructor creates a new <see cref="MySqlConnection"/> using the provided connection string and attempts to open it.
        /// If successful, <paramref name="ConnectionState"/> is set to <c>Open</c>; otherwise, it is set to <c>Closed</c> and an exception is thrown.
        /// <b>Note:</b> The <paramref name="ServerConnectionString"/> should not include the database name, as it is set separately via the <see cref="DatabaseName"/> property.
        /// </remarks>
        public EnumGenerator(string ServerConnectionString, out ConnectionState ConnectionState)
        {
            conn = new MySqlConnection(ServerConnectionString);
            try
            {
                conn.Open();
                ConnectionState = conn.State;
            }
            catch(Exception e)
            {
                ConnectionState = ConnectionState.Closed;
                throw new Exception("Unable to connect to server.\n\n" + e.Message.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the name of the MySQL database to be used for table and column enumeration.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the target database name for metadata extraction.
        /// </value>
        /// <remarks>
        /// This property must be set before invoking any schema-related methods such as <c>GenerateEnumFiles</c>.
        /// The database name should not be included in the <c>ServerConnectionString</c> passed to the constructor.
        /// </remarks>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Generates C# <c>enum</c> files for each table in the specified MySQL database and saves them to the target folder.
        /// </summary>
        /// <param name="Folder">
        /// The directory path where the generated <c>.cs</c> enum files will be saved. The folder will be created if it does not exist.
        /// </param>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="bullet">
        /// <item><description>Retrieves all table names from the database specified in <see cref="DatabaseName"/>.</description></item>
        /// <item><description>For each table, retrieves its column names and generates a corresponding <c>enum</c> definition.</description></item>
        /// <item><description>Creates the output folder if necessary and writes each enum to a separate <c>.cs</c> file named after the table.</description></item>
        /// </list>
        /// Each enum includes the table name and its columns as members, with spaces and hyphens replaced by underscores.
        /// </remarks>
        public void GenerateEnumFiles(string Folder)
        {
            string Enums = "";
            string OutputPath = "";

            foreach(string tables in GetTables())
            {
                Enums = GenerateEnumFromList(GetColumns(tables), tables);

                Directory.CreateDirectory(Folder);
                OutputPath = Path.Combine(Folder, tables + ".cs");
                File.WriteAllText(OutputPath, Enums);
            }
        }

        private List<string> GetTables()
        {
            List<string> tables = new List<string>();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;

            string command = "SHOW TABLES FROM " + DatabaseName + ";";
            cmd = new MySqlCommand(command, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
                tables.Add(reader[0].ToString());
            reader.Close();
            cmd.Dispose();

            return tables;
        }
        private  List<string> GetColumns(string Table)
        {
            List<string> columns = new List<string>();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;

            string command = "SHOW COLUMNS FROM " + DatabaseName + "." + Table + ";";
            cmd = new MySqlCommand(command, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
                columns.Add(reader[0].ToString());
            reader.Close();
            cmd.Dispose();

            return columns;
        }
        private  string GenerateEnumFromList(List<string> Items, string EnumName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("public enum " + EnumName);
            sb.AppendLine("{");

            sb.AppendLine("    " + EnumName + ",");
            foreach (var item in Items)
            {
                string safeName = item.Replace(" ", "_").Replace("-", "_");
                sb.AppendLine("    " + safeName + ",");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
