# ssdump

## What is it?

ssdump is a command line tool for dumping a Microsoft SQL Server database.  It can dump the schema only (tables, views, stored procedures, etc.), or the schema with the data included.  The output is a SQL script that can be imported into another SQL Server instance.  It is intended to be similar to the tool "mysqldump" which provides similar functionality for MySQL and MariaDB databases.  mysqldump was used as a starting point when coming up with the command line parameters for ssdump.

It is possible to generate SQL script dumps like this using the GUI tool Microsoft SQL Server Management Studio (SSMS).  In SSMS, you can right-click on a database and select "Tasks &rarr; Generate Scripts" to get a GUI wizard that walks you through creating such a dump.  The intention of ssdump is to allow for this process to be automated or scripted.  In fact, ssdump links with the SQL Server Management Studio libraries in order to generate the scripts.  Most of the options available in the SSMS wizard should be available via the libraries as well, but I have no exposed all of them in this program.

I created ssdump with the intention of using it to dump the database schema only and then load that into a version control system, in order to track changes to the schema, stored procedure code, and so forth, because I was unable to find an existing tool that allowed this.  After I got it working, I added a few options that seemed useful and then tossed it up here on GitHub.

## Getting started

Run the application from the command line with no parameters to get some help text explaining how to use it and what the available parameters are.  Parameter format is similar to mysqldump.

A simple example run could look like:

```
ssdump --host=sqlserver.company.com --user=ssdump_user --password=BEgETDp4FnHu8xzzRMwr --no-data MyDatabase > output.sql
```

If no username or password is provided on the command line, ssdump will use Windows authentication.  Providing the uername and password uses SQL Server authentication.

ssdump uses libraries from SSMS.  You need to have SQL Server Management Studio 2014 installed in order for it to run.

ssdump writes the SQL script to the console and it can be redirected to a file using a batch script or any other standard scripting tool.  Like SSMS, currently, ssdump can only dump one database at a time.

## GUI

A GUI tool, ssdumpGUI, is available as well.  A window pops up with information to fill in, but the process is the same as the command-line tool.

## Development

If you clone this repository, what you will get is a Visual Studio 2015 solution that should have everything that you need to build the program yourself.  Free versions of Visual Studio (Express or Community) should work fine.  The solution includes references to SSMS DLLs, you will need to have SQL Server Management Studio 2014 installed.  The only other third-party dependency has been copied in for now, it is the file `ssdumpConsole/Mono.Options/Options.cs` which is a command line parameter parsing library from the [Mono project](https://github.com/mono/mono/tree/master/mcs/class/Mono.Options).

[This tutorial from MSSQLTips](https://www.mssqltips.com/sqlservertip/1833/generate-scripts-for-database-objects-with-smo-for-sql-server/) served as the starting point for this project.

## Support

If you find this tool useful and you have a feature that you would like to see added, submit an issue and I will take a look.  Or, implement it yourself and submit a pull request.

## License / Legal

You are free to fork or extend ssdump to suit your needs.  You may not bundle ssdump or any derivative as part of a commercial product.
