using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssdump
{
    /// <summary>
    /// A spot to hold settings used to determine the behavior of a single execution of the program.
    /// </summary>
    class ProgramSettings
    {
        /// <summary>
        /// Hostname or IP address of server to connect to.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Whether or not to include CREATE DATABASE statement.
        /// </summary>
        public bool IncludeCreateDatabase { get; set; }

        /// <summary>
        /// Whether or not to include CREATE USER statements.
        /// </summary>
        public bool IncludeUsers { get; set; }

        /// <summary>
        /// Maximum packet size (bytes).
        /// </summary>
        public int? PacketSize { get; set; }

        /// <summary>
        /// Password to use when connecting to server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Do not include data (schema only).
        /// </summary>
        public bool NoData { get; set; }

        /// <summary>
        /// Connection timeout (in seconds).
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// User to use when connecting to the server.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Whether or not to encrypt network communications.
        /// </summary>
        public bool UseEncryption { get; set; }

        /// <summary>
        /// Whether or not to use Windows authentication.
        /// </summary>
        public bool UseWindowsAuthentication { get; set; }

        /// <summary>
        /// Constructor: Set default values.
        /// </summary>
        public ProgramSettings()
        {
            // Set default values.
            Host = "localhost";
            IncludeCreateDatabase = false;
            IncludeUsers = false;
            NoData = false;
            PacketSize = null;
            Password = null;
            Timeout = 10;
            User = null;
            UseEncryption = false;
            UseWindowsAuthentication = true;
        }
    }
}
