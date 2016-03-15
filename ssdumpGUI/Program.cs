using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace ssdump
{
    class Program
    {
        /// <summary>
        /// Program starts here.
        /// </summary>
        /// <param name="args">Command-line options</param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ssdumpGUI());
        }
    }
}
