using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ssdump
{
    class Program
    {
        /// <summary>
        /// Program name.
        /// </summary>
        private static readonly string ProgramName = "ssdump";

        /// <summary>
        /// Program version.
        /// </summary>
        private static readonly string ProgramVersion = "0.8";

        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <param name="args">Command-line options</param>
        static void Main(string[] args)
        {
            // Variables for holding command-line option results, with defaults.
            bool help = false;
            ProgramSettings settings = new ProgramSettings();

            // Option set, maps command-line options to variables above.
            OptionSet options = new OptionSet()
            {
                { "create-db", "Include CREATE DATABASE statement", value => settings.IncludeCreateDatabase = value != null },
                { "encrypt", "Use encrypted connection", value => settings.UseEncryption = value != null },
                { "help", "Show help message", value => help = value != null },
                { "h|host=", "Connect to host", value => settings.Host = value },
                { "include-users", "Include CREATE USER statements", value => settings.IncludeUsers = value != null },
                { "d|no-data", "No row information", value => settings.NoData = value != null },
                { "max-allowed-packet=", "Maximum packet length", (int value) => settings.PacketSize = value },
                { "p|password=", "Password to use when connecting to server", value => settings.Password = value },
                { "timeout=", "Connection timeout (seconds)", (int value) => settings.Timeout = value },
                { "u|user=", "User to use if not current user (SQL Server authentication)", value => settings.User = value }
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

            if (!help && (settings.Password == null && settings.User != null) || (settings.Password != null && settings.User == null))
            {
                Console.Error.WriteLine("To use SQL Server authentication, both user and password must be provided.");
                help = true;
            }

            if (settings.Password != null && settings.User != null)
            {
                settings.UseWindowsAuthentication = false;
            }

            // Do what was requested.
            if (help)
            {
                ShowHelp(options);
            }
            else
            {
                Execute(settings, extras);
            }
        }

        /// <summary>
        /// Main process.
        /// </summary>
        /// <param name="settings">Settings object used to determine process behavior.</param>
        /// <param name="extras">Extra command-line options that were not parsed out.</param>
        private static void Execute(ProgramSettings settings, List<string> extras)
        {
            try
            {
                // Set up server object and connection options.
                Server server = new Server(settings.Host);
                server.ConnectionContext.ApplicationName = ProgramName + " v" + ProgramVersion;
                server.ConnectionContext.ConnectTimeout = settings.Timeout;
                server.ConnectionContext.LoginSecure = settings.UseWindowsAuthentication;
                server.ConnectionContext.EncryptConnection = settings.UseEncryption;

                if (settings.User != null && settings.Password != null)
                {
                    server.ConnectionContext.Login = settings.User;
                    server.ConnectionContext.Password = settings.Password;
                }

                if (settings.PacketSize != null)
                {
                    server.ConnectionContext.PacketSize = (int) settings.PacketSize;
                }

                // Connect.
                server.ConnectionContext.Connect();

                if (server.ConnectionContext.IsOpen)
                {
                    // Set up scripter object and script options.
                    Scripter scripter = new Scripter(server);

                    // List of databases to dump.
                    List<string> databases = WhichDatabases(settings, extras, server);

                    // List of tables to dump.
                    List<string> tables = WhichTables(settings, extras, server);

                    // Extra SQL statements to be written out.
                    List<string> extraStatements = new List<string>();

                    // Set script options.
                    scripter.Options.DriAll = true;
                    scripter.Options.ScriptData = !settings.NoData;
                    scripter.Options.ScriptOwner = false;

                    foreach (string databaseName in databases)
                    {
                        Database database = server.Databases[databaseName];

                        // List of objects to dump.
                        List<Urn> urns = new List<Urn>();

                        // If no specific tables were requested, write out database info.
                        if (tables.Count == 0)
                        {
                            if (settings.IncludeCreateDatabase)
                            {
                                urns.Add(database.Urn);

                                foreach (ExtendedProperty extendedProperty in database.ExtendedProperties)
                                {
                                    urns.Add(extendedProperty.Urn);
                                }
                            }

                            if (settings.IncludeUsers)
                            {
                                // Also output users.
                                foreach (User user in database.Users)
                                {
                                    if (!user.IsSystemObject)
                                    {
                                        urns.Add(user.Urn);

                                        foreach (ExtendedProperty extendedProperty in user.ExtendedProperties)
                                        {
                                            urns.Add(extendedProperty.Urn);
                                        }

                                        StringCollection userRoles = user.EnumRoles();
                                        foreach (string userRole in userRoles)
                                        {
                                            // Can't find a way to make the code do this...
                                            extraStatements.Add(string.Format("ALTER ROLE [{0}] ADD MEMBER [{1}]", userRole, user.Name));
                                        }
                                    }
                                }
                            }

                            // Also output schemas.
                            foreach (Schema schema in database.Schemas)
                            {
                                if (!schema.IsSystemObject)
                                {
                                    urns.Add(schema.Urn);

                                    foreach (ExtendedProperty extendedProperty in schema.ExtendedProperties)
                                    {
                                        urns.Add(extendedProperty.Urn);
                                    }
                                }
                            }
                        }

                        // Loop through tables.
                        foreach (Table table in database.Tables)
                        {
                            // Output table if it was requested, or if no specific tables were requested.
                            if (tables.Count == 0 || tables.Contains(table.Name))
                            {
                                urns.Add(table.Urn);

                                foreach (ExtendedProperty extendedProperty in table.ExtendedProperties)
                                {
                                    urns.Add(extendedProperty.Urn);
                                }

                                foreach (Index index in table.Indexes)
                                {
                                    urns.Add(index.Urn);

                                    foreach (ExtendedProperty extendedProperty in index.ExtendedProperties)
                                    {
                                        urns.Add(extendedProperty.Urn);
                                    }
                                }

                                foreach (Trigger trigger in table.Triggers)
                                {
                                    urns.Add(trigger.Urn);

                                    foreach (ExtendedProperty extendedProperty in trigger.ExtendedProperties)
                                    {
                                        urns.Add(extendedProperty.Urn);
                                    }
                                }
                            }

                        }

                        // Loop through views.
                        foreach (View view in database.Views)
                        {
                            // Output view if it was requested, or if no specific views were requested.
                            if (tables.Count == 0 || tables.Contains(view.Name))
                            {
                                if (!view.IsSystemObject)
                                {
                                    urns.Add(view.Urn);

                                    foreach (ExtendedProperty extendedProperty in view.ExtendedProperties)
                                    {
                                        urns.Add(extendedProperty.Urn);
                                    }
                                }
                            }
                        }

                        // Don't forget stored procedures.
                        if (tables.Count == 0)
                        {
                            foreach (StoredProcedure storedProcedure in database.StoredProcedures)
                            {
                                if (!storedProcedure.IsSystemObject)
                                {
                                    urns.Add(storedProcedure.Urn);

                                    foreach (ExtendedProperty extendedProperty in storedProcedure.ExtendedProperties)
                                    {
                                        urns.Add(extendedProperty.Urn);
                                    }
                                }
                            }
                        }

                        // Dump everything.
                        if (urns.Count > 0)
                        {
                            WriteScripts(scripter.EnumScriptWithList(urns.ToArray()));
                        }

                        foreach (String extraStatement in extraStatements)
                        {
                            Console.WriteLine(extraStatement);
                        }
                    }
                }
                else
                {
                    Console.Error.WriteLine("Not connected to server.");
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error during process execution:");
                Console.Error.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Figure out which databases to dump.
        /// </summary>
        /// <param name="settings">Parsed command-line options</param>
        /// <param name="extras">Unparsed command-line options</param>
        /// <param name="server">Server object, used to pull list of databases from server</param>
        /// <returns>List of databases to dump</returns>
        private static List<string> WhichDatabases(ProgramSettings settings, List<string> extras, Server server)
        {
            List<string> databases = new List<string>();

            // First extra is database to dump.
            databases.Add(extras[0]);

            return databases;
        }

        /// <summary>
        /// Figure out which tables to dump.
        /// </summary>
        /// <param name="settings">Parsed command-line options</param>
        /// <param name="extras">Unparsed command-line options</param>
        /// <param name="server">Server object, used to pull list of databases from server</param>
        /// <returns>List of tables to dump</returns>
        private static List<string> WhichTables(ProgramSettings settings, List<string> extras, Server server)
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
        /// Write scripts to console.
        /// </summary>
        /// <param name="scripts">Collection of scripts to be written</param>
        private static void WriteScripts(StringCollection scripts)
        {
            foreach (string script in scripts)
            {
                Console.WriteLine(script);
            }
        }

        /// <summary>
        /// Write scripts to console.
        /// </summary>
        /// <param name="scripts">Collection of scripts to be written</param>
        private static void WriteScripts(IEnumerable<string> scripts)
        {
            foreach (string script in scripts)
            {
                Console.WriteLine(script);
            }
        }

        /// <summary>
        /// Show help message, which includes command-line option descriptions.
        /// </summary>
        /// <param name="options"></param>
        private static void ShowHelp(OptionSet options)
        {
            Console.WriteLine();
            Console.WriteLine(ProgramName + " v" + ProgramVersion);
            Console.WriteLine("Dumping structure and contents of Microsoft SQL Server databases and tables.");
            Console.WriteLine("Usage: ssdump [OPTIONS] database [table1 table2 ...]");
            Console.WriteLine();
            options.WriteOptionDescriptions(Console.Out);
        }
    }
}
