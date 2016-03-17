using Mono.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ssdump
{
    class Program
    {
        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <param name="args">Command-line options</param>
        static void Main(string[] args)
        {
            try {
                // Variables for holding command-line option results, with defaults.
                bool help = false;
                DumpProcessor processor = new DumpProcessor();
                processor.WriteToConsole = true;

                // Option set, maps command-line options to variables above.
                OptionSet options = new OptionSet()
            {
                { "create-db", "Include CREATE DATABASE statement", value => processor.IncludeCreateDatabase = value != null },
                { "encrypt", "Use encrypted connection", value => processor.UseEncryption = value != null },
                { "help", "Show help message", value => help = value != null },
                { "h|host=", "Connect to host", value => processor.Host = value },
                { "include-users", "Include CREATE USER statements", value => processor.IncludeUsers = value != null },
                { "d|no-data", "No row information", value => processor.NoData = value != null },
                { "max-allowed-packet=", "Maximum packet length", (int value) => processor.MaxPacket = value },
                { "p|password=", "Password to use when connecting to server", value => processor.Password = value },
                { "timeout=", "Connection timeout (seconds)", (int value) => processor.Timeout = value },
                { "u|user=", "User to use if not current user (SQL Server authentication)", value => processor.Username = value }
            };

                // Parse command-line options.
                List<string> extras;
                try
                {
                    extras = options.Parse(args);
                }
                catch (OptionException exception)
                {
                    Console.Error.WriteLine("Error parsing command-line options:");
                    Console.Error.WriteLine(exception.Message);
                    return;
                }

                // Check command-line parameters and make sure that everything is in order.
                if (!help && extras.Count == 0)
                {
                    Console.Error.WriteLine("Name of the database to dump is required.");
                    help = true;
                }

                if (!help && (processor.Password == null && processor.Username != null) || (processor.Password != null && processor.Username == null))
                {
                    Console.Error.WriteLine("To use SQL Server authentication, both user and password must be provided.");
                    help = true;
                }

                if (processor.Password != null && processor.Username != null)
                {
                    processor.UseWindowsAuthentication = false;
                }

                // Do what was requested.
                if (help)
                {
                    ShowHelp(options);
                }
                else
                {
                    processor.DatabaseName = WhichDatabases(extras)[0];
                    processor.Tables = WhichTables(extras);
                    processor.Execute();
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Exception: " + exception.Message);
                Console.Error.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Figure out which databases to dump.
        /// </summary>
        /// <param name="extras">Unparsed command-line options</param>
        /// <returns>List of databases to dump</returns>
        private static List<string> WhichDatabases(List<string> extras)
        {
            List<string> databases = new List<string>();

            // First extra is database to dump.
            databases.Add(extras[0]);

            return databases;
        }

        /// <summary>
        /// Figure out which tables to dump.
        /// </summary>
        /// <param name="extras">Unparsed command-line options</param>
        /// <returns>List of tables to dump</returns>
        private static List<string> WhichTables(List<string> extras)
        {
            List<string> tables = new List<string>();

            // It will be an empty list (meaning all tables) unless...
            if (extras.Count > 1)
            {
                // Everything except first value is a table to dump.
                for (int index = 1; index < extras.Count; index++)
                {
                    tables.Add(extras[index]);
                }
            }

            return tables;
        }

        /// <summary>
        /// Show help message, which includes command-line option descriptions.
        /// </summary>
        /// <param name="options"></param>
        private static void ShowHelp(OptionSet options)
        {
            Console.WriteLine();
            Console.WriteLine(DumpProcessor.ProgramName + " v" + DumpProcessor.ProgramVersion);
            Console.WriteLine("Dumping structure and contents of Microsoft SQL Server databases and tables.");
            Console.WriteLine("Usage: ssdump [OPTIONS] database [table1 table2 ...]");
            Console.WriteLine();
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}