using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Represents metadata required to construct a MySQL connection string, including server credentials and SSL configuration.
    /// </summary>
    public struct ConnectionStringMetadata
    {
        /// <summary>
        /// Gets or sets the hostname or IP address of the MySQL server.
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Gets or sets the name of the MySQL database to connect to.
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// Gets or sets the username used to authenticate with the MySQL server.
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Gets or sets the password used to authenticate with the MySQL server.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the SSL mode used to secure the MySQL connection.
        /// </summary>
        public SSLModes SSLMode { get; set; }
        /// <summary>
        /// Gets or sets the file path to the Certificate Authority (CA) certificate used for SSL verification.
        /// </summary>
        public string ssl_ca { get; set; }
        /// <summary>
        /// Gets or sets the file path to the client SSL certificate used for authentication with the MySQL server.
        /// </summary>
        public string ssl_cert { get; set; }
        /// <summary>
        /// Gets or sets the file path to the client SSL private key used for authentication with the MySQL server.
        /// </summary>
        public string ssl_key { get; set; }

        /// <summary>
        /// Gets the constructed MySQL connection string based on the provided server, credentials, and SSL configuration.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                StringBuilder CS = new StringBuilder();
                CS.Append("server=" + Server + "; ");
                CS.Append("database=" + Database + "; ");
                CS.Append("user=" + User + "; ");

                if (!string.IsNullOrWhiteSpace(Password))
                    CS.Append("password=" + Password + "; ");

                CS.Append("sslmode=" + SSLMode.ToString().ToLower() + "; ");

                if(SSLMode == SSLModes.VerifyCA || SSLMode == SSLModes.VerifyFull)
                {
                    CS.Append("sslca=" + ssl_ca + "; ");
                    CS.Append("sslcert=" + ssl_cert + "; ");
                    CS.Append("sslkey=" + ssl_key + "; ");
                }

                return CS.ToString();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringMetadata"/> struct with server credentials and optional SSL configuration.
        /// </summary>
        /// <param name="SetServer">The hostname or IP address of the MySQL server.</param>
        /// <param name="SetDatabase">The name of the MySQL database to connect to.</param>
        /// <param name="SetUser">The username used to authenticate with the MySQL server.</param>
        /// <param name="SetPassword">The password used to authenticate with the MySQL server. Optional.</param>
        /// <param name="SetSSLMode">The SSL mode used to secure the MySQL connection.</param>
        /// <param name="Set_ssl_ca">The file path to the Certificate Authority (CA) certificate. Required for VERIFY_CA and VERIFY_FULL modes.</param>
        /// <param name="Set_ssl_cert">The file path to the client SSL certificate. Required for VERIFY_CA and VERIFY_FULL modes.</param>
        /// <param name="Set_ssl_key">The file path to the client SSL private key. Required for VERIFY_CA and VERIFY_FULL modes.</param>
        public ConnectionStringMetadata(
            string SetServer,
            string SetDatabase,
            string SetUser,
            string SetPassword = "",
            SSLModes SetSSLMode = SSLModes.None,
            string Set_ssl_ca = "",
            string Set_ssl_cert = "",
            string Set_ssl_key = "")
        {
            Server = SetServer;
            Database = SetDatabase;
            User = SetUser;
            Password = SetPassword;
            SSLMode = SetSSLMode;
            ssl_ca = Set_ssl_ca;
            ssl_cert = Set_ssl_cert;
            ssl_key = Set_ssl_key;
        }

        /// <summary>
        /// Determines whether the connection metadata is complete and valid based on the selected SSL mode.
        /// </summary>
        public bool IsValid()
        {
            if (SSLMode == SSLModes.VerifyCA || SSLMode == SSLModes.VerifyFull)
                return
                    !string.IsNullOrWhiteSpace(Server) &&
                    !string.IsNullOrWhiteSpace(Database) &&
                    !string.IsNullOrWhiteSpace(User) &&
                    !string.IsNullOrWhiteSpace(ssl_ca) &&
                    !string.IsNullOrWhiteSpace(ssl_cert) &&
                    !string.IsNullOrWhiteSpace(ssl_key);
            else
                return
                    !string.IsNullOrWhiteSpace(Server) &&
                    !string.IsNullOrWhiteSpace(Database) &&
                    !string.IsNullOrWhiteSpace(User);
        }
    }
}
