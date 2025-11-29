using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

namespace JunX.NETStandard.MySQL
{
    public partial class DBConnect
    {
        /// <summary>
        /// Provides static methods for generating, retrieving, and validating a custom MySQL connection string configuration file.
        /// </summary>
        /// <remarks>
        /// This class creates and manages a standalone <c>DBConnect.config</c> file located in the application's base directory.
        /// The configuration file contains a <c>configuration</c> root element with a nested <c>ConnectionString</c> section,
        /// which includes an <c>Add</c> element storing the connection string, provider name, and identifier.
        /// All operations are performed statically and assume a single named connection entry.
        /// </remarks>
        public static class ConfigGenerator
        {
            private const string NAME = "DBConnect";
            private const string PROVIDER = "JunX.NETStandard.MySQL";
            private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBConnect.config");

            /// <summary>
            /// Generates a new configuration file containing a MySQL connection string entry.
            /// </summary>
            /// <param name="ConnectionString">
            /// The connection string to be embedded in the configuration file. Must be non-empty and properly formatted.
            /// </param>
            /// <exception cref="Exception">
            /// Thrown when the provided connection string is null, empty, or consists only of whitespace.
            /// </exception>
            /// <remarks>
            /// This method creates a <c>DBConnect.config</c> file in the application's base directory.
            /// The file structure includes a root <c>configuration</c> element with a nested <c>ConnectionString</c> section.
            /// Within that section, an <c>Add</c> element is written with attributes for <c>name</c>, <c>connectionString</c>, and <c>providerName</c>.
            /// The file is overwritten if it already exists.
            /// </remarks>
            public static void GenerateDBConfig(string ConnectionString)
            {
                if (string.IsNullOrWhiteSpace(ConnectionString))
                    throw new Exception("Connection string cannot be empty.");

                XmlWriterSettings XWS = new XmlWriterSettings { Indent = true };

                using (XmlWriter writer = XmlWriter.Create(ConfigPath, XWS))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("configuration");
                    writer.WriteStartElement("ConnectionString");

                    writer.WriteStartElement("Add");
                    writer.WriteAttributeString("name", NAME);
                    writer.WriteAttributeString("connectionString", ConnectionString);
                    writer.WriteAttributeString("providerName", PROVIDER);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                
            }

            /// <summary>
            /// Retrieves the MySQL connection string from the custom <c>DBConnect.config</c> file.
            /// </summary>
            /// <returns>
            /// The connection string value associated with the predefined name, or <c>null</c> if not found.
            /// </returns>
            /// <exception cref="Exception">
            /// Thrown when the configuration file <c>DBConnect.config</c> is missing from the application's base directory.
            /// </exception>
            /// <remarks>
            /// This method loads the XML configuration file and searches for an <c>Add</c> element under the <c>ConnectionString</c> section.
            /// It returns the value of the <c>connectionString</c> attribute if the <c>name</c> attribute matches the predefined constant.
            /// The XML structure is expected to be case-sensitive and well-formed.
            /// </remarks>
            public static string GetConnectionString()
            {
                if (!File.Exists(ConfigPath))
                    throw new Exception("Missing File: DBConnect.config");

                XmlDocument XD = new XmlDocument();
                XD.Load(ConfigPath);

                var addNodes = XD.SelectNodes("/configuration/ConnectionString/Add");

                if (addNodes == null)
                    return null;

                foreach (XmlNode node in addNodes)
                {
                    var nameAttrib = node.Attributes?["name"];
                    var connStrAttrib = node.Attributes?["connectionString"];

                    if (nameAttrib?.Value == NAME)
                        return connStrAttrib?.Value;
                }
                return null;
            }

            /// <summary>
            /// Checks whether the <c>DBConnect.config</c> file exists in the application's base directory.
            /// </summary>
            /// <returns>
            /// <c>true</c> if the configuration file is present; otherwise, <c>false</c>.
            /// </returns>
            /// <remarks>
            /// This method performs a simple file existence check using <see cref="System.IO.File.Exists"/>.
            /// It does not validate the file's contents or structure.
            /// </remarks>
            public static bool ConfigExists()
            {
                return File.Exists(ConfigPath);
            }
        }
    }
}
