using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Specifies the SSL mode used when connecting to a MySQL server.
    /// </summary>
    public enum SSLModes
    {
        /// <summary>
        /// SSL is disabled; all connections are unencrypted.
        /// </summary>
        None,

        /// <summary>
        /// SSL is used if available; falls back to unencrypted if not.
        /// </summary>
        Preferred,

        /// <summary>
        /// SSL is required; connection fails if SSL is unavailable.
        /// </summary>
        Required,

        /// <summary>
        /// SSL is required with server certificate validation against a trusted certificate authority.
        /// </summary>
        VerifyCA,

        /// <summary>
        /// SSL is required with full verification including server hostname identity.
        /// </summary>
        VerifyFull
    }
}
